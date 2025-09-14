import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/Eviroments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class UsuariosService {

  private api: string = environment.apiUrl;
  constructor(private http: HttpClient) {
  }
  
  GetUsuarios(): Observable<any> {
      return this.http.get<any>(`${this.api}/Usuario/GetUsuarios`);
  }
}
