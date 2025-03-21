import { EuropeDTO } from './entities-models/europe.type';
import { FrenchDTO } from './entities-models/french.type';

export type Group = {
    id: number;
    name: string;
    etablissementClients: FrenchDTO[];
    clientEuropes: EuropeDTO[];
    ficheCount: number;
};

export type GroupPagination = {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
};
