import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnnotationFilterType } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnnotationFilterService {

  constructor(private http: HttpClient) { }

  getList(annotationSourceId: number): Observable<AnnotationFilterType[]> {
    return this.http.get<AnnotationFilterType[]>(`${environment.apiUrl}/AnnotationFilters/Source/${annotationSourceId}`);
  }
}
