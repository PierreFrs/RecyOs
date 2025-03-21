import { NgModule } from '@angular/core';
import { FrenchSuppliersListComponent } from './french-suppliers-list/french-suppliers-list.component';
import { SharedModule } from '../../../shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { EntitiesListComponent } from '../../../shared/components/lists/entities-list/entities-list.component';

@NgModule({
    declarations: [FrenchSuppliersListComponent],
    exports: [FrenchSuppliersListComponent],
    imports: [
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatOptionModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatSelectModule,
        MatSortModule,
        CommonModule,
        SharedModule,
        EntitiesListComponent,
    ],
    providers: [],
})
export class FrenchSupplierModule {}
