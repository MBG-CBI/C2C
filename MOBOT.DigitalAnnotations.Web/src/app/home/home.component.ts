import {
  Component,
  ViewChild,
  OnInit,
  ViewEncapsulation
} from '@angular/core';

import { Router, ActivatedRoute, Params } from '@angular/router';
import { AnnotationSourceService, AnnotationService } from '../services';
import {
  AnnotationSource,
  AnnotationTarget
} from '../models';
import { AnnotationListComponent } from '../annotations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {
  private _width = 0;
  private _height = 0;

  public get height(): number {return this._height; }
  public get width(): number {return this._width; }

  private _imageUrl: string = null;
  get imageUrl(): string { return this._imageUrl; }
  private _annotationSource: AnnotationSource = new AnnotationSource();
  get annotationSource(): AnnotationSource { return this._annotationSource; }

  // @ViewChild(AnnotationListComponent)annotationList: AnnotationListComponent;

  public title = 'Home';

  constructor(private _service: AnnotationSourceService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const imageUri = params['imageSource'];
      console.log(imageUri);
      if (!imageUri) {
        this.router.navigate(['/400']);
      } else {
        this._imageUrl = imageUri;
        const model = new AnnotationSource();
        model.sourceUrl = this._imageUrl;
        this._annotationSource = model;
      }
    });
  }

  onImageLoaded(img: any): void {
    this._height = img.clientHeight;
    this._width = img.clientWidth;
    const model = new AnnotationSource();
    model.imageWidth = this._width;
    model.imageHeight = this.height;
    model.sourceUrl = this._imageUrl;
    this._service.create(model).subscribe((source: AnnotationSource) => {
      model.createdDate = new Date(source.createdDate);
      model.createdUser = source.createdUser;
      model.id = source.id;
      model.imageHeight = source.imageHeight;
      model.imageWidth = source.imageWidth;
      model.rerumStorageUrl = source.rerumStorageUrl;
      model.sourceUrl = source.sourceUrl;
      if (source.targets) {
        model.targets = [];
        source.targets.forEach((t) => {
          const target = new AnnotationTarget();
          target.id = t.id;
          target.coordinateX = t.coordinateX;
          target.coordinateY = t.coordinateY;
          target.height = t.height;
          target.width = t.width;
          target.sourceId = model.id;
          model.targets.push(target);
        });
      }
      model.updateDate = source.updateDate;
      model.updatedUser = source.updatedUser;
      this._annotationSource = model;
    });

    // this._service.getTargets(this._annotationSource.id).subscribe((targets) => {
    //   this._annotationSource.targets.splice(0, this._annotationSource.targets.length);
    //   targets.forEach(t => {
    //     const target = new AnnotationTarget(t.coordinateX, t.coordinateY, t.width, t.height);
    //     target.id = t.id;
    //     target.sourceId = t.sourceId;
    //     this._annotationSource.targets.push(target);
    //   });

    // });
  }

}
