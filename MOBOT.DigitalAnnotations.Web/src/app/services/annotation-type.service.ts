import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnnotationType } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnnotationTypeService {

  constructor(private http: HttpClient) { }

  public getList(): Observable<AnnotationType[]> {
    return this.http.get<AnnotationType[]>(`${environment.apiUrl}/AnnotationType/List`);
  }
}
