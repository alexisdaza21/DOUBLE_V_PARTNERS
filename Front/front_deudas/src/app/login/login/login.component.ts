import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { CommonService } from 'src/app/services/common/common.service';
import { Usuario } from '../../clases/usuario';
import { LoginService } from '../../services/login/login.service';
import { environment } from '../../../Eviroments/enviroments';

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

  //logica para insertar usuario y realizar validacion de login
  showRegisterModal = false;
  _isSesion = false;
  _loginUser = new Usuario();
  _loginCreate = new Usuario();
  constructor(private commonService: CommonService, private loginService: LoginService) { }


  async onLogin() {
    const validate = await this.commonService.validarCampos(this._loginUser);
    if (validate) {
      this.loginService.login(this._loginUser).subscribe(
        (response: any) => {
          var strTitulo = 'Error';
          if (response.status == 200) {
            strTitulo = 'Correcto';
            this._loginUser.id = response.data[0].id;
            environment.hsJwt = response.data[0].password;
            this._loginUser.password = '';
            this._isSesion = true;
          }
          this.commonService.mostrarAlert(strTitulo, response.mensaje);
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
    const validate = await this.commonService.validarCampos(this._loginCreate);
    if (validate) {
      this.loginService.registrarUsuario(this._loginCreate).subscribe(
        (response: any) => {
          
          var strTitulo = response.status == 200 ? 'Correcto' : 'Error';
          this.commonService.mostrarAlert(strTitulo, response.mensaje);
          this._loginCreate = new Usuario();
        }

      )
    }
  }

  onCloseSesion() {
    location.reload();
  }

}
