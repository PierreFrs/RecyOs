import { Commercial } from 'app/models/commercial.type';

export interface RoleDto {
    id: number;
    name: string;
}
export interface UserDto {
    id: number;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    avatar: string;
    status: string;
    societeId: number;
    roles: RoleDto[];
}

export interface UserDtoPaginator {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}

export interface SignUpDto {
    email?: string; // nullable: true
    password?: string; // nullable: true
    firstName?: string; // nullable: true
    lastName?: string; // nullable: true
    userName?: string; // nullable: true
    confirmPassword?: string; // nullable: true
}
