import { FrenchDTO } from './entities-models/french.type';
import { EuropeDTO } from './entities-models/europe.type';

export type Commercial = {
    id: number;
    firstname: string;
    lastname: string;
    username: string;
    phone: string;
    email: string;
    codeMkgt: string;
    idHubSpot?: string;
    etablissementClients: FrenchDTO[];
    clientEuropes: EuropeDTO[];
    userId?: number;
};

export interface CommercialPagination {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}
