import { AnnotationFilterTypes } from './annotation-filter-types.enum';
import { AnnotationFilter } from './annotation-filter.model';

export class AnnotationFilterType {
  type: AnnotationFilterTypes;
  filterName: string;
  filters: AnnotationFilter[] = [];
}
