import { Injectable, EventEmitter } from '@angular/core';
import { User, Group } from '../models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private _user: User;
  private _userGroups: Group[] = [];
  public get userGroups(): Group[] {
    return this._userGroups;
  }

  public get user(): User {
    return this._user;
  }

  constructor(private http: HttpClient) { }

  public login(userName: string, password: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.post<User>(`${environment.apiUrl}/Authorization/Login`, {userName: userName, password: password})
        .subscribe(resp => {
          if (resp && resp.id > 0) {
            this._user = {...resp};
            this.getGroupsFromUser(this._user);
            resolve(true);
          }
        }, error => reject(error.error));
      });
  }

  public logout(): void {
    this._user = null;
    this._userGroups = [];
  }

  public isUserInGroup(groupId: number): boolean {
    if (groupId === 1 || groupId === 2) {
      return true;
    }
    if (this._user) {
      return groupId === 3 || this._user.groups.findIndex(ug => ug.groupId === groupId) > -1;
    }

  }

  private getGroupsFromUser(user: User): void {
    this._userGroups.splice(0, this.userGroups.length);
    user.groups.forEach(ug => {
      this._userGroups.push(ug.group);
    });
  }
}
