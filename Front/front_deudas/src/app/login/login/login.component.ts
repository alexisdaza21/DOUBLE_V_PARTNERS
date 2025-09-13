import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { CommonService } from 'src/app/services/common/common.service';
import { Usuario } from '../../clases/usuario';
import { LoginService } from '../../services/login/login.service';

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

  showRegisterModal = false;

  _loginUser = new Usuario();
  _loginCreate = new Usuario();
  constructor(private commonService: CommonService, private loginService: LoginService) { }


  async onLogin() {
    const validate = await this.validarCampos(this._loginUser);
    if (validate) {
      this.loginService.login(this._loginUser).subscribe(
        (response: any) => {
          debugger
          var strTitulo = response.status == 200 ? 'Correcto' : 'Error';
          this.commonService.mostrarAlert(strTitulo, response.mensaje);
          this._loginCreate = new Usuario();
        }
      )
    }
  }


  openRegisterModal() {
    this.showRegisterModal = true;
  }

  closeRegisterModal() {
    this.showRegisterModal = false;
  }

  async onRegister() {
    const validate = await this.validarCampos(this._loginCreate);
    if (validate) {
      this.loginService.registrarUsuario(this._loginCreate).subscribe(
        (response: any) => {
          debugger
          var strTitulo = response.status == 200 ? 'Correcto' : 'Error';
          this.commonService.mostrarAlert(strTitulo, response.mensaje);
          this._loginCreate = new Usuario();
        }

      )
    }
  }

  async validarCampos(datos: Usuario) {

    const result = this.commonService.validateModel(datos)
    if (result.length > 0) {
      let strErrores = '';
      for (const error of result) {
        for (const constraint in error.constraints) {
          if (Object.prototype.hasOwnProperty.call(error.constraints, constraint)) {
            strErrores += `- ${error.constraints[constraint]} \n `;
          }
        }
      }
      this.commonService.mostrarAlert('Error', strErrores);

      return false;
    } else {
      return true;
    }

  }


}
