import { NgModule } from '@angular/core';
import { EuropeListComponent } from './europe-list/europe-list.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { SharedModule } from '../../../shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { EuropeServices } from '../../../core/services/entity-services/europe.services';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { EuropeParametrageTabResolver } from '../../../core/resolvers/europe-parametrage-tab.resolver';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ReactiveFormsModule } from '@angular/forms';
import { AsyncPipe, CommonModule, NgClass } from '@angular/common';
import { EntitiesListComponent } from '../../../shared/components/lists/entities-list/entities-list.component';

@NgModule({
    imports: [
        CommonModule,
        MatProgressBarModule,
        MatFormFieldModule,
        MatOptionModule,
        SharedModule,
        MatSelectModule,
        MatButtonModule,
        MatIconModule,
        MatInputModule,
        MatSortModule,
        MatPaginatorModule,
        MatTabsModule,
        MatDialogModule,
        MatCheckboxModule,
        ReactiveFormsModule,
        AsyncPipe,
        NgClass,
        EntitiesListComponent,
        EuropeListComponent,
    ],
    exports: [EuropeListComponent],
    providers: [EuropeServices, EuropeParametrageTabResolver],
})
export class EuropeModule {}
