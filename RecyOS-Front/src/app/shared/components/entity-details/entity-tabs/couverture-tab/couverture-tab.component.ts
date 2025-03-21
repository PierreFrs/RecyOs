import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../../@fuse/animations';
import { BusinessCouvertureServices } from '../../business-couverture/business-couverture.service';
import { EntrepriseCouvertureDTO } from '../../business-couverture/business-couverture.type';
import { EntrepriseBaseDTO } from '../../../../../models/entities-models/french.type';
import { BusinessesServices } from '../../../../../core/services/entity-services/businesses.services';

@Component({
    selector: 'couverture-tab',
    templateUrl: './couverture-tab.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class CouvertureTabComponent implements OnInit {
    @Input() siretEtablissement: string;
    entrepriseCouverture: EntrepriseCouvertureDTO;
    selectedCouvertureId: number;
    entrepriseBaseClient: EntrepriseBaseDTO;
    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _businessCouvertureServices: BusinessCouvertureServices,
        private _businessesServices: BusinessesServices,
    ) {}

    ngOnInit(): void {
        if (this.siretEtablissement) {
            this.getEtablissementCouverture();
            this.getEntrepriseBase();
        }
    }

    private getEtablissementCouverture() {
        this._businessCouvertureServices
            .getEtablissementCouvertureBySiret(this.siretEtablissement)
            .subscribe((etablissementCouverture) => {
                if (etablissementCouverture) {
                    this.entrepriseCouverture = etablissementCouverture; // Store the actual data
                    this.selectedCouvertureId = etablissementCouverture.id;
                }
                this._changeDetectorRef.markForCheck();
            });
    }
    private getEntrepriseBase() {
        this._businessesServices
            .getEntrepriseBaseBySiret(this.siretEtablissement)
            .subscribe((entrepriseBase) => {
                if (entrepriseBase) {
                    this.entrepriseBaseClient = entrepriseBase;
                }
                this._changeDetectorRef.markForCheck();
            });
    }
}
