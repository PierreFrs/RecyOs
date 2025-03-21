import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ChangelogComponent } from './changelog';
import { changelogRoutes } from './changelog.routing';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [ChangelogComponent],
    imports: [CommonModule, RouterModule.forChild(changelogRoutes)],
})
export class ChangelogModule {}
