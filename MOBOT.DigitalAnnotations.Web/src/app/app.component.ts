import { Component, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { ErrorHandlerService } from './services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements AfterViewInit {
  private lastError: string[];

  constructor(errorHandler: ErrorHandlerService) {
    errorHandler.errors.subscribe(error => {
      this.lastError = error;
    });
   }

  ngAfterViewInit(): void {}

  get error(): string[] {
    return this.lastError;
  }

  clearError(): void {
    this.lastError = null;
  }
}
