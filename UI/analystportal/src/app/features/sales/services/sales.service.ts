import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Sales } from '../models/sales.model';
import { Observable } from 'rxjs';
import { AddSales } from '../models/add-sales.model';
import { environment } from 'src/environments/environment';
import { EditSales } from '../models/edit-sales.model';

@Injectable({
  providedIn: 'root'
})
export class SalesService {

  constructor(private http: HttpClient) { }

  createSales (data: AddSales) : Observable<Sales> {
    return this.http.post<Sales>(`${environment.apiBaseUrl}/api/sales?addAuth=true`, data);
  }

  getAllSales(queryOrg?: number, queryCat?: number, queryPro?: string, 
              sortBy?: string, sortDirection?: string,
              pageNumber?: number, pageSize?: number): Observable<Sales[]> {

    let params = new HttpParams();

    if (queryOrg) {
      params = params.set('queryOrg', queryOrg);
    }

    if (queryCat) {
      params = params.set('queryCat', queryCat);
    }
    
    if (queryPro) {
      params = params.set('queryPro', queryPro);
    }

    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }

    if (sortDirection) {
      params = params.set('sortDirection', sortDirection);
    }

    if (pageNumber) {
      params = params.set('pageNumber', pageNumber);
    }

    if (pageSize) {
      params = params.set('pageSize', pageSize);
    }

    return this.http.get<Sales[]>(`${environment.apiBaseUrl}/api/sales?addAuth=true`, {
      params: params
    });
  }

  getSalesById(id: string): Observable<Sales> {
    return this.http.get<Sales>(`${environment.apiBaseUrl}/api/sales/${id}?addAuth=true`);
  }

  getSalesCount(): Observable<number> {
    return this.http.get<number>(`${environment.apiBaseUrl}/api/sales/count`);
  }

  updateSales(id: string, editSales: EditSales): Observable<Sales> {
    return this.http.put<Sales>(`${environment.apiBaseUrl}/api/sales/${id}?addAuth=true`, editSales);
  }

  deleteSales(id: string): Observable<Sales> {
    return this.http.delete<Sales>(`${environment.apiBaseUrl}/api/sales/${id}?addAuth=true`);
  }
}
