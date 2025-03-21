import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { financesRoutes } from "./finances.routing";

@NgModule({
    imports: [
        RouterModule.forChild(financesRoutes)
    ]
})
export class FinancesModule {
}