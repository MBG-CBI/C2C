import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tag } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  constructor(private http: HttpClient) { }

  public getList(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${environment.apiUrl}/Tags/List`);
  }
}
