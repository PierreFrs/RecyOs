import { FrenchDTO } from "app/models/entities-models/french.type";
import { SocieteDto } from "app/models/societe.type";

export interface BalanceFrance {
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
    etablissementClient: FrenchDTO;
    societe: SocieteDto;
}

export interface BalanceFranceResponse {
    items: BalanceFrance[];
}

// Ajout de l'interface pour la pagination
export interface BalancePaginator {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}

// Interface pour la réponse paginée
export interface BalanceFranceGridResponse {
    paginator: BalancePaginator;
    items: BalanceFrance[];
    sommeTotal: number;
}

// Interface pour les paramètres de filtrage de la grille
export interface BalanceGridParams {
    FilterClientName?: string;
    FilterBySocieteId?: string;
    FilterByClientCommercialId?: string;
    PageNumber?: number;
    PageSize?: number;
    SortBy?: string;
    OrderBy?: string;
}