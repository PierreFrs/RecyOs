import {FactorClientEuropeBuStrategyService} from "./factor-client-europe-bu-strategy.service";
import {FactorClientFranceBuStrategyService} from "./factor-client-france-bu-strategy.service";

export interface IFactorClientBuStrategyObject {
    region: 'europe' | 'france';
    strategy:
        | FactorClientEuropeBuStrategyService
        | FactorClientFranceBuStrategyService;
}

