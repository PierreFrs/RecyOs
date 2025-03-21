import { NgModule } from '@angular/core';
import { SharedModule } from 'app/shared/shared.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ReactiveFormsModule } from '@angular/forms';
import {
    CommonModule,
    I18nPluralPipe,
} from '@angular/common';
import { SettingsComponent } from './settings.component';
import { settingsRoutes } from './settings.routing';
import { RouterModule } from '@angular/router';
import { ParameterControlComponent } from './parameter-control.compomemt/parameter-control.component';
import { GeneralComponent } from './general/general.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { EngineComponent } from './engine/engine.component';
import { DbParameterControleComponent } from './db-parameter-contol/db-parameter-controle.component';

@NgModule({
    declarations: [
        SettingsComponent,
        GeneralComponent,
        EngineComponent,
        DbParameterControleComponent
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
        MatSlideToggleModule,
        RouterModule.forChild(settingsRoutes),
        SharedModule,
        ReactiveFormsModule,
        I18nPluralPipe,
        ParameterControlComponent
    ]
})
export class SettingsModule { }