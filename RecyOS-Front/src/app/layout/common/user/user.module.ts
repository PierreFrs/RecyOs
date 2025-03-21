import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { UserComponent } from 'app/layout/common/user/user.component';
import { CommonModule, NgClass } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';

@NgModule({
    declarations: [UserComponent],
    imports: [
        CommonModule,
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        SharedModule,
        NgClass,
    ],
    exports: [UserComponent],
})
export class UserModule {}
