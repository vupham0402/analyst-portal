import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import { Organization } from '../../organization/models/organization.model';
import { OrganizationService } from '../../organization/services/organization.service';
import { ListOrganizationComponent } from '../../organization/list-organization/list-organization/list-organization.component';
import { RegisterRequest } from '../models/register-request.model';
import { RegisterResponse } from '../models/register-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  $user = new BehaviorSubject<User | undefined>(undefined);

  constructor(private http: HttpClient,
              private cookieService: CookieService,
              private organizationService: OrganizationService) { }

  register(request: RegisterRequest, organizationId: number): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${environment.apiBaseUrl}/api/auth/register`, {
      firstName: request.firstName,
      lastName: request.lastName,
      organizationId: organizationId,
      email: request.email,
      password: request.password
    });  
  }            

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiBaseUrl}/api/auth/login`, {
      email: request.email,
      password: request.password
    });  
  }

  setUser(user: User): void {
    this.$user.next(user);
    localStorage.setItem('organization', user.organizationName);
    localStorage.setItem('first-name', user.firstName);
    localStorage.setItem('last-name', user.lastName);
    localStorage.setItem('user-email', user.email);
    localStorage.setItem('user-roles', user.roles.join(','));
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  getUser(): User | undefined {
    const firstName = localStorage.getItem('first-name');
    const lastName = localStorage.getItem('last-name');
    const email = localStorage.getItem('user-email');
    const organization = localStorage.getItem('organization');
    const roles = localStorage.getItem('user-roles');
    if (email && roles && firstName && lastName && organization) {
      const user: User = {
        firstName: firstName,
        lastName: lastName,
        email: email,
        organizationName: organization,
        roles: roles.split(',')
      };

      return user;
    }

    return undefined;
  }

  logout(): void {
    localStorage.clear();
    this.cookieService.delete('Authorization', '/');
    this.$user.next(undefined);
  }
}
