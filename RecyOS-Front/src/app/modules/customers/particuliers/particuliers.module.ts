import {NgModule} from "@angular/core";
import {ChoiceMkgtCodeDialogComponent} from "../businesses/choice-mkgt-code-dialog/choice-mkgt-code-dialog";
import {ParticulierService} from "../../../core/services/entity-services/particulier.service";
import {EntitiesListComponent} from "../../../shared/components/lists/entities-list/entities-list.component";
import {ParticuliersListComponent} from "./particuliers-list/particuliers-list.component";

@NgModule({
    declarations: [ChoiceMkgtCodeDialogComponent],
    exports: [ChoiceMkgtCodeDialogComponent],
    providers: [ParticulierService],
    imports: [
        ParticuliersListComponent,
        EntitiesListComponent
    ],
})
export class ParticuliersModule {}
