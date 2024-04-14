import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Organization } from '../../organization/models/organization.model';
import { BehaviorSubject, Observable, Subscription, map, of, tap } from 'rxjs';
import { OrganizationService } from '../../organization/services/organization.service';
import { Router } from '@angular/router';
import { DashboardService } from '../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  console = console;
  organizations$?: Observable<Organization[]>;
  years$?: Observable<Array<string>>;
  pieChartData$?: Observable<any>;
  lineChartData$?: Observable<any>;
  treeChartData$?: Observable<any>;
  barChartData$?: Observable<any>;
  selectedYear: string = '2014';
  isStateView: boolean = true;
  selectedState: string | null = null;
  organizationName: string = '';
  selectedOrganizationId: number = 0;
  flag: boolean = false;
  logoUrl: string | null = null;

  selectedOrganizationId$: BehaviorSubject<number|null> = new BehaviorSubject<number|null>(null);
  
  constructor(private organizationService: OrganizationService,
              private dashboardService: DashboardService) {

  }

  ngOnInit(): void {  
    this.organizationName = localStorage.getItem('organization') || '';
    if (this.organizationName !== 'EyeProGPO') {
      this.organizationService.getOrganizationByName(this.organizationName)
        .subscribe({
          next: (response) => {
            this.selectedOrganizationId$.next(response.id);
            this.logoUrl = response.logoUrl;
          }
      });
      this.selectedOrganizationId$.subscribe(id => {
        this.selectedOrganizationId = id ? id : 0; 
        this.onOrganizationChange();
      });
    } 
    else {
      this.organizations$ = this.organizationService.getAllOrganizations();
      this.selectedOrganizationId = 2;
      this.organizationService.getOrganizationById(this.selectedOrganizationId)
        .subscribe({
          next: (response) => {
            this.logoUrl = response.logoUrl;
          }
        });
      this.onOrganizationChange();
    }   
  }

  onOrganizationChange(): void {
    if (this.selectedOrganizationId > 0) {
      this.organizationService.getOrganizationById(this.selectedOrganizationId)
        .subscribe({
          next: (response) => {
            this.logoUrl = response.logoUrl;
          }
        });
      //line chart
      this.dashboardService.getTotalSalesByOrgAndYear(this.selectedOrganizationId)
        .subscribe({
          next: (data) => {
            if (data) {
              this.lineChartData$ = of(this.transformDataForLineChart(data));
              this.years$ = of(Object.keys(data));
              this.flag = true;
            }
          }
      });

      //bar chart
      this.dashboardService.getTotalSalesByRegionAndCategory(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            if (parseInt(this.selectedYear) > 0) {
              this.barChartData$ = of(this.transformDataForBarChart(data));
            }
            else {
              this.barChartData$ = undefined;
            }
          }
      });

      //pie chart
      this.dashboardService.getTotalSalesByRegion(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            if (parseInt(this.selectedYear) > 0) {
              this.pieChartData$ = of(this.transformDataForPieChart(data));
            }
            else {
              this.pieChartData$ = undefined;
            }
          }
      });

      //heat map
      this.dashboardService.getTotalSalesByStateAndCity(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            if (parseInt(this.selectedYear) > 0) {
              this.treeChartData$ = of(this.transformDataForStateTreeMap(data));
            }
            else {
              this.treeChartData$ = undefined;
            }
          }
      });
      
    } else {
      this.lineChartData$ = of(null); // Reset chart data if no organization is selected
    }
  }

  onYearChange(): void {
    if (this. selectedOrganizationId > 0 && this.selectedYear && parseInt(this.selectedYear) > 0) {
      this.dashboardService.getTotalSalesByOrgAndYear(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            this.lineChartData$ = of(this.transformDataForLineChart(data));
          }
      });

      //bar chart
      this.dashboardService.getTotalSalesByRegionAndCategory(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            if (parseInt(this.selectedYear) > 0) {
              this.barChartData$ = of(this.transformDataForBarChart(data));
            }
            else {
              this.barChartData$ = undefined;
            }
          }
      });

      //pie chart
      this.dashboardService.getTotalSalesByRegion(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            this.pieChartData$ = of(this.transformDataForPieChart(data));
          }
      });

      //tree map
      this.dashboardService.getTotalSalesByStateAndCity(this.selectedOrganizationId, this.selectedYear)
        .subscribe({
          next: (data) => {
            this.treeChartData$ = of(this.transformDataForStateTreeMap(data));
          }
      });
    } 
    else if (parseInt(this.selectedYear) === 0) {
      this.onOrganizationChange();
    }
    else {
      this.lineChartData$ = of(null); // Reset chart data if no organization is selected
      this.pieChartData$ = of(null);
      this.treeChartData$ = of(null);
      this.barChartData$ = of(null);
    }
  }

  private transformDataForLineChart(data: any): any[] {
    // Initialize an empty array for the chart series
    let chartSeries = [];

    if (parseInt(this.selectedYear) > 0)
    {
      const yearlyData = data[this.selectedYear];
      for (const [month, totalSales] of Object.entries(yearlyData)) {
        if (month !== 'Total') {
          chartSeries.push({
            name: month,
            value: totalSales
          });
        } 
      }

      return [{
        name: 'Total Sales',
        series: chartSeries
      }];
    }
    else {
      // Loop through each year in the data
      for (const [year, details] of Object.entries(data)) {

        // For each year, push an object containing the year and the total sales for that year
        const salesDetails = details as any;
        chartSeries.push({
          name: year,
          value: salesDetails['Total']
        });
      }

      return [{
        name: 'Total Sales',
        series: chartSeries
      }];  
    }
  }

  private transformDataForBarChart(data: any): any[] {
    // Initialize an empty array for the chart series
    let chartSeries = [];

    if (parseInt(this.selectedYear) > 0)
    {
      for (const [region, details] of Object.entries(data)) {
        const cateData = details as any;
        let series = [];
        for (const [category, value] of Object.entries(cateData)) {
          let object = {
            name: category,
            value: value
          };
          series.push(object);
        }
        let regionObject = {
          name: region,
          series: series
        }
        chartSeries.push(regionObject);
      }
      //console.log(chartSeries);
    }

    else {
      chartSeries = [];
    }
     
    return chartSeries;
  }

  private transformDataForPieChart(data: any): any[] {
    // Initialize an empty array for the chart series
    let chartSeries = [];

    if (parseInt(this.selectedYear) > 0)
    {
      for (const [region, totalSales] of Object.entries(data)) {
        chartSeries.push({
          name: region,
          value: totalSales
        });
      }
      return chartSeries;
    }
    else {
      return [];
    }
  }

  private transformDataForStateTreeMap(data: any): any[] {
    // Initialize an empty array for the chart series
    let chartSeries = [];

    if (parseInt(this.selectedYear) > 0)
    {
      for (const [state, details] of Object.entries(data)) {
        const cityData = details as any;
        let stateObject = {
          name: state,
          value: cityData['Total']
        };
        chartSeries.push(stateObject);
      }
      //console.log(chartSeries);
      return chartSeries;
    }
    else {
      return [];
    }
  }

  private transformDataForCityTreeMap(data: any, selectedState: string): any[] {
    // Initialize an empty array for the chart series
    let chartSeries = [];

    if (parseInt(this.selectedYear) > 0)
    {
      const stateData = data[selectedState];
      for (const [city, value] of Object.entries(stateData)) {
        if (city !== 'Total') {
          let stateObject = {
            name: city,
            value: value
          };
          chartSeries.push(stateObject);
        }
      }
      //console.log(chartSeries);
      return chartSeries;
    }
    else {
      return [];
    }
  }

  onSelect(event: any): void {
    this.selectedState = event.name;
    if (this.selectedOrganizationId > 0) {
      if (this.isStateView) {
        //tree map
        this.dashboardService.getTotalSalesByStateAndCity(this.selectedOrganizationId, this.selectedYear)
          .subscribe({
            next: (data) => {
              if (this.selectedState) {
                this.treeChartData$ = of(this.transformDataForCityTreeMap(data, this.selectedState));
              }
            }
        });
      }
      else {
        //tree map
        this.dashboardService.getTotalSalesByStateAndCity(this.selectedOrganizationId, this.selectedYear)
          .subscribe({
            next: (data) => {
              this.treeChartData$ = of(this.transformDataForStateTreeMap(data));
            }
        });
      }
      this.isStateView = !this.isStateView;
    }
  }
  
}
