import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrganizationLogo } from '../models/organization-logo.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class LogoService {
  selectedImage: BehaviorSubject<OrganizationLogo> = new BehaviorSubject<OrganizationLogo>({
    fileName: '',
    title: '',
    url: ''
  });

  constructor(private http: HttpClient) { }

  uploadImage(file: File, fileName: string, title: string): Observable<OrganizationLogo> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);

    return this.http.post<OrganizationLogo>(`${environment.apiBaseUrl}/api/images`, formData);
  }

  getAllImages(): Observable<OrganizationLogo[]> {
    return this.http.get<OrganizationLogo[]>(`${environment.apiBaseUrl}/api/images`);
  }

  selectImage(image: OrganizationLogo): void {
    this.selectedImage.next(image);
  }

  onSelectImage(): Observable<OrganizationLogo> {
    return this.selectedImage.asObservable();
  }
}
