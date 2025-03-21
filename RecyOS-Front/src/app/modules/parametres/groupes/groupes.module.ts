import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { groupesRoutes } from './groupes.routing';

@NgModule({
    imports: [CommonModule, RouterModule.forChild(groupesRoutes)],
})
export class GroupesModule {}
