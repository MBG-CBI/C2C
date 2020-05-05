import { TestBed } from '@angular/core/testing';

import { AnnotationTypeService } from '../annotation-type.service';

describe('AnnotationTypeService', () => {
  let service: AnnotationTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnnotationTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
