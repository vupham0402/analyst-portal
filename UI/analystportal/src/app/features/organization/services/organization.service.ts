import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddOrganization } from '../models/add-organization.model';
import { Observable, catchError, map, of } from 'rxjs';
import { Organization } from '../models/organization.model';
import { environment } from 'src/environments/environment.development';
import { EditOrganization } from '../models/edit-organization.model';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {

  constructor(private http: HttpClient) { }

  createOrganization (data: AddOrganization) : Observable<Organization> {
    return this.http.post<Organization>(`${environment.apiBaseUrl}/api/organizations?addAuth=true`, data);
  }

  getAllOrganizations(): Observable<Organization[]> {
    return this.http.get<Organization[]>(`${environment.apiBaseUrl}/api/organizations`);
  }

  getOrganizationById(id: number): Observable<Organization> {
    return this.http.get<Organization>(`${environment.apiBaseUrl}/api/organizations/${id}`);
  }

  getOrganizationByName(name: string): Observable<Organization> {
    return this.http.get<Organization>(`${environment.apiBaseUrl}/api/organizations/${name}`);
  }

  updateOrganization(id: number, editOrganization: EditOrganization): Observable<Organization> {
    return this.http.put<Organization>(`${environment.apiBaseUrl}/api/organizations/${id}?addAuth=true`, editOrganization);
  }

  deleteOrganization(id: number): Observable<Organization> {
    return this.http.delete<Organization>(`${environment.apiBaseUrl}/api/organizations/${id}?addAuth=true`);
  }
}
