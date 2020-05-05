import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NoopAnimationsModule, BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule,
  ReactiveFormsModule
} from '@angular/forms';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatSelectModule} from '@angular/material/select';

import { AppComponent } from './app.component';
import { RoutingConfig } from './app.routing';
import { TopNavComponent } from './top-nav/top-nav.component';
import {HomeComponent} from './home/home.component';
import { Error400Component } from './errors';
import {
  AnnotationListComponent,
  AnnotationDetailComponent
} from './annotations';
import {
  AnnotationService,
  ErrorHandlerService,
  RequestInterceptorService,
  AnnotationSourceService
} from './services';
import { RenderCanvasComponent } from './canvas/render-canvas/render-canvas.component';
import { CoverCanvasComponent } from './canvas/cover-canvas/cover-canvas.component';
import { SelectedCanvasComponent } from './canvas/selected-canvas/selected-canvas.component';
import { LoginComponent } from './login/login.component';
import { AnnotationFilterComponent } from './annotations/annotation-filter/annotation-filter.component';
import { MentionModule } from 'angular-mentions';

const errorHandler = new ErrorHandlerService();
export function handlerFactory(): ErrorHandlerService {
  return errorHandler;
}

@NgModule({
  declarations: [
    AppComponent,
    TopNavComponent,
    HomeComponent,
    Error400Component,
    AnnotationListComponent,
    AnnotationDetailComponent,
    RenderCanvasComponent,
    CoverCanvasComponent,
    SelectedCanvasComponent,
    LoginComponent,
    AnnotationFilterComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    NoopAnimationsModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatIconModule,
    MatChipsModule,
    MatMenuModule,
    MatToolbarModule,
    MatButtonModule,
    MatSelectModule,
    MentionModule,
    RoutingConfig
  ],
  exports: [
  ],
  providers: [
    { provide: ErrorHandlerService, useFactory: handlerFactory },
    { provide: ErrorHandler, useFactory: handlerFactory },
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    AnnotationService,
    AnnotationSourceService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
