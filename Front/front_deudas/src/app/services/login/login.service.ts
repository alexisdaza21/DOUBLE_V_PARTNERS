import { Injectable } from '@angular/core';
import { environment } from '../../../Eviroments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = environment.apiUrl;

  
  constructor() { }
}
