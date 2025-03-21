import {
    EtablissementDTOPagination,
    EtablissementFormDto,
    FrenchDTO,
} from './french.type';
import { EuropeDTO, EuropeDTOPagination, EuropeFormDto } from './europe.type';

export type ClientDTO = FrenchDTO | EuropeDTO;

export type ClientDTOPagination = | EtablissementDTOPagination | EuropeDTOPagination;

export type ClientFormDto = EtablissementFormDto | EuropeFormDto;
