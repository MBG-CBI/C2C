import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VocabularyLookupResponse } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VocabularyService {

  constructor(private http: HttpClient) { }

  lookup(searchTerm: string): Observable<VocabularyLookupResponse[]> {
    const params = new HttpParams().set('searchTerm', searchTerm);
    return this.http.get<VocabularyLookupResponse[]>(`${environment.apiUrl}/Vocabulary/Lookup`, { params: params });
  }
}
