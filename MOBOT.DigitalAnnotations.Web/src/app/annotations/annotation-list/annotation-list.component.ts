import {
  Component,
  OnInit,
  ViewEncapsulation,
  AfterViewInit
} from '@angular/core';

import {
  AnnotationTarget, Annotation, DisplayModes
} from '../../models';
import { AnnotationService, AuthorizationService } from '../../services';

@Component({
  selector: 'app-annotation-list',
  templateUrl: './annotation-list.component.html',
  styleUrls: ['./annotation-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AnnotationListComponent implements OnInit, AfterViewInit {
  public get annotations(): Annotation[] {
    const userId = this._authService.user ? this._authService.user.id : 0;
    const filtered = this._service.annotations.filter(a => {
      if (a.mode === DisplayModes.Create || a.groupId === 1 || ((userId > 0) && (a.createdUserId === userId ||
        a.groupId === 3)) || (this._authService.userGroups.findIndex(g => g.id === a.groupId) > -1)) {
          return true;
        }
        return false;
    });
    return filtered;
  }
  currentTarget: AnnotationTarget = null;

  constructor(private _service: AnnotationService,
    private _authService: AuthorizationService) { }

  ngOnInit() {
    this._service.onCurrentTargetClicked.subscribe(data => {
      if (data && !this._service.isInEditMode) {
        this.currentTarget = data as AnnotationTarget;
      }
    });

  }

  ngAfterViewInit(): void {
  }

  newAnnotation(): void {
    this._service.createAnnotation(null, this.currentTarget);
  }

  canAdd(): string {
    return !this._service.isInEditMode && this._authService.user && this._authService.user.id > 0 ? 'visible' : 'hidden';
  }

}
