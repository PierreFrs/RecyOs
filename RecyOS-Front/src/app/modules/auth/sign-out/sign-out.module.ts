import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AuthSignOutComponent } from 'app/modules/auth/sign-out/sign-out.component';
import { authSignOutRoutes } from 'app/modules/auth/sign-out/sign-out.routing';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [AuthSignOutComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(authSignOutRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule,
    ],
})
export class AuthSignOutModule {}
