import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

import { ApplicationError } from '../models';

@Injectable({
  providedIn: 'root'
})
export class RequestInterceptorService implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      map((event: HttpEvent<any>) => {
        return event;
      }),
      catchError((error: HttpErrorResponse) => this.handleError(error))
    );
    // .do((event: HttpEvent<any>) => {}, (err: any) => {
    //   if (err instanceof HttpErrorResponse) {
    //     // do error handling here
    //     const error = {...err};
    //     this.handleError(error);
    //   }
    // });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
        console.error('An error occurred:', error.error.message);
        throwError(new Error(`Network Error. Url: ${error.url}.`));
    } else if (error.status === 400) {
        const messages: string[] = [];
        messages.push(error.message);
        const err = new ApplicationError(messages);
        err.name = error.name;
        return throwError(new ApplicationError(messages));
    } else if (error.status === 401) {
        console.log(error);
        this.router.navigateByUrl('/login');
    } else if (error.status === 500) {
        return throwError(new Error(`Internal server error.  Url: ${error.url}.`));
    }
    return throwError(new Error(`Network Error. Url: ${error.url}.`));

  }

}
