import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatRippleModule } from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { AngularIbanModule } from 'angular-iban';
import { MatDialogModule } from '@angular/material/dialog';
import { BusinessesServices } from '../../../core/services/entity-services/businesses.services';
import { PappersService } from '../../../core/services/pappers.service';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseNavigationModule } from '../../../../@fuse/components/navigation';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { ReactiveFormsModule } from '@angular/forms';
import { AsyncPipe, CommonModule, NgClass } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import { EntitiesListComponent } from '../../../shared/components/lists/entities-list/entities-list.component';

@NgModule({
    providers: [BusinessesServices, PappersService],
    imports: [
        CommonModule,
        MatButtonModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatRippleModule,
        MatSortModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatTooltipModule,
        MatTabsModule,
        AngularIbanModule,
        MatDialogModule,
        MatSidenavModule,
        FuseNavigationModule,
        MatTableModule,
        MatCardModule,
        MatStepperModule,
        ReactiveFormsModule,
        AsyncPipe,
        NgClass,
        SharedModule,
        EntitiesListComponent,
    ],
})
export class BusinessesModule {}
