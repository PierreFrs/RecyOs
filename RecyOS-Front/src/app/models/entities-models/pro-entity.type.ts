import {
    SupplierDTO,
    SupplierDTOPagination,
    SupplierFormDto,
} from './supplier.type';
import { ClientDTO, ClientDTOPagination, ClientFormDto } from './client.type';

export type ProfessionalDto = ClientDTO | SupplierDTO;

export type ProfessionalEntityDTOPagination = ClientDTOPagination | SupplierDTOPagination;

export type ProfessionalEntityFormDto = ClientFormDto | SupplierFormDto;
