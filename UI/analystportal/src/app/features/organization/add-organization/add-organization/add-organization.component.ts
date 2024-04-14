import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddOrganization } from '../../models/add-organization.model';
import { Subscription } from 'rxjs';
import { OrganizationService } from '../../services/organization.service';
import { Router } from '@angular/router';
import { LogoService } from 'src/app/shared/services/logo.service';

@Component({
  selector: 'app-add-organization',
  templateUrl: './add-organization.component.html',
  styleUrls: ['./add-organization.component.css']
})
export class AddOrganizationComponent implements OnInit, OnDestroy {
  model: AddOrganization;
  isImageSelectorVisible: boolean = false;

  imageSelectorSubscription$?: Subscription;
  
  constructor(private organizationService: OrganizationService,
              private router: Router,
              private logoService: LogoService) {
       this.model = {
        name: '',
        logoUrl: ''
       }      
  }

  ngOnInit(): void {
    this.imageSelectorSubscription$ = this.logoService.onSelectImage()
      .subscribe({
        next: (selectedImage) => {
          this.model.logoUrl = selectedImage.url;
          this.closeImageSelector();
        }
      })
  }

  onFormSubmit(): void {
    console.log(this.model);
    this.organizationService.createOrganization(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/organizations');
        }
      });
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription$?.unsubscribe();
  }

}
