import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Router, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private cookieService: CookieService,
              private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const state = this.router.routerState.snapshot;
    if (this.shouldInterceptRequest(request, state)) {
      const authRequest = request.clone({
        setHeaders: {
          'Authorization': this.cookieService.get('Authorization')
        }
      });
      return next.handle(authRequest);   
    }
    return next.handle(request);
  }

  private shouldInterceptRequest(request: HttpRequest<any>, state: RouterStateSnapshot): boolean {
    if (state.url.includes('/login?returnUrl=%2F')) {
      return true;
    }
    else if (request.urlWithParams.indexOf('addAuth=true', 0) > -1) {
      return true;
    }
    else return false;
  }
}
