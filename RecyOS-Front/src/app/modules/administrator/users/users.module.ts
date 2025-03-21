import { NgModule } from '@angular/core';
import { UsersListComponent } from './list/users.list.component';
import { RouterModule } from '@angular/router';
import { usersRoutes } from './users.routing';
import { SharedModule } from 'app/shared/shared.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { UsersComponent } from './users.component';
import { DetailsUsersComponent } from './details/details.users.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NewUserDialogComponent } from './new-user-dialog/new-user-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
    AsyncPipe,
    CommonModule,
    I18nPluralPipe,
    NgClass,
} from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
@NgModule({
    declarations: [
        UsersComponent,
        UsersListComponent,
        DetailsUsersComponent,
        NewUserDialogComponent,
    ],
    imports: [
        CommonModule,
        MatButtonModule,
        MatCheckboxModule,
        MatInputModule,
        MatSidenavModule,
        MatFormFieldModule,
        MatIconModule,
        MatTooltipModule,
        RouterModule.forChild(usersRoutes),
        SharedModule,
        ReactiveFormsModule,
        I18nPluralPipe,
        AsyncPipe,
        NgClass,
        MatSelectModule,
    ],
})
export class UsersModule {}
