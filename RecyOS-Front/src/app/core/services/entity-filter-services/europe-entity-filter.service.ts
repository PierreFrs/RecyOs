import {Injectable} from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class EuropeEntityFilterService {
    constructor() {
    }

    getfilterParam(filterType: number = 0): string {
        switch (filterType) {
            case 0:
                return  'FiltredByNom';
            case 1:
                return 'FiltredByVat';
            case 2:
                return 'FiltredByCodeMkgt';
            case 3:
                return 'FiltredByIdOdoo';
            case 4:
                return 'BadMail';
            case 5:
                return 'BadTel';
            case 6:
                return 'Factor';
            case 7:
                return 'Radie';
            case 8:
                return 'FiltredByVat';
            case 9:
                return 'FilteredByIdDashdoc';

            default:
                return '';

        }
    }
}
