import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import {
  Annotation,
  AnnotationSource,
  AnnotationTarget,
  DisplayModes,
  MouseCoordinates,
  AnnotationFilterRequest,
  AnnotationRequest
} from '../models';
import { environment } from '../../environments/environment';
import { AnnotationSourceService } from './annotation-source.service';
import { map, finalize } from 'rxjs/operators';
import { AuthorizationService } from './authorization.service';


@Injectable({
  providedIn: 'root'
})
export class AnnotationService {
  private _annotations: Annotation[] = [];
  public get annotations(): Annotation[] {
    return this._annotations;
  }
  public annotationSource: AnnotationSource;
  public currentAnnotation: Annotation;
  public currentTarget: AnnotationTarget = null;
  public isInEditMode = false;

  public cancelCreateAnnotation  = new EventEmitter<Annotation>();
  public annotationCreated = new EventEmitter<Annotation>();
  public annotationCreating = new EventEmitter<Annotation>();
  public onMouseMove = new EventEmitter<MouseCoordinates>();
  public onHighlightTarget = new EventEmitter<AnnotationTarget>();
  public onClearHighlight = new EventEmitter<AnnotationTarget>();
  public onCurrentTargetClicked = new EventEmitter<AnnotationTarget>();
  public onDrawingStarted = new EventEmitter();

  private _filters: AnnotationFilterRequest[] = [];
  // public onCurrentTargetSelected = new EventEmitter<AnnotationTarget>();
  constructor(private _annotationSourceService: AnnotationSourceService,
    private http: HttpClient,
    private authService: AuthorizationService) {
    this._annotationSourceService.annotationSource.subscribe(data => {
      this.annotationSource = {...data};
    });
  }

  postAnnotation(annotation: Annotation): Observable<Annotation> {
    if (!annotation.parentId || annotation.parentId < 1) {
       this._annotations.pop();
    }
    return this.http.post<Annotation>(`${environment.apiUrl}/Annotations`, annotation)
      .pipe(map(response => {
        const added = this.convertToAnnotation(response);
        if (!added.parentId || added.parentId < 1) {
          this._annotations.push(added);
        } else {
          const found = this._annotations.find(a => a.id === annotation.parentId);
          if (found) {
            const idx = found.annotations.findIndex(a => a.id === annotation.id);
            if (idx > -1) {
              found[idx] = added;
            }
          }
        }
        this.isInEditMode = false;
        this.annotationCreated.emit(added);
        return response;
      }));
  }

  putAnnotation(annotation: Annotation): Observable<Annotation> {
    return this.http.put<Annotation>(`${environment.apiUrl}/Annotations`, annotation)
      .pipe(map(response => {
        const updated = this.convertToAnnotation(response);
        return updated;
      }), finalize(() => {
        this.currentAnnotation = null;
        this.isInEditMode = false;
      }));
  }

  createAnnotation(body: string, target: AnnotationTarget): void {
    const annotation = new Annotation(DisplayModes.Create);
    annotation.body = body;
    annotation.target = target;
    annotation.targetId = target.id;
    annotation.target.sourceId = target.sourceId || this.annotationSource.id;
    this.currentAnnotation = annotation;
    this.isInEditMode = true;
    this._annotations.push(annotation);
    this.annotationCreating.emit(annotation);
    this.currentAnnotation = annotation;
    this.currentTarget = target;
  }

  replyToAnnotation(annotation: Annotation): void {
    if (annotation) {
      this.currentAnnotation = annotation;
      this.isInEditMode = true;
      this.currentTarget = annotation.target;
      this.annotationCreating.emit(annotation);
    }
  }

  editAnnotation(annotation: Annotation) {
    this.currentAnnotation = annotation;
    this.isInEditMode = true;
    this.currentTarget = annotation.target;
  }

  cancelCreateOperation(annotation: Annotation): void {
    this.currentAnnotation = null;
    this.isInEditMode = false;
    if (annotation.mode === DisplayModes.Create) {
      if (!annotation.parentId || annotation.parentId < 1) {
        const matchIdx = this._annotations.findIndex(a => a.id === annotation.id);
        if (matchIdx > -1) {
          this._annotations.splice(matchIdx, 1);
        }
      } else {
        this.cancelCreateAnnotation.emit(annotation);
      }
    }
  }

  startDrawing(): void {
    this._annotations.splice(0, this._annotations.length);
    this.onDrawingStarted.emit();
  }

  mouseMove(coordinates: MouseCoordinates): void {
    this.onMouseMove.emit(coordinates);
  }

  highlightTarget(target: AnnotationTarget): void {
      this.currentTarget = target;
      this.onHighlightTarget.emit(target);
  }

  currentTargetClicked(): void {
    this.loadAnnotations();
    this.onCurrentTargetClicked.emit(this.currentTarget);
  }

  applyFilters(searchText: string, filters: AnnotationFilterRequest[]) {
    this._filters = filters;
    this.loadAnnotations(searchText);
  }

  search(text: string) {
    this.loadAnnotations(text);
  }

  clearFilters(): void {
    this._filters.splice(0, this._filters.length);
    if (this.currentTarget && this.currentTarget.id > 0) {
      this.loadAnnotations();
    } else {
      this._annotations.splice(0, this._annotations.length);
    }
  }

  clearTarget(): void {
    if (this.currentTarget) {
      this.onClearHighlight.emit(this.currentTarget);
      this.currentTarget = null;
    }
  }

  private loadAnnotations(searchText?: string) {
    const request = new AnnotationRequest();
    request.sourceId = this.annotationSource.id;
    request.targetId = this.currentTarget && this.currentTarget.id > 0 ? this.currentTarget.id : null;
    request.filters = this._filters;
    request.searchText = searchText && searchText.length > 0 ? searchText : null;
    this._annotations.splice(0, this._annotations.length);
    this.http.post<Annotation[]>(`${environment.apiUrl}/Annotations/List`, request)
      .subscribe((data) => {
        if (data) {
          data.forEach(a => {
            const annotation = this.convertToAnnotation(a);
            this._annotations.push(annotation);
          });
        }
      });
  }

  private convertToAnnotation(a: Annotation): Annotation {
    const target = new AnnotationTarget(a.target.coordinateX, a.target.coordinateY, a.target.width, a.target.height);
    const annotation = new Annotation(DisplayModes.Display);
    annotation.id = a.id;
    annotation.subject = a.subject;
    annotation.parentId = a.parentId;
    annotation.body = a.body;
    annotation.createdDate = new Date(a.createdDate);
    annotation.createdUser = a.createdUser;
    annotation.createdUserId = a.createdUserId;
    annotation.subject = a.subject;
    annotation.licenseId = a.licenseId;
    annotation.license = a.license;
    annotation.groupId = a.groupId;
    annotation.groupName = a.groupName;
    annotation.annotationTypeId = a.annotationTypeId;
    annotation.annotationType = a.annotationType;
    target.id = a.target.id;
    target.source = a.target.source;
    target.sourceId = a.target.sourceId;
    annotation.target = target;
    annotation.target.id = a.target.id;
    annotation.updateDate = a.updateDate !== null ? new Date(a.updateDate) : null;
    annotation.updatedUser = a.updatedUser;
    annotation.updatedUserId = a.updatedUserId;
    annotation.tags = a.tags;
    a.annotations.forEach(an => {
      annotation.annotations.push(this.convertToAnnotation(an));
      // return ;
    });
    return annotation;
  }

}
