import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TopNavComponent implements OnInit {
  @ViewChild(LoginComponent) public loginComponent: LoginComponent;
  get isLoggedIn(): boolean {
    return this.auth.user && this.auth.user.id > 0;
  }

  constructor(private auth: AuthorizationService) { }

  ngOnInit() {
  }

  getPersonColor(): string {
    return this.isLoggedIn ? 'black' : 'darkgrey';
  }

  logIn(): void {
    this.loginComponent.show();
  }

  logout(): void {
    this.auth.logout();
  }
}
