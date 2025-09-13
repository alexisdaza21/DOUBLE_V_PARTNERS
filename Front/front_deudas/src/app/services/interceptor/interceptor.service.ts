
import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../Eviroments/enviroments';

@Injectable()
export class InterceptorService implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = environment.hsJwt; 
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'My-Custom-Header': 'foobar'
      }
    });

    return next.handle(cloned);
  }
}
