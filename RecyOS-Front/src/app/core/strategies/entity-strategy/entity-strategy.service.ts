import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { FrenchSupplierServiceStrategy } from './suppliers-strategies/french-supplier-service-strategy';
import { EuropeSupplierServiceStrategy } from './suppliers-strategies/europe-supplier-service-strategy';
import { EtablissementClientServiceStrategy } from './client-form-strategies/etablissement-client-service-strategy';
import { ClientEuropeServiceStrategy } from './client-form-strategies/client-europe-service-strategy';
import { IFilesServiceStrategy } from '../files-strategies/IFilesServiceStrategy';
import { FranceFilesTabStrategyService } from '../files-strategies/france-files-tab-strategy.service';
import { EuropeFilesTabStrategyService } from '../files-strategies/europe-files-tab-strategy.service';
import { IStrategyObject } from './IStrategyObject';
import {ParticulierServiceStrategy} from "./client-form-strategies/particulier-service-strategy";
import {EntityDto} from "../../../models/entities-models/entity.type";

@Injectable({
    providedIn: 'root',
})
export class EntityStrategyService {
    constructor(
        private readonly router: Router,
        private readonly frenchSupplierServiceStrategy: FrenchSupplierServiceStrategy,
        private readonly europeSupplierServiceStrategy: EuropeSupplierServiceStrategy,
        private readonly frenchClientServiceStrategy: EtablissementClientServiceStrategy,
        private readonly europeClientServiceStrategy: ClientEuropeServiceStrategy,
        private readonly particulierServiceStrategy: ParticulierServiceStrategy,
        private readonly frenchFilesService: FranceFilesTabStrategyService,
        private readonly europeFilesService: EuropeFilesTabStrategyService,
    ) {}

    determineStrategy(): IStrategyObject {
        const url = this.router.url;
        let entityStatus: "professional" | "particulier";
        let region: "france" | "europe";
        let entityType: "client" | "supplier";

        // Extracting entityType and region from URL
        if (url.includes('/customers/')) {
            entityType = 'client';
        } else if (url.includes('/suppliers/')) {
            entityType = 'supplier';
        }

        if (url.includes('/europe')) {
            region = 'europe';
        } else if (url.includes('/businesses')) {
            region = 'france';
        }

        if (url.includes('/customer/lists')) {
            entityType = 'client';
            region = 'france';
        }

        if (url.includes('/particuliers')) {
            entityStatus = 'particulier';
        } else {
            entityStatus = 'professional';
        }

        let strategy:
            | EtablissementClientServiceStrategy
            | ClientEuropeServiceStrategy
            | FrenchSupplierServiceStrategy
            | EuropeSupplierServiceStrategy
            | ParticulierServiceStrategy;

        if (entityStatus === 'particulier') {
            strategy = this.particulierServiceStrategy;
        } else if (entityStatus === 'professional') {
            if (entityType === 'supplier') {
                switch (region) {
                    case 'france':
                        strategy = this.frenchSupplierServiceStrategy;
                        break;
                    case 'europe':
                        strategy = this.europeSupplierServiceStrategy;
                        break;
                    default:
                        throw new Error('Unsupported supplier region');
                }
            } else if (entityType === 'client') {
                switch (region) {
                    case 'france':
                        strategy = this.frenchClientServiceStrategy;
                        break;
                    case 'europe':
                        strategy = this.europeClientServiceStrategy;
                        break;
                    default:
                        throw new Error('Unsupported client region');
                }
            } else {
                throw new Error('Unsupported entity type');
            }
        }

        return<IStrategyObject> { status: entityStatus, type: entityType, region: region, strategy: strategy };
    }

    determineStrategyFromEntity(entity: EntityDto): IStrategyObject {
        const url = this.router.url;
        let entityStatus: "professional" | "particulier";
        let region: "france" | "europe" | "unknown";
        let entityType: "client" | "supplier";

        if ('titre' in entity) {
            entityStatus = 'particulier';
        } else {
            entityStatus = 'professional';
        }

        if ('vat' in entity) {
            region = 'europe';
        } else if ('siret' in entity) {
            region = 'france';
        } else {
            region = 'unknown';
        }

        if ('client' in entity && 'fournisseur' in entity) {
            if (entity.client === true && entity.fournisseur === false) {
                entityType = 'client';
            } else if (entity.client === false && entity.fournisseur === true) {
                entityType = 'supplier';
            } else if (
                entity.client === true &&
                entity.fournisseur === true &&
                url.includes('/customers/')
            ) {
                entityType = 'client';
            } else if (
                entity.client === true &&
                entity.fournisseur === true &&
                url.includes('/suppliers/')
            ) {
                entityType = 'supplier';
            }
        }else if (url.includes('/particuliers')) {
            entityType = 'client';
        } else {
            throw new Error('Unsupported entity type');
        }

        let strategy:
            | EtablissementClientServiceStrategy
            | ClientEuropeServiceStrategy
            | FrenchSupplierServiceStrategy
            | EuropeSupplierServiceStrategy
            | ParticulierServiceStrategy;

        if (entityStatus === 'professional') {
            if (entityType === 'supplier') {
                switch (region) {
                    case 'france':
                        strategy = this.frenchSupplierServiceStrategy;
                        break;
                    case 'europe':
                        strategy = this.europeSupplierServiceStrategy;
                        break;
                    default:
                        throw new Error('Unsupported supplier region');
                }
            } else if (entityType === 'client') {
                switch (region) {
                    case 'france':
                        strategy = this.frenchClientServiceStrategy;
                        break;
                    case 'europe':
                        strategy = this.europeClientServiceStrategy;
                        break;
                    default:
                        throw new Error('Unsupported client region');
                }
            }
        } else if (entityStatus === 'particulier') {
            strategy = this.particulierServiceStrategy;
        }
        else {
            throw new Error('Unsupported entity type');
        }

        return { status: entityStatus, type: entityType, region: region, strategy: strategy };
    }

    determineFilesStrategyFromEntity(entity: EntityDto): IFilesServiceStrategy {
        if ('vat' in entity) {
            return this.europeFilesService;
        } else if ('siret' in entity) {
            return this.frenchFilesService;
        } else {
            throw new Error('Unsupported region or entity type');
        }
    }
}
