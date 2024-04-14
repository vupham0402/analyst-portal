# Analyst Portal

## Description
This project provides a web application where members can log in to view personalized data visualizations. Admin users have the capability to view visualizations for all members and modify the data as needed. 

## Features
- **Member Login**: Members can log in to view personalized visualizations of their data.
- **Admin Panel**: Admins can log in to view all members' data, perform CRUD operations, and manage user accounts.
- **Data Visualization**: Uses ngx-charts to render interactive charts and graphs based on the member's data.
- **Secure Authentication**: Implements JWT tokens to handle secure authentication and session management.
- **Azure Blob Storage Integration**: Integrates Azure blob storage for storing and retrieving member-specific logos efficiently.
- **Database Integration**: Utilizes Azure SQL Database for structured data storage and queries.

## API Installing
- Clone the repository
- Navigate to the backend directory, restore dependencies: dotnet restore
- Set up Jwt key, issuer, and audience
- Add connection strings from Azure SQL Database and Azure Blob Storage
- Create and migrate data for Azure SQL Database (please use 2 csv files from /Data to add data for Sales and OrganizationSales tables)
- Start the backen server: dotnet run

## UI Installing
- Navigate to the frontend directory, install dependencies: npm install
- Run fronent server: ng serve --open

# CI/CD Automation
- Terraform: Used to provision Azure Container Registry (ACR) and Azure Kubernetes Service (AKS) cluster.
- Docker: Builds and containerizes the source code, storing the Docker images in ACR.
- Azure Kubernetes Service (AKS): Pulls the Docker images from ACR and deploys them to the server with a configured load balancer.
- GitLab CI: Manages the automation of build, test, and deployment pipelines, ensuring that every commit triggers the appropriate workflows.

## Running
- After creating ACR and AKS Cluster, push the source code to gitlab to run CI/CD automation
- Please make sure to create needed variables for each gitlab yaml file

# Built With
- .NET 8 - The web framework used for the backend
- Angular 17 - The web framework used for the frontend
- Docker - Containerization
- Terraform - Infrastructure as code
- GitLab CI - Continuous Integration/Continuous Deployment
- Azure Blob Storage - Blob storage service
- Azure SQL Database - Managed relational SQL Database service



