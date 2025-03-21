import { Component } from '@angular/core';
import {EntitiesListComponent} from "../../../../shared/components/lists/entities-list/entities-list.component";

@Component({
    selector: 'app-particuliers-list',
    templateUrl: './particuliers-list.component.html',
    imports: [
        EntitiesListComponent
    ],
    standalone: true
})
export class ParticuliersListComponent {

}
