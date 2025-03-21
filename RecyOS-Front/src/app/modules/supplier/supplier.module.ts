import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { supplierRoutes } from './supplier.routing';

@NgModule({
    imports: [RouterModule.forChild(supplierRoutes)],
})
export class SupplierModule {}
