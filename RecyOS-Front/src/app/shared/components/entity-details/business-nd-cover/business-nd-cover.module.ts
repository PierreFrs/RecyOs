import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BusinessNDCoverServices } from '../../../../core/services/business-nd-cover.service';
import { BusinessNDCoverDetailComponent } from './business-nd-cover-detail/business-nd-cover-detail.component';
import {FormatSirenPipe} from "../../../pipes/format-siren.pipe";

@NgModule({
    declarations: [BusinessNDCoverDetailComponent],
    exports: [BusinessNDCoverDetailComponent],
    providers: [BusinessNDCoverServices],
    imports: [
        MatIconModule,
        NgIf,
        CommonModule,
        FormsModule,
        MatTooltipModule,
        FormatSirenPipe,
    ],
})
export class BusinessNDCoverModule {}
