import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
  export class DashboardService {

    constructor(private http: HttpClient) { }

    getTotalSalesByRegion(orgId?: number, year?: string): Observable<Object> {
      let params = new HttpParams();
      if (orgId) {
        params = params.append('orgId', orgId);
      }
      if (year) {
        params = params.append('year', year)
      }
      return this.http.get<Object>(`${environment.apiBaseUrl}/api/sales/data2?addAuth=true`, {params});
    }

    getTotalSalesByOrgAndYear(orgId?: number, year?: string): Observable<Object> {
      let params = new HttpParams();
      if (orgId) {
        params = params.append('orgId', orgId);
      }
      if (year) {
        params = params.append('year', year)
      }
      return this.http.get<Object>(`${environment.apiBaseUrl}/api/sales/data1?addAuth=true`, {params});
    }

    getTotalSalesByStateAndCity(orgId?: number, year?: string): Observable<Object> {
      let params = new HttpParams();
      if (orgId) {
        params = params.append('orgId', orgId);
      }
      if (year) {
        params = params.append('year', year)
      }
      return this.http.get<Object>(`${environment.apiBaseUrl}/api/sales/data3?addAuth=true`, {params});
    }

    getTotalSalesByRegionAndCategory(orgId?: number, year?: string): Observable<Object> {
      let params = new HttpParams();
      if (orgId) {
        params = params.append('orgId', orgId);
      }
      if (year) {
        params = params.append('year', year)
      }
      return this.http.get<Object>(`${environment.apiBaseUrl}/api/sales/data4?addAuth=true`, {params});
    }
  }
