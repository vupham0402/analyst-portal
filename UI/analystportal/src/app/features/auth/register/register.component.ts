import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ListOrganizationComponent } from 'src/app/features/organization/list-organization/list-organization/list-organization.component';
import { Organization } from 'src/app/features/organization/models/organization.model';
import { OrganizationService } from 'src/app/features/organization/services/organization.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { RegisterRequest } from '../models/register-request.model';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  organizations$?: Observable<Organization[]>;
  model: RegisterRequest;
  selectedOrganizationId: number = -1;

  constructor(private organizationService: OrganizationService,
              private authService: AuthService,
              private router: Router) {
    this.model = {
      firstName: '',
      lastName: '',
      organizationId: this.selectedOrganizationId,
      email: '',
      password: '',
    };    
  }

  ngOnInit(): void {
    this.organizations$ = this.organizationService.getAllOrganizations();
  }

  onFormSubmit(): void {
    this.authService.register(this.model, this.selectedOrganizationId)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/');  
        }
      })
  }
}
