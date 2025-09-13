import { Injectable } from '@angular/core';
import { environment } from '../../../Eviroments/enviroments';
import { Usuario } from 'src/app/clases/usuario';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = environment.apiUrl;


  constructor(private http: HttpClient) { }

  registrarUsuario(usuarioRegistro: Usuario): Observable<any> {
    const headers =
    {
      'Authorization': `Bearer ${localStorage.getItem('hsJwt')}`,
      'My-Custom-Header': 'foobar'
    };
    let options = { headers };
    return this.http.post<any>(`${this.apiUrl}/Usuario/CreateUsuarios`, usuarioRegistro, options);
  }
}
