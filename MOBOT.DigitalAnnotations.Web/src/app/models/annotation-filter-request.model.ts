import { AnnotationFilterTypes } from './annotation-filter-types.enum';

export class AnnotationFilterRequest {
  filterType: AnnotationFilterTypes;
  filterId?: number;
  date?: Date;
}
