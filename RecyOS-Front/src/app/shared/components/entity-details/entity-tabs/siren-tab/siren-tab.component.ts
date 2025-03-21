import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { fuseAnimations } from '../../../../../../@fuse/animations';
import {
    EntrepriseBaseDTO,
    EtablissementFicheDTO,
} from '../../../../../models/entities-models/french.type';
import { BusinessesServices } from '../../../../../core/services/entity-services/businesses.services';

@Component({
    selector: 'siren-tab',
    templateUrl: './siren-tab.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class SirenTabComponent implements OnInit {
    @Input() siretEntity: string;
    selectedEtablissementFiche: EtablissementFicheDTO | null = null;
    selectedEntrepriseBase: EntrepriseBaseDTO | null = null;
    hasSirenData = false;

    /**
     * Constructor
     */
    constructor(
        private _businessesServices: BusinessesServices,
        private _changeDetectorRef: ChangeDetectorRef,
    ) {}

    ngOnInit(): void {
        this._businessesServices
            .getEntrepriseBaseBySiret(this.siretEntity)
            .subscribe((entrepriseBase: EntrepriseBaseDTO) => {
                this.selectedEntrepriseBase = entrepriseBase;

                this._businessesServices
                    .getEtablissementFicheBySiret(this.siretEntity)
                    .subscribe((etablissementFiche: EtablissementFicheDTO) => {
                        this.selectedEtablissementFiche = etablissementFiche;
                        this.hasSirenData = etablissementFiche !== null;
                        this._changeDetectorRef.markForCheck();
                    });
            });
    }
}
