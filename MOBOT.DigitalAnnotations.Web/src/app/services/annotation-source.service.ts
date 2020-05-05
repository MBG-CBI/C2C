import { Injectable} from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';

import { environment } from '../../environments/environment';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { AnnotationSource, AnnotationTarget } from '../models';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AnnotationSourceService {
  private _annotationSource: AnnotationSource = new AnnotationSource();
  private onAnnotationSourceChanged = new BehaviorSubject<AnnotationSource>(this._annotationSource);
  public annotationSource: Observable<AnnotationSource> = this.onAnnotationSourceChanged.asObservable();

  private onImageLoaded = new BehaviorSubject<AnnotationSource>(this._annotationSource);
  public imageLoaded: Observable<AnnotationSource> = this.onImageLoaded.asObservable();

  constructor(private http: HttpClient, router: Router) {
  }

  public getBySourceUrl(url: string): Observable<AnnotationSource> {
    let source: Observable<AnnotationSource>;
    this.http.get<AnnotationSource>(`${environment.apiUrl}/AnnotationSources`, {
      params: new HttpParams()
        .set('sourceUrl', url)
    }).subscribe((data: AnnotationSource) => {
      if (!data) {
        const model = new AnnotationSource();
        model.sourceUrl = url;
        source = this.create(model);
      }
    });
    return source;
  }

  getTargets(sourceId: number): Observable<AnnotationTarget[]> {
    return this.http.get<AnnotationTarget[]>(`${environment.apiUrl}/AnnotationSources/${sourceId}/Targets`);
  }

  public create(model: AnnotationSource): Observable<AnnotationSource> {
    return this.http.post<AnnotationSource>(`${environment.apiUrl}/AnnotationSources`, model)
      .pipe(map((response: AnnotationSource) => {
        this._annotationSource = {...response};
        this.onAnnotationSourceChanged.next(this._annotationSource);
        return this._annotationSource;
      }));
  }

  public update(model: AnnotationSource): Observable<AnnotationSource> {
    return this.http.put<AnnotationSource>(`${environment.apiUrl}/AnnotationSources`, model)
      .pipe(map((response: AnnotationSource) => {
      this._annotationSource = {...response};
      this.onAnnotationSourceChanged.next(this._annotationSource);
      return this._annotationSource;
    }));
  }

  public updateHeight(newHeight: number): Observable<AnnotationSource> {
    this._annotationSource.imageHeight = newHeight;
    this.onAnnotationSourceChanged.next(this._annotationSource);
    return this.update(this._annotationSource);
  }

  public updateWidth(newWidth: number): Observable<AnnotationSource> {
    this._annotationSource.imageWidth = newWidth;
    this.onAnnotationSourceChanged.next(this._annotationSource);
    return this.update(this._annotationSource);
  }
}
