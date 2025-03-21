import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { SharedModule } from 'app/shared/shared.module';
import { BalancesFranceComponent } from './balances-france.component';
import { balancesFranceRoutes } from './balances-france.routing';
import { ReactiveFormsModule } from '@angular/forms';
import { CreditAmountPipe } from '../../../shared/pipes/credit-amount.pipe';

@NgModule({
    declarations: [
        BalancesFranceComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(balancesFranceRoutes),
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatTableModule,
        MatSortModule,
        MatPaginatorModule,
        MatSelectModule,
        SharedModule,
        ReactiveFormsModule,
        CreditAmountPipe
    ]
})
export class BalancesFranceModule { }