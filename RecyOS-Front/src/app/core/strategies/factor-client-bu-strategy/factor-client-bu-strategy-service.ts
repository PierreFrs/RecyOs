import { IFactorClientBuStrategyObject } from "./IFactorClientBuStrategyObject";
import { FactorClientFranceBuStrategyService } from "./factor-client-france-bu-strategy.service";
import { FactorClientEuropeBuStrategyService } from "./factor-client-europe-bu-strategy.service";
import { Injectable } from '@angular/core';
import {EntityDto} from "../../../models/entities-models/entity.type";

@Injectable({
    providedIn: 'root'
})
export class FactorClientBuStrategyService {
    constructor(
        private readonly _factorClientFranceBuService: FactorClientFranceBuStrategyService,
        private readonly _factorClientEuropeBuService: FactorClientEuropeBuStrategyService
    ) {}

    determineStrategyFromEntity(entity: EntityDto): IFactorClientBuStrategyObject {
        let region: string;

        if ('vat' in entity) {
            region = 'europe';
        } else if ('siret' in entity) {
            region = 'france';
        } else {
            throw new Error('Region not found');
        }

        let strategy:
            | FactorClientFranceBuStrategyService
            | FactorClientEuropeBuStrategyService;

        switch (region) {
            case 'france':
                strategy = this._factorClientFranceBuService;
                break;
            case 'europe':
                strategy = this._factorClientEuropeBuService;
                break;
            default: {
                throw new Error('Region not found');
            }
        }

        return { region: region, strategy: strategy };
    }
}
