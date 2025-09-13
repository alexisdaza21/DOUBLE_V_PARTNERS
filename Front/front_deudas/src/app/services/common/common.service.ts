import { Injectable } from '@angular/core';
import { environment } from '../../../Eviroments/enviroments';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { validateSync } from 'class-validator';

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


}
