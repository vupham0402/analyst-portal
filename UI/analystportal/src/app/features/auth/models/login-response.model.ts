export interface LoginResponse {
    token: string;
    firstName: string;
    lastName: string;
    email: string;
    organizationName: string;
    roles: string[];
}