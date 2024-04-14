import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { OrganizationLogo } from '../../models/organization-logo.model';
import { NgForm } from '@angular/forms';
import { LogoService } from '../../services/logo.service';

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.css']
})
export class ImageSelectorComponent implements OnInit{
  private file?: File;
  fileName: string = '';
  title: string = '';
  images$?: Observable<OrganizationLogo[]>;

  @ViewChild('form', {static: false}) imageUploadForm?: NgForm;

  constructor(private logoService: LogoService) {
    
  }
  ngOnInit(): void {
    this.getImages();
  }

  onFileUploadChange(event: Event): void {
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];
  }

  uploadImage(): void {
    if (this.file && this.fileName !== '' && this.title !== '') {
      // Image service to upload the image
      this.logoService.uploadImage(this.file, this.fileName, this.title)
        .subscribe({
          next: (response) => {
            // console.log(response);
            this.imageUploadForm?.resetForm();
            this.getImages();
          }
        });
    }
  }

  selectImage(image: OrganizationLogo): void {
    this.logoService.selectImage(image);
  }

  private getImages() {
    this.images$ = this.logoService.getAllImages();
  }

}
