import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Organization } from '../../models/organization.model';
import { ActivatedRoute, Router } from '@angular/router';
import { OrganizationService } from '../../services/organization.service';
import { LogoService } from 'src/app/shared/services/logo.service';
import { EditOrganization } from '../../models/edit-organization.model';

@Component({
  selector: 'app-edit-organization',
  templateUrl: './edit-organization.component.html',
  styleUrls: ['./edit-organization.component.css']
})
export class EditOrganizationComponent implements OnInit, OnDestroy {
  id: number | null = null;
  model?: Organization;
  isImageSelectorVisible: boolean = false;

  routerSubscription?: Subscription;
  editOrganizationSubscription?: Subscription;
  getOrganizationSubscription?: Subscription;
  deleteOrganizationSubscription?: Subscription;
  imageSelectSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
              private organizationService: OrganizationService,
              private router: Router,
              private logoService: LogoService) {

  }

  onFormSubmit(): void {
    // Convert this model to request object
    if (this.model && this.id) {
      var editOrganization: EditOrganization = {
        name: this.model.name,
        logoUrl: this.model.logoUrl
      };

      this.editOrganizationSubscription = this.organizationService.updateOrganization(this.id, editOrganization)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/organizations');
          }
        });
    }
  }

  ngOnInit(): void {
    this.routerSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        const paramId = params.get('id');
        this.id = paramId !== null ? parseInt(paramId, 10) : null;

        // Get Organization API
        if (this.id) {
          this.getOrganizationSubscription = this.organizationService.getOrganizationById(this.id)
            .subscribe({
              next: (response) => {
                this.model = response;
              }
          });
        }

        this.imageSelectSubscription = this.logoService.onSelectImage()
          .subscribe({
            next: (response) => {
             if (this.model) {
              this.model.logoUrl = response.url;
              this.isImageSelectorVisible = false;
             } 
            }
          })
      }
    })    
  }

  onDelete(): void {
    if (this.id) {
      // call service and delete blogpost
      this.deleteOrganizationSubscription = this.organizationService.deleteOrganization(this.id)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/organizations');
          }
        });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
    this.editOrganizationSubscription?.unsubscribe();
    this.deleteOrganizationSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
    this.getOrganizationSubscription?.unsubscribe();
  }
}
