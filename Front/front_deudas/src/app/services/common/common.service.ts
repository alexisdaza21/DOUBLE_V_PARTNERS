import { Injectable } from '@angular/core';
import { environment } from '../../../Eviroments/enviroments';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { validateSync } from 'class-validator';
import { Usuario } from 'src/app/clases/usuario';

@Injectable({
    providedIn: 'root'
})
export class CommonService {

    private api: string = environment.apiUrl;
    constructor(private http: HttpClient) {
    }

    validateModel(datos: any) {
        return validateSync(datos);
    }

    mostrarAlert(header: string, message: string) {
        alert(`${header}\n\n${message}`);
    }

    getTokenAnonimo(): Observable<any> {

        return this.http.post<any>(`${this.api}/Usuario/GetTokenAnonimo`, {});
    }

    async validarCampos(datos: Usuario) {

        const result = this.validateModel(datos)
        debugger
        if (result.length > 0) {
            let strErrores = '';
            for (const error of result) {
                for (const constraint in error.constraints) {
                    if (Object.prototype.hasOwnProperty.call(error.constraints, constraint)) {
                        strErrores += `- ${error.constraints[constraint]} \n `;
                    }
                }
            }
            this.mostrarAlert('Error', strErrores);

            return false;
        } else {
            return true;
        }

    }
}
