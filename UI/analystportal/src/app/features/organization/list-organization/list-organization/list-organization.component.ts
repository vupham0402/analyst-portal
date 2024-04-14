import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Organization } from '../../models/organization.model';
import { OrganizationService } from '../../services/organization.service';

@Component({
  selector: 'app-list-organization',
  templateUrl: './list-organization.component.html',
  styleUrls: ['./list-organization.component.css']
})
export class ListOrganizationComponent implements OnInit{

  organizations$?: Observable<Organization[]>;

  constructor(private organizationService: OrganizationService) {

  }

  ngOnInit(): void {
    // get all organizations from API
    this.organizations$ = this.organizationService.getAllOrganizations();
  }
}
