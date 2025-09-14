import { Component, EventEmitter, Output } from '@angular/core';
import { Deuda } from 'src/app/clases/deuda';
import { Usuario } from 'src/app/clases/usuario';
import { CommonService } from 'src/app/services/common/common.service';
import { DeudasService } from 'src/app/services/deudas/deudas.service';
import { UsuariosService } from 'src/app/services/usuarios/usuarios.service';

@Component({
  selector: 'app-crear',
  templateUrl: './crear.component.html',
  styleUrls: ['./crear.component.css']
})
export class CrearComponent {

  //Logica paragenerar inserccion de deudas, no se permite seleccionar el mismo amigo en los campos 
  //de los usuarios
  
  _deuda = new Deuda();

  @Output() getDeudasEvent = new EventEmitter<void>();
  _usuarios: Usuario[] = [];

  constructor(
    private commonService: CommonService,
    private usuariosService: UsuariosService,
    private deudasService: DeudasService
  ) {
    this._deuda.id = 0;
    this._deuda.idUsuarioPresta = 0;
    this._deuda.idUsuarioDebe = 0; // caso inválido, deberían ser diferentes
    this._deuda.descripcion = "";
    this._deuda.monto = 0;
    this.getUsuarios()
  }


  async getUsuarios() {

    this.usuariosService.GetUsuarios().subscribe(
      (response: any) => {
        if (response.status == 200) {
          this._usuarios = response.data;
        }
      }
    )
  }

  async crearDeuda() {
    const validate = await this.commonService.validarCampos(this._deuda);
    if (validate) {
      this.deudasService.createDeudas(this._deuda).subscribe(
        (response: any) => {          
          var strTitulo = response.status == 200 ? 'Correcto' : 'Error';
          this.commonService.mostrarAlert(strTitulo, response.mensaje);
          this._deuda = new Deuda();
          this.getDeudas();
        }

      )

    }
  }
  getDeudas() {
    this.getDeudasEvent.emit();
  }
}
