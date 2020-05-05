import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';

declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  userName = new FormControl('', Validators.required);
  password = new FormControl('', Validators.required);
  public showError = false;
  constructor(private fb: FormBuilder, private authService: AuthorizationService) {
    this.loginForm = fb.group({
      'userName': this.userName,
      'password': this.password
    });

  }

  ngOnInit(): void {
  }

  show(): void {
    $('#login-dialog').modal('show');
  }

  hide(): void {
    $('#login-dialog').modal('hide');
  }

  login(): void {
    this.authService.login(this.userName.value, this.password.value)
      .then(this.hide)
      .catch( reason => {
        this.showError = true;
      });
  }

}
