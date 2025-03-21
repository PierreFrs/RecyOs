import {ClientParticulierDto, ClientParticulierDtoPagination, ClientParticulierFormDto} from "./particulier.type";
import {ProfessionalDto, ProfessionalEntityDTOPagination, ProfessionalEntityFormDto} from "./pro-entity.type";

export type EntityDto = ProfessionalDto | ClientParticulierDto;

export type EntityFormDto = ProfessionalEntityFormDto | ClientParticulierFormDto;

export type EntityDTOPagination = ProfessionalEntityDTOPagination | ClientParticulierDtoPagination;
