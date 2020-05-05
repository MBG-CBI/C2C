import { AnnotationFilterRequest } from './annotation-filter-request.model';

export class AnnotationRequest {
  sourceId?: number;
  targetId?: number;
  searchText: string;
  filters: AnnotationFilterRequest[] = [];
}
