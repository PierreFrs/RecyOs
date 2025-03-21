import { EuropeDTO } from "app/models/entities-models/europe.type";
import { BalancePaginator } from "../balances-france/balances-france.types";
import { SocieteDto } from "app/models/societe.type";

export interface BalanceEurope {
    id: number;
    clientId: number;
    societeId: number;
    dateRecuperationBalance: string;
    montant: number;
    createDate: string;
    updatedAt: string | null;
    createdBy: string | null;
    updatedBy: string | null;
    isDeleted: boolean;
    clientEurope: EuropeDTO;
    societe: SocieteDto;
}

export interface BalanceEuropeResponse {
    items: BalanceEurope[];
}

export interface BalanceEuropeGridResponse {
    paginator: BalancePaginator;
    items: BalanceEurope[];
    sommeTotal: number;
}

export interface BalanceGridParams {
    FilterClientName?: string;
    FilterBySocieteId?: string;
    FilterByClientCommercialId?: string;
    PageNumber?: number;
    PageSize?: number;
    SortBy?: string;
    OrderBy?: string;
}

