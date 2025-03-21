import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { commerciauxRoutes } from './commerciaux.routing';
import { CommonModule } from '@angular/common';
import { SocietesResolver } from '../../../core/resolvers/societes.resolver';

@NgModule({
    imports: [CommonModule, RouterModule.forChild(commerciauxRoutes)],
    providers: [SocietesResolver],
})
export class CommerciauxModule {}
