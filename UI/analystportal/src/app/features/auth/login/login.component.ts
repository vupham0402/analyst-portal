import { Component } from '@angular/core';
import { LoginRequest } from '../models/login-request.model';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  model: LoginRequest;

  constructor(private authService: AuthService,
              private cookieService: CookieService,
              private router: Router) {
    this.model = {
      email: 'admin@analystportal.com',
      password: 'Admin@123'
    };    
  }

  onFormSubmit(): void {
      this.authService.login(this.model)
        .subscribe({
          next: (response) => {
            // Set Auth Cookie
            // this.cookieService.set('Authorization', `Bearer ${response.token}`,
            // undefined, '/', undefined, true, 'Strict');
            this.cookieService.set('Authorization', `Bearer ${response.token}`,
            undefined, '/', undefined, false, 'Strict');
            // Set User
            this.authService.setUser({
              firstName: response.firstName,
              lastName: response.lastName,
              email: response.email,
              roles: response.roles,
              organizationName: response.organizationName
            });

            // Redirect back to Home
            this.router.navigateByUrl('/');
          }
        });
  }
}
