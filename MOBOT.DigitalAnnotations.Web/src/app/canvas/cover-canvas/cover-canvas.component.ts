
import {
  Component,
  OnInit,
  ViewEncapsulation,
  ViewChild,
  AfterViewInit,
  ElementRef,
  Input
} from '@angular/core';
import { AnnotationService, AuthorizationService } from '../../services';
import { AnnotationTarget, MouseCoordinates } from '../../models';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-cover-canvas',
  templateUrl: './cover-canvas.component.html',
  styleUrls: ['./cover-canvas.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CoverCanvasComponent implements OnInit, AfterViewInit {
  private _isDrawing = false;
  private _startX = 0;
  private _startY = 0;
  private _width = 0;
  private _height = 0;
  private _currentTarget: AnnotationTarget = null;
  private _lineWidth = 1;
  private _strokeStyle = 'red';
  private _lineDash = 6;

  private _context: CanvasRenderingContext2D;

  @Input('height') set height (value: number) {
    this._height = value;
  }
  @Input('width') set width (value: number) {
    this._width = value;
  }

  get heightString(): string { return `${this._height}px`; }
  get widthString(): string { return `${this._width}px`; }

  get currentTarget(): Observable<AnnotationTarget> { return of(this._currentTarget); }
  get canCreateAnnotation(): boolean {
    return this._authService.user && this._authService.user.id > 0;
  }

  @ViewChild('canvas', {static: true}) canvas: ElementRef;

  constructor(private _annotationService: AnnotationService,
    private _authService: AuthorizationService) { }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this._context = (<HTMLCanvasElement>this.canvas.nativeElement).getContext('2d');
    this._annotationService.cancelCreateAnnotation.subscribe(() => {
      this._context.clearRect(0, 0, this._width, this._height);
    });
    this._annotationService.annotationCreated.subscribe(() => {
      this._context.clearRect(0, 0, this._width, this._height);
    });
    this._annotationService.onHighlightTarget.subscribe((target: AnnotationTarget) => {
        if (!this._isDrawing && !this._annotationService.isInEditMode) {
          this._context.clearRect(0, 0, this._width, this._height);
          if (target) {
            this.setHightlightContext();
            target.highlight(this._context);
            // this._currentTarget = target;
            (<HTMLElement>this.canvas.nativeElement).style.cursor = 'pointer';
          } else {
            (<HTMLElement>this.canvas.nativeElement).style.cursor = 'default';
          }
        }
    });
    this._annotationService.onClearHighlight.subscribe((target: AnnotationTarget) => {
      if (target) {
        target.clearHighlight(this._context);
      }
    });
  }

  onMouseUp(event: MouseEvent): void {
    if (!this._annotationService.isInEditMode) {
      const bcr: any = (<HTMLCanvasElement>this.canvas.nativeElement).getBoundingClientRect();
      const mouseX = event.clientX - bcr.left;
      const mouseY = event.clientY - bcr.top;
      (<HTMLElement>this.canvas.nativeElement).style.cursor = 'default';
      if (this._isDrawing && Math.ceil(this._startX) !== Math.ceil(mouseX) &&
        Math.ceil(this._startY) !== Math.ceil(mouseY) &&
          mouseX - this._startX > 0 && mouseY - this._startY > 0) {
        (<HTMLElement>this.canvas.nativeElement).style.cursor = 'default';
        this._annotationService.createAnnotation(null, this._currentTarget);
      } else if (this._annotationService.currentTarget !== null) {
          this._context.clearRect(0, 0, this._width, this._height);
          this._annotationService.currentTargetClicked();
      }
      this._isDrawing = false;
    }
  }

  onMouseDown(event: MouseEvent): void {
    if (!this._annotationService.isInEditMode && this.canCreateAnnotation) {
      this._annotationService.startDrawing();
      this._isDrawing = true;
      const bcr: any = (<HTMLCanvasElement>this.canvas.nativeElement).getBoundingClientRect();
      this._startX = event.clientX - bcr.left;
      this._startY = event.clientY - bcr.top;
      (<HTMLElement>this.canvas.nativeElement).style.cursor = 'crosshair';
    }
  }

  onMouseMove(event: MouseEvent): void {
    const bcr: any = (<HTMLCanvasElement>this.canvas.nativeElement).getBoundingClientRect();
    const mouseX = event.clientX - bcr.left;
    const mouseY = event.clientY - bcr.top;
    if (this._isDrawing) {
      this._context.clearRect(0, 0, this._width, this._height);
      const rectangle = new Path2D();
      this._currentTarget = new AnnotationTarget(this._startX, this._startY, mouseX - this._startX, mouseY - this._startY);
      rectangle.rect(this._startX, this._startY, mouseX - this._startX, mouseY - this._startY);
      this.setContext();
      this._currentTarget.draw(this._context);
    } else {
      this._annotationService.mouseMove(new MouseCoordinates(mouseX, mouseY));
    }
  }

  private setContext(): void {
    this._context.lineWidth = this._lineWidth;
    this._context.strokeStyle = this._strokeStyle;
    this._context.setLineDash([this._lineDash]);
  }

  private setHightlightContext(): void {
    this._context.lineWidth = 0;
    this._context.fillStyle = 'rgb(255, 255, 0, 0.5)';
  }
}
