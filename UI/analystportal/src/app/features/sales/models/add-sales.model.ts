export interface AddSales {
    id: string;
    orderDate: Date;
    categoryId: number;
    productName: string;
    total: number;
    city: string;
    state: string;
    region: string;
    organizations: number[];
}