import {
    ChangeDetectorRef,
    Component,
    ElementRef,
    Input,
    OnDestroy,
    OnInit,
    Renderer2,
    TemplateRef,
    ViewChild,
    ViewContainerRef,
} from '@angular/core';
import {
    FormControl,
    FormGroup,
    UntypedFormControl,
    ReactiveFormsModule,
} from '@angular/forms';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import {
    CategorieClientDto,
    FrenchDTO,
} from '../../../../../models/entities-models/french.type';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../../models/business-unit.type';
import { Overlay, OverlayRef, OverlayModule } from '@angular/cdk/overlay';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { RoleAssignmentService } from '../../../../../core/services/role-assignment.service';
import {
    MatCheckboxChange,
    MatCheckboxModule,
} from '@angular/material/checkbox';
import { TemplatePortal, PortalModule } from '@angular/cdk/portal';
import { IEntityServiceStrategy } from '../../../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import { EntityStrategyService } from '../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IStrategyObject } from '../../../../../core/strategies/entity-strategy/IStrategyObject';
import { Commercial } from '../../../../../models/commercial.type';
import {
    EntityDto,
    EntityDTOPagination,
    EntityFormDto,
} from '../../../../../models/entities-models/entity.type';
import { Group } from 'app/models/group.type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SelectFieldComponent } from '../../../form-components/select-field/select-field.component';
import { BillingInfosFormGroupComponent } from '../../../form-components/form-groups/parametrage-form-group/billing-infos-form-group/billing-infos-form-group.component';
import { ErpCodesFormGroupComponent } from '../../../form-components/form-groups/parametrage-form-group/erp-codes-form-group/erp-codes-form-group.component';
import { EuropeDTO } from '../../../../../models/entities-models/europe.type';

@Component({
    selector: 'app-parametrage-tab',
    templateUrl: './parametrage-tab.component.html',
    standalone: true,
    styles: [
        `
            .placeholder-label {
                color: rgba(0, 0, 0, 0.6);
            }
        `,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatAutocompleteModule,
        MatInputModule,
        MatFormFieldModule,
        MatSelectModule,
        MatIconModule,
        MatCheckboxModule,
        MatButtonModule,
        OverlayModule,
        PortalModule,
        BillingInfosFormGroupComponent,
        ErpCodesFormGroupComponent,
        SelectFieldComponent,
    ],
})
export class ParametrageTabComponent implements OnInit, OnDestroy {
    /**
     * Component Properties
     * Includes inputs, view children, and state management
     */
    @Input() parametrageFormGroup: FormGroup;
    @Input() selectedEntity: EntityDto;
    @Input() readonlyConditions: {
        codeMkgt: boolean;
        idOdoo: boolean;
        codeGpi: boolean;
        frnCodeGpi: boolean;
        idDashdoc: boolean;
    };
    @Input() selectedEntityForm: FormGroup;
    strategyObject?: IStrategyObject;

    @ViewChild('buSelect') buSelect: MatSelect;
    @ViewChild('businessUnitsPanel') businessUnitPanel: TemplateRef<any>;
    @ViewChild('businessUnitsPanelOrigin') businessUnitPanelOrigin: ElementRef;
    @ViewChild('searchInput') searchInput: ElementRef;

    categories?: CategorieClientDto[] = [];
    businessUnits: BusinessUnitDto[] = [];
    associatedBusinessUnits: BusinessUnitDto[] = [];
    selectedBusinessUnits = new FormControl([]);
    previousSelection: number[] = [];
    filteredBusinessUnits: BusinessUnitDto[] = [];
    commerciaux: Commercial[] = [];
    commerciauxOptions: { value: number; label: string }[] = [];
    groups: Group[] = [];
    filteredGroups: Group[] = [];

    protected readonly FormControl = FormControl;

    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;
    private tagsPanelOverlayRef: OverlayRef;
    private readonly unsubscribeAll: Subject<any> = new Subject<any>();

    /** Constructor */
    constructor(
        private readonly _route: ActivatedRoute,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _overlay: Overlay,
        private readonly _renderer2: Renderer2,
        private readonly _viewContainerRef: ViewContainerRef,
        private readonly _roleAssignmentService: RoleAssignmentService,
        private readonly _entityStrategyService: EntityStrategyService
    ) {}

    /**
     * Lifecycle Methods
     * Handles component initialization and cleanup
     */
    async ngOnInit() {
        // Wait for data to be loaded
        await this.importResolversData();

        // Initialize strategy first, with null check
        if (this.selectedEntity) {
            this.strategyObject =
                this._entityStrategyService.determineStrategyFromEntity(
                    this.selectedEntity
                );
            if (this.strategyObject) {
                this.entityServiceStrategy = this.strategyObject.strategy;
            }
        }

        // Then initialize form controls
        this.initializeFormControls();

        // Then populate options
        this.populateCommerciauxOptions();
        this.filteredGroups = this.groups;

        // Finally, handle business units
        if ('client' in this.selectedEntity && this.selectedEntity.client) {
            this.fetchBusinessUnits();
        }
        this.initializeFormControlsForBusinessUnits();
    }

    ngOnDestroy(): void {
        this.unsubscribeAll.next(null);
        this.unsubscribeAll.complete();
    }

    /**
     * Form Control Getters
     * Provides access to individual form controls
     */
    get erpCodesFormGroup(): FormGroup {
        return this.parametrageFormGroup.get('erpCodesForm') as FormGroup;
    }

    get billingInfosFormGroup(): FormGroup {
        return this.parametrageFormGroup.get('billingInfosForm') as FormGroup;
    }

    get categorieIdControl(): FormControl {
        return this.parametrageFormGroup.contains('categorieId')
            ? (this.parametrageFormGroup.get('categorieId') as FormControl)
            : null;
    }

    get commercialIdControl(): FormControl {
        return this.parametrageFormGroup.get('commercialId') as FormControl;
    }

    get groupIdControl(): FormControl {
        return this.parametrageFormGroup.get('groupId') as FormControl;
    }

    get canUpdate(): boolean {
        if (!this.strategyObject) {
            return false;
        }
        return this._roleAssignmentService.getCanUpdate(
            this.strategyObject.type,
            this.strategyObject.status
        );
    }

    /**
     * Data Initialization Methods
     * Handles loading and setting up initial data
     */
    private importResolversData(): Promise<void> {
        return new Promise((resolve) => {
            this._route.data.subscribe((data) => {
                this.categories = data['categories'];
                this.businessUnits = data['businessUnits'];
                this.commerciaux = data['commerciaux'];
                this.groups = data['groups'];
                resolve();
            });
        });
    }

    private initializeFormControls(): void {
        if (!this.parametrageFormGroup.contains('groupId')) {
            let initialGroupId: number;

            if (this.hasGroupId(this.selectedEntity)) {
                initialGroupId = this.selectedEntity.groupId;
            } else {
                const naGroup = this.groups.find((g) => g.id === 1);
                initialGroupId = naGroup ? naGroup.id : 1;
            }

            const groupControl = new FormControl(initialGroupId);
            this.parametrageFormGroup.addControl('groupId', groupControl);

            setTimeout(() => {
                groupControl.setValue(initialGroupId, { emitEvent: true });
                this._changeDetectorRef.markForCheck();
            });

            groupControl.valueChanges.subscribe((value) => {
                this._changeDetectorRef.markForCheck();
            });
        }
    }

    private populateCommerciauxOptions(): void {
        if (this.commerciaux) {
            this.commerciauxOptions = this.commerciaux.map((commercial) => ({
                value: commercial.id,
                label: `${commercial.firstname} ${commercial.lastname}`,
            }));
        }
    }

    /**
     * Group Management Methods
     * Handles group selection and filtering functionality
     */
    displayGroupFn = (groupId: number | null): string => {
        const group = this.groups.find((g) => g.id === groupId);
        return group ? group.name : 'NA';
    };

    private _filter(value: string): Group[] {
        const filterValue = value.toLowerCase();
        return this.groups.filter((group) =>
            group.name.toLowerCase().includes(filterValue)
        );
    }

    filterGroups(event: Event): void {
        const filterValue = (event.target as HTMLInputElement).value;
        this.filteredGroups = this._filter(filterValue);
    }

    private hasGroupId(entity: EntityDto): entity is FrenchDTO | EuropeDTO {
        return 'groupId' in entity;
    }

    compareIds(id1: number, id2: number): boolean {
        return id1 === id2;
    }

    /**
     * Business Units Management
     * Handles fetching and managing business unit associations
     */
    private fetchBusinessUnits(): void {
        this.entityServiceStrategy
            .getBusinessUnitsByEntityId(this.selectedEntity.id)
            .subscribe({
                next: this.handleBusinessUnitsFetched.bind(this),
                error: this.handleBusinessUnitsFetchError.bind(this),
            });
    }

    private handleBusinessUnitsFetched(businessUnits: BusinessUnitDto[]): void {
        this.associatedBusinessUnits = businessUnits;
        const associatedIds = businessUnits.map((unit) => unit.id);
        this.parametrageFormGroup.controls['businessUnitIds'].setValue(
            associatedIds || []
        );
        this._changeDetectorRef.markForCheck();
    }

    private handleBusinessUnitsFetchError(error: any): void {
        console.error('Error getting associated business units:', error);
    }

    private initializeFormControlsForBusinessUnits(): void {
        this.parametrageFormGroup.addControl(
            'businessUnitIds',
            new UntypedFormControl([])
        );

        this.previousSelection =
            this.parametrageFormGroup.get('businessUnitIds').value || [];
    }

    private manageBusinessUnitAssociation(
        businessUnitId: number,
        isAdding: boolean
    ): void {
        const actionObservable = isAdding
            ? this.entityServiceStrategy.addBusinessUnitToEntity({
                  clientId: this.selectedEntity.id,
                  businessUnitId,
              })
            : this.entityServiceStrategy.removeBusinessUnitFromEntity({
                  clientId: this.selectedEntity.id,
                  businessUnitId,
              });

        actionObservable.subscribe({
            next: () => this.updateBusinessUnitIds(businessUnitId, isAdding),
            error: (error) =>
                this.handleError(error, isAdding ? 'adding' : 'removing'),
        });
    }

    private updateBusinessUnitIds(
        businessUnitId: number,
        isAdding: boolean
    ): void {
        const currentIds: number[] =
            this.parametrageFormGroup.get('businessUnitIds').value || [];
        const updatedIds = isAdding
            ? [...currentIds, businessUnitId]
            : currentIds.filter((id) => id !== businessUnitId);
        this.parametrageFormGroup.get('businessUnitIds').setValue(updatedIds);
        this.syncSelectedBusinessUnits();
    }

    private handleError(error: any, action: string): void {
        console.error(`Error ${action} business unit:`, error);
    }

    private syncSelectedBusinessUnits(): void {
        const selectedIds =
            this.parametrageFormGroup.get('businessUnitIds').value || [];
        this.associatedBusinessUnits = this.businessUnits.filter((unit) =>
            selectedIds.includes(unit.id)
        );
        this._changeDetectorRef.markForCheck();
    }

    businessUnitIsChecked(businessUnit: BusinessUnitDto): boolean {
        return this.associatedBusinessUnits
            ?.map((r) => r.id)
            .includes(businessUnit.id);
    }

    /**
     * Business Units Panel UI
     * Handles the overlay panel for business unit selection
     */
    openBusinessUnitsPanel(): void {
        this.createOverlay();
        this.attachBusinessUnitsPanel();
        this.setupBackdropClickHandler();
    }

    private createOverlay(): void {
        this.tagsPanelOverlayRef = this._overlay.create({
            backdropClass: '',
            hasBackdrop: true,
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this.getOverlayPositionStrategy(),
        });
    }

    private getOverlayPositionStrategy(): any {
        return this._overlay
            .position()
            .flexibleConnectedTo(this.businessUnitPanelOrigin.nativeElement)
            .withFlexibleDimensions(true)
            .withViewportMargin(64)
            .withLockedPosition(true)
            .withPositions([
                {
                    originX: 'start',
                    originY: 'bottom',
                    overlayX: 'start',
                    overlayY: 'top',
                },
            ]);
    }

    private attachBusinessUnitsPanel(): void {
        const templatePortal = new TemplatePortal(
            this.businessUnitPanel,
            this._viewContainerRef
        );
        this.tagsPanelOverlayRef.attach(templatePortal);
        this.handleOverlayAttachments();
    }

    private handleOverlayAttachments(): void {
        this.tagsPanelOverlayRef.attachments().subscribe(() => {
            this._renderer2.addClass(
                this.businessUnitPanelOrigin.nativeElement,
                'panel-opened'
            );
            this.focusOnOverlayInput();
        });
    }

    private focusOnOverlayInput(): void {
        this.tagsPanelOverlayRef.overlayElement.querySelector('input').focus();
    }

    private setupBackdropClickHandler(): void {
        this.tagsPanelOverlayRef
            .backdropClick()
            .subscribe(() => this.closeOverlay());
    }

    private closeOverlay(): void {
        this._renderer2.removeClass(
            this.businessUnitPanelOrigin.nativeElement,
            'panel-opened'
        );
        if (this.tagsPanelOverlayRef?.hasAttached()) {
            this.tagsPanelOverlayRef.detach();
            this.filteredBusinessUnits = this.businessUnits;
        }
    }

    toggleBusinessUnitSelection(unitId: number): void {
        const currentSelection = this.selectedBusinessUnits.value;
        const index = currentSelection.indexOf(unitId);
        if (index > -1) {
            currentSelection.splice(index, 1);
        } else {
            currentSelection.push(unitId);
        }
        this.selectedBusinessUnits.setValue([...currentSelection]);
    }

    /**
     * Utility Methods
     * Helper methods for template and component functionality
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    /**
     * Business Units Management
     * Handles fetching and managing business unit associations
     */
    onBusinessUnitCheckboxChange(
        event: MatCheckboxChange,
        businessUnitId: number
    ): void {
        if (event.checked) {
            this.manageBusinessUnitAssociation(businessUnitId, true);
        } else {
            this.manageBusinessUnitAssociation(businessUnitId, false);
        }
    }
}
