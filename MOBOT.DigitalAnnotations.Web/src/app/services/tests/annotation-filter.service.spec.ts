import { TestBed } from '@angular/core/testing';

import { AnnotationFilterService } from '../annotation-filter.service';

describe('AnnotationFilterService', () => {
  let service: AnnotationFilterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnnotationFilterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
