import { TestBed } from '@angular/core/testing';

import { AnnotationSourceService } from '../annotation-source.service';

describe('AnnotationSourceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AnnotationSourceService = TestBed.get(AnnotationSourceService);
    expect(service).toBeTruthy();
  });
});
