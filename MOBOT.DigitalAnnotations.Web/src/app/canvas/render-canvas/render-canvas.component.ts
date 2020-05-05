import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import { Annotation, AnnotationTarget } from '../../models';
import { AnnotationService, AnnotationSourceService } from '../../services';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-render-canvas',
  templateUrl: './render-canvas.component.html',
  styleUrls: ['./render-canvas.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RenderCanvasComponent implements OnInit, AfterViewInit, OnChanges {
  private _width = 0;
  private _height = 0;
  private _context: CanvasRenderingContext2D;
  private _targets: AnnotationTarget[] = [];
  private _lineWidth = 1;
  private _strokeStyle = 'green';

  @Input('height') set height (value: number) {
    this._height = value;
  }
  @Input('width') set width (value: number) {
    this._width = value;
  }
  @Input('targets') set targets (value: AnnotationTarget[]) {
    this._targets =  value;
  }
  get targets(): AnnotationTarget[] { return this._targets; }

  get heightString(): string { return `${this._height}px`; }
  get widthString(): string { return `${this._width}px`; }
  // public get targets(): Observable<AnnotationTarget[]> { return of(this._targets); }

  @ViewChild('rendercanvas', {static: true}) canvas: ElementRef;

  constructor(private _annotationService: AnnotationService,
    private _annotationSourceService: AnnotationSourceService) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['targets']) {
      if (this._targets && this._targets.length > 0) {
        this._context.clearRect(0, 0, this._width, this._height);
        this._targets.forEach(t => {
          const target = new AnnotationTarget(t.coordinateX, t.coordinateY, t.width, t.height);
          target.id = t.id;
          target.sourceId = t.sourceId;
          this.setContext();
          target.draw(this._context);
        });
      }
    }
  }


  ngAfterViewInit(): void {
    this._context = (<HTMLCanvasElement>this.canvas.nativeElement).getContext('2d');
    this._annotationService.annotationCreated.subscribe((annotation: Annotation) => {
      const rect = new Path2D();
      const target = new AnnotationTarget(annotation.target.coordinateX, annotation.target.coordinateY,
        annotation.target.width, annotation.target.height);
      target.id = annotation.target.id;
      target.sourceId = annotation.target.sourceId;
      target.source = annotation.target.source;
      if (this._targets.findIndex(t => t.id === target.id) < 0) {
        rect.rect(target.coordinateX, target.coordinateY, target.width, target.height);
        this.setContext();
        target.draw(this._context);
        this._targets.push(target);
      }
    });

    // this._annotationSourceService.imageLoaded.subscribe((source) => {
    //   if (source && source.id > 0) {
    //     const sourceId = source.id;
    //     this._annotationSourceService.getTargets(sourceId).subscribe(targets => {
    //       this._targets.splice(0, this._targets.length);
    //       targets.forEach(t => {
    //         const target = new AnnotationTarget(t.coordinateX, t.coordinateY, t.width, t.height);
    //         target.id = t.id;
    //         target.sourceId = t.sourceId;
    //         this.setContext();
    //         target.draw(this._context);
    //         this._targets.push(target);
    //       });
    //     });

    //   }
    // });

    // this._annotationSourceService.annotationSource.subscribe(data => {
    //   if (data && data.id > 0) {
    //     const sourceId = data.id;
    //     this._annotationSourceService.getTargets(sourceId).subscribe(targets => {
    //       this._targets.splice(0, this._targets.length);
    //       targets.forEach(t => {
    //         const target = new AnnotationTarget(t.coordinateX, t.coordinateY, t.width, t.height);
    //         target.id = t.id;
    //         target.sourceId = t.sourceId;
    //         this.setContext();
    //         target.draw(this._context);
    //         this._targets.push(target);
    //       });
    //     });
    //   }
    // });

    this._annotationService.onMouseMove.subscribe(c => {
      const target = this.findTarget(c.x, c.y);
      if (target) {
        this._annotationService.highlightTarget(target);
      } else {
        this._annotationService.clearTarget();
      }
    });

  }

  findTarget(x: number, y: number): AnnotationTarget {
    let target: AnnotationTarget = null;
    let smallestBox = Number.MAX_VALUE;
    const found = this._targets.filter(t => t.contains(x, y));
    if (found) {
      found.forEach(t => {
        if (t.width * t.height < smallestBox) {
          target = t;
          smallestBox = t.width * t.height;
        }
      });
    }

    return target;
  }

  private setContext(): void {
    this._context.lineWidth = this._lineWidth;
    this._context.strokeStyle = this._strokeStyle;
  }
}
