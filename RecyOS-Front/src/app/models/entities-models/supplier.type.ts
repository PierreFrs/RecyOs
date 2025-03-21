import {
    EuropeDTO,
    EuropeDTOPagination,
    FournisseurEuropeFormDto,
} from './europe.type';
import {
    EtablissementDTOPagination,
    FournisseurFranceFormDto,
    FrenchDTO,
} from './french.type';

export type SupplierDTO = FrenchDTO | EuropeDTO;

export type SupplierDTOPagination =
    | EtablissementDTOPagination
    | EuropeDTOPagination;

export type SupplierFormDto =
    | FournisseurFranceFormDto
    | FournisseurEuropeFormDto;
