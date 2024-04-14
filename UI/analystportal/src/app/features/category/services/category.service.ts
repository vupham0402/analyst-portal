import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddCategory } from '../models/add-category.model';
import { Category } from '../models/category.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EditCategory } from '../models/edit-category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  createCategory (data: AddCategory) : Observable<Category> {
    return this.http.post<Category>(`${environment.apiBaseUrl}/api/categories?addAuth=true`, data);
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/categories?addAuth=true`);
  }

  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`);
  }

  updateCategory(id: number, editCategory: EditCategory): Observable<Category> {
    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`, editCategory);
  }

  deleteCategory(id: number): Observable<Category> {
    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`);
  }
}
