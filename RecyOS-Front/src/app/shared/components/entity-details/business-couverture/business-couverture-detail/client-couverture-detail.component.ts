import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnChanges,
    OnDestroy,
    SimpleChanges,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../../@fuse/animations';
import { EntrepriseCouvertureDTO } from '../business-couverture.type';
import { BusinessCouvertureServices } from '../business-couverture.service';
import { Subject, takeUntil } from 'rxjs';
import { EntrepriseBaseDTO } from '../../../../../models/entities-models/french.type';
import { MatIconModule } from '@angular/material/icon';
import { NdCoverStatusComponent } from '../../../../../widgets/nd-cover-status/nd-cover-status.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule, DatePipe, DecimalPipe, NgClass } from '@angular/common';
import { FormatSirenPipe } from '../../../../pipes/format-siren.pipe';
import { ColorPipe } from '../../../../pipes/color.pipe';
import { TrimZeroPipe } from '../../../../pipes/TrimZero.pipe';

@Component({
    selector: 'client-couverture-detail',
    templateUrl: './client-couverture-detail.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
    standalone: true,
    imports: [
        CommonModule,
        MatIconModule,
        NdCoverStatusComponent,
        MatTooltipModule,
        NgClass,
        DatePipe,
        DecimalPipe,
        FormatSirenPipe,
        ColorPipe,
        TrimZeroPipe,
    ],
})
export class ClientCouvertureDetailComponent implements OnChanges, OnDestroy {
    @Input() BusinessCoverId: number;
    @Input() entrepriseCouverture: EntrepriseCouvertureDTO;
    @Input() entrepriseBaseClient?: EntrepriseBaseDTO;
    value: string;

    private _unsubscribeAll = new Subject();
    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _businessesCouvertureServices: BusinessCouvertureServices,
    ) {}

    ngOnChanges(changes: SimpleChanges) {
        if (changes['BusinessCoverId'] && this.BusinessCoverId !== undefined) {
            this._businessesCouvertureServices
                .getEtablissementCouvertureById(this.BusinessCoverId)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe((entrepriseCouverture) => {
                    this.entrepriseCouverture = entrepriseCouverture;
                    this._changeDetectorRef.detectChanges();
                });
        }
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(undefined);
        this._unsubscribeAll.complete();
    }
}
