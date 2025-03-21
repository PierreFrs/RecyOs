import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '../../../../@fuse/components/card';
import { MatDialogModule } from '@angular/material/dialog';
import { BusinessesModule } from '../businesses/businesses.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgClass, NgSwitch } from '@angular/common';
import {
    OperationsCardComponent
} from "../../../shared/cards/referentiels-cards/operations-card/operations-card.component";

@NgModule({
    imports: [
        CommonModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatRadioModule,
        MatSelectModule,
        MatSidenavModule,
        MatSlideToggleModule,
        SharedModule,
        FuseCardModule,
        MatDialogModule,
        BusinessesModule,
        ReactiveFormsModule,
        NgClass,
        NgSwitch,
        OperationsCardComponent,
    ],
})
export class CustomerOperationsModule {}
