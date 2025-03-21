import { EtablissementClientServiceStrategy } from './client-form-strategies/etablissement-client-service-strategy';
import { ClientEuropeServiceStrategy } from './client-form-strategies/client-europe-service-strategy';
import { FrenchSupplierServiceStrategy } from './suppliers-strategies/french-supplier-service-strategy';
import { EuropeSupplierServiceStrategy } from './suppliers-strategies/europe-supplier-service-strategy';
import {ParticulierServiceStrategy} from "./client-form-strategies/particulier-service-strategy";

export interface IStrategyObject {
    status: 'professional' | 'particulier';
    type: 'client' | 'supplier';
    region: 'europe' | 'france' | 'unknown';
    strategy:
        | EtablissementClientServiceStrategy
        | ClientEuropeServiceStrategy
        | ParticulierServiceStrategy
        | FrenchSupplierServiceStrategy
        | EuropeSupplierServiceStrategy
}
