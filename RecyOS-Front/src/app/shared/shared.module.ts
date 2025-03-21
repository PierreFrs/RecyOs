import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RecygroupSpinnerComponent } from '../widgets/recygroup-spinner/recygroup-spinner.component';
import { ErpCreationWaitingDialog } from '../modules/customers/OdooCreationWaitDialog/ErpCreationWaitingDialog';
import { MatDialogModule } from '@angular/material/dialog';
import { TestWarningBannerComponent } from '../widgets/test-warning-banner/test-warning-banner.component';
import { NdCoverStatusComponent } from '../widgets/nd-cover-status/nd-cover-status.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AddButtonComponent } from './components/buttons/add-button/add-button.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { FuseCardModule } from '../../@fuse/components/card';
import { PhoneMaskDirective } from '../core/directives/phone-mask.directive';
import { BalancesTableComponent } from './components/tables/balances-table/balances-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { EtablissementClientServiceStrategy } from '../core/strategies/entity-strategy/client-form-strategies/etablissement-client-service-strategy';
import { ClientEuropeServiceStrategy } from '../core/strategies/entity-strategy/client-form-strategies/client-europe-service-strategy';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FrenchSupplierServiceStrategy } from '../core/strategies/entity-strategy/suppliers-strategies/french-supplier-service-strategy';
import { EuropeSupplierServiceStrategy } from '../core/strategies/entity-strategy/suppliers-strategies/europe-supplier-service-strategy';
import { MatTabsModule } from '@angular/material/tabs';
import { DocumentsTabComponent } from './components/entity-details/entity-tabs/documents-tab/documents-tab.component';
import { SirenTabComponent } from './components/entity-details/entity-tabs/siren-tab/siren-tab.component';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FilePreviewColComponent } from './components/entity-details/entity-tabs/documents-tab/file-preview-col/file-preview-col.component';
import { FilesListColComponent } from './components/entity-details/entity-tabs/documents-tab/files-list-col/files-list-col.component';
import { FilesTypesColComponent } from './components/entity-details/entity-tabs/documents-tab/files-types-col/files-types-col.component';
import { UploadPdfDialogComponent } from './components/entity-details/entity-tabs/documents-tab/files-list-col/upload-pdf-dialog/upload-pdf-dialog.component';
import { UpdatePdfDialogComponent } from './components/entity-details/entity-tabs/documents-tab/file-preview-col/pdf-viewer/update-pdf-dialog/update-pdf-dialog.component';
import { DeletePdfDialogComponent } from './components/entity-details/entity-tabs/documents-tab/file-preview-col/pdf-viewer/delete-pdf-dialog/delete-pdf-dialog.component';
import { PdfViewerComponent } from './components/entity-details/entity-tabs/documents-tab/file-preview-col/pdf-viewer/pdf-viewer.component';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BalancesTabComponent } from './components/entity-details/entity-tabs/balances-tab/balances-tab.component';
import { CouvertureTabComponent } from './components/entity-details/entity-tabs/couverture-tab/couverture-tab.component';
import { BusinessNDCoverModule } from './components/entity-details/business-nd-cover/business-nd-cover.module';
import { ClientCouvertureDetailComponent } from './components/entity-details/business-couverture/business-couverture-detail/client-couverture-detail.component';
import { StringToDatePipe } from './pipes/string-to-date.pipe';
import { UpdateSiretDialogComponent } from './components/dialogs/update-siret-dialog/update-siret-dialog.component';

@NgModule({
    declarations: [
        RecygroupSpinnerComponent,
        ErpCreationWaitingDialog,
        TestWarningBannerComponent,
        BalancesTableComponent,
        DocumentsTabComponent,
        SirenTabComponent,
        FilePreviewColComponent,
        FilesListColComponent,
        FilesTypesColComponent,
        UploadPdfDialogComponent,
        UpdatePdfDialogComponent,
        DeletePdfDialogComponent,
        PdfViewerComponent,
        BalancesTabComponent,
        CouvertureTabComponent,
        BalancesTableComponent,
        UpdateSiretDialogComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatIconModule,
        MatTooltipModule,
        MatButtonModule,
        MatInputModule,
        MatSelectModule,
        MatMenuModule,
        MatButtonModule,
        MatTableModule,
        MatSortModule,
        FuseCardModule,
        MatCheckboxModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatTabsModule,
        MatListModule,
        MatCardModule,
        MatSidenavModule,
        NgxExtendedPdfViewerModule,
        MatToolbarModule,
        BusinessNDCoverModule,
        NdCoverStatusComponent,
        ClientCouvertureDetailComponent,
        StringToDatePipe,
        PhoneMaskDirective,
        AddButtonComponent,
    ],
    exports: [
        FormsModule,
        RecygroupSpinnerComponent,
        ErpCreationWaitingDialog,
        TestWarningBannerComponent,
        BalancesTableComponent,
        DocumentsTabComponent,
        SirenTabComponent,
        BalancesTabComponent,
        CouvertureTabComponent,
    ],
    providers: [
        {
            provide: EtablissementClientServiceStrategy,
            useClass: EtablissementClientServiceStrategy,
        },
        {
            provide: ClientEuropeServiceStrategy,
            useClass: ClientEuropeServiceStrategy,
        },
        {
            provide: FrenchSupplierServiceStrategy,
            useClass: FrenchSupplierServiceStrategy,
        },
        {
            provide: EuropeSupplierServiceStrategy,
            useClass: EuropeSupplierServiceStrategy,
        },
    ],
})
export class SharedModule {}
