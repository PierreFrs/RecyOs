import { ClientParticulierDto } from "app/models/entities-models/particulier.type";
import { SocieteDto } from "app/models/societe.type";
import { BalancePaginator } from "../balances-france/balances-france.types";

export interface BalanceParticulier {
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
    clientParticulier: ClientParticulierDto;
    societe: SocieteDto;
}

export interface BalanceParticulierResponse {
    items: BalanceParticulier[];
}

export interface BalanceParticulierGridResponse {
    paginator: BalancePaginator;
    items: BalanceParticulier[];
    sommeTotal: number;
}



