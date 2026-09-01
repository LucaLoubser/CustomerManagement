import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<unknown>, next: HttpHandler)
    : Observable<HttpEvent<unknown>> {
    var cloned = req.clone({
      setHeaders: { 'X-API-Key': environment.apiKey }
    });

    return next.handle(cloned);
  }
}