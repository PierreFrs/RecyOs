import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { FuseModule } from '@fuse';
import { FuseConfigModule } from '@fuse/services/config';
import { FuseMockApiModule } from '@fuse/lib/mock-api';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { mockApiServices } from 'app/mock-api';
import { LayoutModule } from 'app/layout/layout.module';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import { CustomPaginator } from './customPaginator';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { CommonModule, registerLocaleData } from '@angular/common';
import localeFr from '@angular/common/locales/fr';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ConfigService } from './config.service';
import { firstValueFrom } from 'rxjs';

registerLocaleData(localeFr);

const routerConfig: ExtraOptions = {
    preloadingStrategy: PreloadAllModules,
    scrollPositionRestoration: 'enabled',
};

export function initializeApp(configService: ConfigService) {
    return () =>
        firstValueFrom(configService.loadConfig()).then(
        () => console.log('Configuration initialisée'),
            (error) =>
                console.error(
                    'Erreur de chargement de la configuration:',
                    error
                ) // Log d'erreur
    );
}

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CommonModule,
        RouterModule.forRoot(appRoutes, routerConfig),
        FuseModule,
        FuseConfigModule.forRoot(appConfig),
        FuseMockApiModule.forRoot(mockApiServices),
        CoreModule,
        LayoutModule,
        ReactiveFormsModule,
        SharedModule,
        MatIconModule,
        MatTooltipModule,
        MatDialogModule,
        MatButtonModule,
    ],
    bootstrap: [AppComponent],
    providers: [
        { provide: MatPaginatorIntl, useValue: CustomPaginator() },
        {
            provide: APP_INITIALIZER,
            useFactory: initializeApp,
            deps: [ConfigService],
            multi: true,
        },
    ],
    exports: [],
})
export class AppModule {}
