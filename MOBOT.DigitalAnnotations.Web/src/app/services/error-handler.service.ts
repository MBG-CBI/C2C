import { Injectable, ErrorHandler } from '@angular/core';
import {
  Subject,
  Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService implements ErrorHandler {
  private subject = new Subject<string[]>();

  handleError(error: any): void {
    console.log(error);
    setTimeout(() => {
      if (error instanceof Error) {
        this.subject.next([error.message]);
      } else {
        this.subject.next(['An error has occurred. Please contact support.']);
      }
    });
  }

  get errors(): Observable<string[]> {
    return this.subject;
  }

}
