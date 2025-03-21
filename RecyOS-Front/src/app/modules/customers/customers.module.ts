import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { customersRoutes } from './customers.routing';

@NgModule({
    imports: [CommonModule, RouterModule.forChild(customersRoutes)],
})
export class CustomersModule {}
