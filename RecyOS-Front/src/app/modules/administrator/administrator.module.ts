import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { administratorRoutes } from './administrator.routing';

@NgModule({
    imports: [RouterModule.forChild(administratorRoutes)],
})
export class AdministratorModule {}
