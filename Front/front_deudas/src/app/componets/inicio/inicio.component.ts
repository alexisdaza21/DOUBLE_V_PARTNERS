import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Usuario } from '../../clases/usuario';
import { CommonService } from 'src/app/services/common/common.service';
import { UsuariosService } from 'src/app/services/usuarios/usuarios.service';
import { DeudasService } from 'src/app/services/deudas/deudas.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.css']
})
export class InicioComponent {
  @Input() _usuario!: Usuario;
  @Output() closeSesionEvent = new EventEmitter<void>();
  activeTab: 'pendientes' | 'pagadas' = 'pendientes';
  showModalRegisterDeuda = false;
  showModalAbonarDeuda = false;
  showModalAbonos = false;
  showbackdrop = false;
  _deudas: any[] = [];
  _abono = {
    idDeuda: 0,
    monto: 0,
    descripcion: ''
  }
  constructor(
    private commonService: CommonService,
    private usuariosService: UsuariosService,
    private deudasService: DeudasService
  ) {
    this.getDeudas();
  }
  closeSesion() {
    this.closeSesionEvent.emit();
  }

  openModalRegisterDeuda() {
    this.showbackdrop = true;
    this.showModalRegisterDeuda = true;
  }

  closeModalRegisterDeuda() {
    this.showbackdrop = false;
    this.showModalRegisterDeuda = false;
  }

  getDeudas() {

    var tipo = this.activeTab == 'pendientes' ? 1 : 2;
    this.deudasService.getDeudas(tipo).subscribe(
      (response: any) => {
        if (response.status == 200) {
          this._deudas = response.data;
        }
      }
    )
  }

  pagarDeudas(idDeuda: number) {

    this.deudasService.pagarDeudas(idDeuda).subscribe(
      (response: any) => {
        if (response.status == 200) {
          this.getDeudas();
        }
      }
    )
  }

  eliminarDeuda(idDeuda: number) {

    this.deudasService.eliminarDeuda(idDeuda).subscribe(
      (response: any) => {
        if (response.status == 200) {
          this.getDeudas();
        }
      }
    )
  }

  openModalAbonarDeuda(idDeuda: number) {
    this._abono.idDeuda = idDeuda;
    this.showbackdrop = true;
    this.showModalAbonarDeuda = true;
  }

  closeModalAbonarDeuda() {
    this._abono.idDeuda = 0;
    this._abono.monto = 0;
    this.showbackdrop = false;
    this.showModalAbonarDeuda = false;
  }

  createAbono() {
    if (this._abono.monto > 0) {
      this.deudasService.createAbono(this._abono.idDeuda, this._abono.monto).subscribe(
        (response: any) => {
          if (response.status == 200) {
            this.commonService.mostrarAlert('Correcto', response.mensaje);
            this.closeModalAbonarDeuda();
            this.getDeudas();
          }
        }
      )
    } else {

      this.commonService.mostrarAlert('Error', 'El monto deber ser mayor a 0');
    }
  }

  openModalAbonos(deuda: any) {
    debugger
    this._abono.idDeuda = deuda.idDeuda;
    this._abono.descripcion = deuda.descripcion;
    this.showbackdrop = true;
    this.showModalAbonos = true;
  }

  closeModalAbonos() {
    this._abono.idDeuda = 0;
    this._abono.descripcion = '';
    this.showbackdrop = false;
    this.showModalAbonos = false;
  }





}