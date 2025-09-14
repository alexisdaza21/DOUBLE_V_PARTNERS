import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Deuda } from 'src/app/clases/deuda';
import { environment } from 'src/Eviroments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class DeudasService {

  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createDeudas(deuda: Deuda): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Deudas/CreateDeudas`, deuda);
  }

  getDeudas(tipo: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Deudas/GetDeudas?tipo=${tipo}`);
  }
  pagarDeudas(idDeuda: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Deudas/PagarDeuda?idDeuda=${idDeuda}`, {});
  }

  eliminarDeuda(idDeuda: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Deudas/EliminarDeuda?idDeuda=${idDeuda}`, {});
  }

  createAbono(idDeuda: number, monto: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Deudas/createAbono?idDeuda=${idDeuda}&monto=${monto}`, {});
  }

  getAbonos(idDeuda: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Deudas/GetAbonos?idDeuda=${idDeuda}`);
  }

  editDeuda(idDeuda: number, monto: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Deudas/EditDeuda?idDeuda=${idDeuda}&monto=${monto}`, {});
  }
}
