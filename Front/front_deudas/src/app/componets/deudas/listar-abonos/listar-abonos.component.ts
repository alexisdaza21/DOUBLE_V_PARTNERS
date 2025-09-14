import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DeudasService } from 'src/app/services/deudas/deudas.service';

@Component({
  selector: 'app-listar-abonos',
  templateUrl: './listar-abonos.component.html',
  styleUrls: ['./listar-abonos.component.css']
})
export class ListarAbonosComponent implements OnChanges {
  
  @Input() _idDeuda: number = 0;
  _abonos: any[] = [];

  constructor(private deudasService: DeudasService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['_idDeuda'] && this._idDeuda > 0) {
      this.getAbonos();
    }
  }

  getAbonos() {
    this.deudasService.getAbonos(this._idDeuda).subscribe(
      (response: any) => {
        if (response.status == 200) {
          this._abonos = response.data;
        }
      }
    );
  }

  getTotalAbonos(): number {
    return this._abonos.reduce((sum, abono) => sum + abono.monto, 0);
  }
  

}
