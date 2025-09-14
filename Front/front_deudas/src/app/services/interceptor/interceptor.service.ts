
import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, finalize } from 'rxjs';
import { environment } from '../../../Eviroments/enviroments';
import { CommonService } from '../common/common.service';
@Injectable()
export class InterceptorService implements HttpInterceptor {
  constructor(private commonService: CommonService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = environment.hsJwt;

    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'My-Custom-Header': 'foobar'
      }
    });

    this.commonService.showLoading();

    return next.handle(cloned).pipe(
      finalize(() => this.commonService.hideLoading())
    );
  }
}