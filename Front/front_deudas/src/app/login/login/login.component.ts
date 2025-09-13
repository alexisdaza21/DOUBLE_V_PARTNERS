import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations: [
    trigger('fadeModal', [
      state('void', style({
        opacity: 0,
        transform: 'scale(0.8)'
      })),
      state('show', style({
        opacity: 1,
        transform: 'scale(1)'
      })),
      transition('void <=> show', [
        animate('300ms ease-in-out')
      ]),
    ])
  ]
})
export class LoginComponent {
  email = '';
  password = '';
  token = '';
  error = '';
  showRegisterModal = false;
  registerEmail = '';
  registerUsername = '';


  onLogin() {
  }


  openRegisterModal() {
    this.showRegisterModal = true;
  }

  closeRegisterModal() {
    this.showRegisterModal = false;
  }

  onRegister() {
  }

}
