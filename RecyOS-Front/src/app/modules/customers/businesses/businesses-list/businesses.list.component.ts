import {
    ChangeDetectionStrategy,
    Component,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../@fuse/animations';
import { EntitiesListComponent } from 'app/shared/components/lists/entities-list/entities-list.component';
@Component({
    selector: 'businesses-list',
    templateUrl: './businesses.list.component.html',
    standalone: true,
    imports: [EntitiesListComponent],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class BusinessesListComponent {
    constructor() {}
}
