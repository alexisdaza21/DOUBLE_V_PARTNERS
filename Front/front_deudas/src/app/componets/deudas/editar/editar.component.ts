import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { CommonService } from 'src/app/services/common/common.service';
import { DeudasService } from 'src/app/services/deudas/deudas.service';

@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrls: ['./editar.component.css']
})
export class EditarComponent  {
  
  @Input() _deuda: any = {};
  @Output() getDeudasEvent = new EventEmitter<void>();
  constructor(private deudasService: DeudasService, private commonService: CommonService,) {}


  editDeuda() {
    if (this._deuda.monto) {
      
      this.deudasService.editDeuda(this._deuda.idDeuda, this._deuda.monto).subscribe(
        (response: any) => {
        if (response.status == 200) {
          this.commonService.mostrarAlert('Correcto', response.mensaje);
          this.getDeudas();
        }
      }
      );
    } else {
      this.commonService.mostrarAlert('Error', 'El monto deber ser mayor a 0');
    }
  }

  getDeudas() {
    this.getDeudasEvent.emit();
  }
}
