import {
    ChangeDetectionStrategy,
    Component,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../@fuse/animations';
import { CommonModule } from '@angular/common';
import { EntitiesListComponent } from 'app/shared/components/lists/entities-list/entities-list.component';
@Component({
    selector: 'app-europe-suppliers-list',
    templateUrl: './europe-suppliers-list.component.html',
    standalone: true,
    imports: [CommonModule, EntitiesListComponent],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class EuropeSuppliersListComponent {
    constructor() {}
}
