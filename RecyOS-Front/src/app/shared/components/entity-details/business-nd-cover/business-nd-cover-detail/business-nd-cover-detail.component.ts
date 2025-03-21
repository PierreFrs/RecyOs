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
import { EntrepriseNDCoverDTO } from '../../../../../models/business-nd-cover.type';
import { BusinessNDCoverServices } from '../../../../../core/services/business-nd-cover.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'business-nd-cover-detail',
    templateUrl: './business-nd-cover-detail.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class BusinessNDCoverDetailComponent implements OnChanges, OnDestroy {
    @Input() BusinessNDCoverId: number;
    @Input() entrepriseNDCover: EntrepriseNDCoverDTO;

    value: string;

    private _unsubscribeAll = new Subject();
    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _businessesNDCoverServices: BusinessNDCoverServices,
    ) {}

    ngOnChanges(changes: SimpleChanges) {
        if (
            changes['BusinessNDCoverId'] &&
            this.BusinessNDCoverId !== undefined
        ) {
            this._businessesNDCoverServices
                .getEtablissementNDCoverById(this.BusinessNDCoverId)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe((entrepriseNDCover) => {
                    this.entrepriseNDCover = entrepriseNDCover;
                    this._changeDetectorRef.detectChanges();
                });
        }
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(undefined);
        this._unsubscribeAll.complete();
    }
}
