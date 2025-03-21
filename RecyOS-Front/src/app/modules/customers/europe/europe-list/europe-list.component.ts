import {
    ChangeDetectionStrategy,
    Component,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../@fuse/animations';
import { EntitiesListComponent } from 'app/shared/components/lists/entities-list/entities-list.component';
@Component({
    selector: 'europe-list',
    templateUrl: './europe-list.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
    standalone: true,
    imports: [EntitiesListComponent],
})
export class EuropeListComponent {
    constructor() {}
}
