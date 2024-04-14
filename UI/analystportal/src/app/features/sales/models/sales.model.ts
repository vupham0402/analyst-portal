import { Organization } from "../../organization/models/organization.model";

export interface Sales {
    id: string;
    orderDate: Date;
    categoryId: number;
    categoryName: string;
    productName: string;
    total: number;
    city: string;
    state: string;
    region: string;
    organizations: Organization[];
    count: number;
}