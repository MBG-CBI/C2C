import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  AfterViewInit
} from '@angular/core';
import { ControlContainer, NgForm } from '@angular/forms';
import { Annotation, DisplayModes, Group, License, AnnotationType, Tag, VocabularyLookupResponse } from '../../models';
import {
  AnnotationService,
  AuthorizationService,
  LicenseService,
  AnnotationTypeService,
  TagService,
  VocabularyService
} from '../../services';

@Component({
  selector: 'app-annotation-detail',
  templateUrl: './annotation-detail.component.html',
  styleUrls: ['./annotation-detail.component.scss'],
  encapsulation: ViewEncapsulation.None,
  viewProviders: [
    { provide: ControlContainer, useExisting: NgForm }
  ]
})
export class AnnotationDetailComponent implements OnInit, AfterViewInit {
  public _annotation: Annotation;
  public displayModes = DisplayModes;
  public selectedUserGroup: Group;
  public licenses: License[] = [];
  public selectedLicense: License;
  public annotationTypes: AnnotationType[] = [];
  public selectedAnnotationType: AnnotationType;
  public tags: Tag[] = [];
  public selectedTags: Tag[] = [];
  public lookupResults: VocabularyLookupResponse[] = [];

  @Input() set annotation (value: Annotation) {
    this._annotation = value;
  }
  get annotation(): Annotation { return this._annotation; }
  get userGroups(): Group[] {
    return this.authService.user ? this.authService.userGroups : [];
  }

  get tagNames(): string {
    if (this._annotation.tags && this._annotation.tags.length > 0) {
      const tagNames = this._annotation.tags.map(t => {
        return t ? t.name : '';
      });
      return tagNames.join(', ');
    }
    return null;
  }

  private _index = 0;
  @Input() set index (value: number) {
    this._index = value;
  }

  private _length = 0;
  @Input() set length (value: number) {
    this._length = value;
  }

  // @ViewChild('annotationbody') annotationBody:

  get mode(): string { return this._annotation !== null && this._annotation.mode === DisplayModes.Edit ? 'Edit' : 'New'; }

  private _initializationState = false;
  get initializeState(): boolean {
    this._initializationState = this._index === this._length - 1;
    return this._initializationState;
  }

  get userCanEdit(): boolean {
    return !this._service.isInEditMode && this.authService.user && this.authService.user.id === this._annotation.createdUserId;
  }

  get userCanReply(): boolean {
    return !this._service.isInEditMode && this.authService.user && this.authService.user.id > 0;
  }

  constructor(private _service: AnnotationService,
    private authService: AuthorizationService,
    private _licenseService: LicenseService,
    private _annotationTypeService: AnnotationTypeService,
    private _tagService: TagService,
    private _vocabularyService: VocabularyService) {
  }

  ngOnInit() {
    if (this.userGroups && this.userGroups.length > 0) {
      this.selectedUserGroup = this.authService.userGroups[0];
    }

    this._licenseService.getList()
      .subscribe(data => {
        this.licenses = [...data];
        const license = this.licenses.find(l => l.id === this._annotation.licenseId);
        if (license) {
          this.selectedLicense = license;
        }
      });

    this._annotationTypeService.getList()
      .subscribe(data => {
        this.annotationTypes = [...data];
        const type = this.annotationTypes.find(at => at.id === this._annotation.annotationTypeId);
        if (type) {
          this.selectedAnnotationType = type;
        }
      });

    this._tagService.getList()
      .subscribe(data => {
        this.tags = [...data];
        // if (this._annotation.tags && this._annotation.tags.length > 0) {
        //   this._annotation.tags.forEach(t => {
        //     const found = this.tags.find(x => x.id === t.id);
        //     if (found) {
        //       this.selectedTags.push(found);
        //     }
        //   });
        // }
      });

    this._service.cancelCreateAnnotation.subscribe(annotation => {
      if (this._annotation.id === annotation.parentId) {
        const foundIdx = this._annotation.annotations.findIndex(a => a.id === annotation.id);
        if (foundIdx > -1) {
          this._annotation.annotations.splice(foundIdx, 1);
        }
      }
    });
    // this._annotation = this._service.currentAnnotation;
  }

  vocabularyLookup($event: string): void {
    // console.log($event);
    this._vocabularyService.lookup($event)
      .subscribe(data => {
        this.lookupResults = [...data];
      });
  }

  mentionSelect($event: VocabularyLookupResponse): string {
    // console.log($event);
    return $event.term.slice(1);
  }

  ngAfterViewInit(): void {
    // this.elem.nativeElement.collapse(() => {
    //   this._initializationState = false;
    // });
  }

  onEditAnnotationClicked(annotaion: Annotation) {
    this._annotation.mode = DisplayModes.Edit;
    this.selectedAnnotationType = this._annotation.annotationType;
    this.selectedLicense = this._annotation.license;
    this.selectedUserGroup = {id: this._annotation.groupId, name: this._annotation.groupName};
    this._service.editAnnotation(this._annotation);
  }

  cancelOperation(): void {
    // if (!this._annotation.parentId || this._annotation.parentId < 1) {
      this._service.cancelCreateOperation(this._annotation);
    // } else {
      // if (this._annotation.mode === DisplayModes.Create) {
      //   const matchIdx = this._annotation.annotations.findIndex(a => a.id === this._annotation.id);
      //   if (matchIdx > -1) {
      //     this._annotation.annotations.splice(matchIdx, 1);
      //   }
      // }
    // }

    if (this._annotation.mode === DisplayModes.Edit) {
      this._annotation.mode = DisplayModes.Display;
    }
  }

  replyToAnnotationClicked(annotation: Annotation): void {
      const reply = new Annotation(DisplayModes.Create);
      reply.body = null;
      reply.target = annotation.target;
      reply.parentId = annotation.id;
      reply.groupId = annotation.groupId;
      reply.annotationTypeId = 11; // reply
      annotation.annotations.push(reply);
      this._service.replyToAnnotation(reply);
  }

  save(): void {
    this._annotation.groupId = this.selectedUserGroup.id;
    // this._annotation.target = this._service.currentTarget;
    if (this._annotation.mode === DisplayModes.Create) {
      this._annotation.createdUserId = this.authService.user.id;
    } else if (this._annotation.mode === DisplayModes.Edit) {
      this._annotation.updatedUserId = this.authService.user.id;
    }
    this._annotation.licenseId = this.selectedLicense && this.selectedLicense.id && this.selectedLicense.id > 0 ?
      this.selectedLicense.id : null;
    this._annotation.annotationTypeId = this.selectedAnnotationType && this.selectedAnnotationType.id &&
      this.selectedAnnotationType.id > 0 ? this.selectedAnnotationType.id : null;
    if (this._annotation.mode === DisplayModes.Create) {
      this._service.postAnnotation(this._annotation).subscribe(resp => {
        this._annotation = resp;
        this._annotation.mode = DisplayModes.Display;
      });
    } else if (this._annotation.mode === DisplayModes.Edit) {
      this._service.putAnnotation(this._annotation).subscribe(resp => {
        this._annotation = resp;
        this._annotation.mode = DisplayModes.Display;
      });
    }
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id === c2.id : c1 === c2;
  }
}
