export interface SocieteDto {
    id: number;
    nom: string;
    idOdoo?: number;
    createDate?: string;
    updateDate?: string;
    updatedAt: string | null;
    createdBy: string | null;
    updatedBy: string | null;
    isDeleted: boolean;
}

// Mise à jour de l'interface pour correspondre à la structure du backend
export interface SocietePaginator {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    endIndex?: number; // Optionnel car peut ne pas être présent dans la réponse du backend
    cost: number;
}

export interface SocieteResponse {
    items: SocieteDto[];
    totalCount?: number;
    paginator?: SocietePaginator;
}

export interface SocieteGridResponse {
    items: SocieteDto[];
    totalCount?: number;
    paginator?: SocietePaginator;
}

export interface SocieteGridParams {
    FilterBySocieteId?: string;
    FilterByNom?: string;
    FilterByIdOdoo?: string;
    FilterIsDeleted?: string;
    PageNumber?: number;
    PageSize?: number;
    SortBy?: string;
    OrderBy?: string;
}