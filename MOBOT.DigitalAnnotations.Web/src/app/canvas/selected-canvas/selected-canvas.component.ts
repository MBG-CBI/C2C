
import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  ViewChild,
  ElementRef
} from '@angular/core';
import { AnnotationTarget } from '../../models';
import { AnnotationService } from '../../services';

@Component({
  selector: 'app-selected-canvas',
  templateUrl: './selected-canvas.component.html',
  styleUrls: ['./selected-canvas.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectedCanvasComponent implements OnInit {
  private _width = 0;
  private _height = 0;
  private _context: CanvasRenderingContext2D;
  private _isDrawing = false;
  private _lineWidth = 1;
  private _strokeStyle = 'green';
  private _clickedTarget: AnnotationTarget = null;

  @Input('height') set height (value: number) {
    this._height = value;
  }
  @Input('width') set width (value: number) {
    this._width = value;
  }

  get heightString(): string { return `${this._height}px`; }
  get widthString(): string { return `${this._width}px`; }

  @ViewChild('selectedcanvas', {static: true}) canvas: ElementRef;

  constructor(private _annotationService: AnnotationService) { }

  ngOnInit() {
    this._context = (<HTMLCanvasElement>this.canvas.nativeElement).getContext('2d');
    this._annotationService.onCurrentTargetClicked.subscribe(t => {
      if (this._clickedTarget) {
        this._clickedTarget.clearHighlight(this._context);
      }
      if (t) {
        const target = t as AnnotationTarget;
        this._clickedTarget = target;
        this.setHightlightContext();
        target.highlight(this._context);
      }
    });

    this._annotationService.onDrawingStarted.subscribe(() => {
      if (this._clickedTarget) {
        this._clickedTarget.clearHighlight(this._context);
        this._clickedTarget = null;
      }
    });
  }

  private setHightlightContext(): void {
    this._context.lineWidth = 0;
    this._context.fillStyle = 'rgb(255, 255, 0, 0.2)';
  }

}
