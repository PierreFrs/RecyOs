import {Injectable} from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class ParticulierEntityFilterService {
    constructor() {
    }

    getfilterParam(filterType: number = 0): string {
        switch (filterType) {
            case 0:
                return  'FilterByNom';
            case 2:
                return 'FilterByCodeMkgt';
            case 3:
                return 'FilterByIdOdoo';
            case 9:
                return 'FilterByIdDashdoc';
            case 4:
                return 'BadMail';
            case 5:
                return 'BadTel';
            default:
                return '';

        }
    }
}
