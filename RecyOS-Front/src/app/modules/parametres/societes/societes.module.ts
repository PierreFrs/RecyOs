import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { societesRoutes } from './societes.routing';
import { SocietesListComponent } from './components/societes-list/societes-list.component';
import { SocietesDetailsComponent } from './components/societes-details/societes-details.component';
import { NewSocieteDialogComponent } from './components/new-societe-dialog/new-societe-dialog.component';
import { ConfirmSocieteDeleteComponent } from './components/societes-details/confirm-societe-delete/confirm-societe-delete.component';
import { AsyncPipe, DatePipe, NgClass, NgForOf, NgIf, NgTemplateOutlet } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'app/shared/shared.module';
import { MatDialogModule } from '@angular/material/dialog';
import {InputFieldComponent} from "../../../shared/components/form-components/input-field/input-field.component";

@NgModule({
    declarations: [
        SocietesListComponent,
        SocietesDetailsComponent,
        NewSocieteDialogComponent,
        ConfirmSocieteDeleteComponent
    ],
    imports: [
        RouterModule.forChild(societesRoutes),
        AsyncPipe,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatSelectModule,
        MatSortModule,
        NgForOf,
        NgIf,
        NgTemplateOutlet,
        ReactiveFormsModule,
        SharedModule,
        NgClass,
        DatePipe,
        MatDialogModule,
        InputFieldComponent
    ]
})
export class SocietesModule {}
