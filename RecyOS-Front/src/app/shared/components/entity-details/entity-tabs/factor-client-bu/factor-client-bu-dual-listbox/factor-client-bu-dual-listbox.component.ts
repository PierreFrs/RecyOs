import {ChangeDetectorRef, Component, Input, OnInit, ViewEncapsulation} from '@angular/core';
import { ClientDTO } from "../../../../../../models/entities-models/client.type";
import { BusinessUnitDto } from "../../../../../../models/business-unit.type";
import { ActivatedRoute } from "@angular/router";
import { IFactorClientBuStrategyObject } from "../../../../../../core/strategies/factor-client-bu-strategy/IFactorClientBuStrategyObject";
import { FactorClientBuStrategyService } from "../../../../../../core/strategies/factor-client-bu-strategy/factor-client-bu-strategy-service";
import { FactorClientBu } from "../../../../../../models/factor-client-bu-models/factor-client-bu.type";
import { MatListModule } from "@angular/material/list";
import { MatIconModule } from "@angular/material/icon";
import { NgForOf } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import {Observable} from "rxjs";
import {FactorBatchRequest} from "../../../../../../models/factor-client-bu-models/factor-batch-request.type";

@Component({
    selector: 'app-factor-client-bu-dual-listbox',
    templateUrl: './factor-client-bu-dual-listbox.component.html',
    styleUrls: ['./factor-client-bu-dual-listbox.component.scss'],
    standalone: true,
    imports: [
        MatListModule,
        MatIconModule,
        NgForOf,
        MatButtonModule
    ],
    encapsulation: ViewEncapsulation.None
})
export class FactorClientBuDualListboxComponent implements OnInit {
    @Input() selectedClient: ClientDTO;
    @Input() updateFactorSignal: Observable<void>;

    businessUnits: BusinessUnitDto[] = [];
    clientBusinessUnits: BusinessUnitDto[] = [];
    selectedItems = {
        available: new Set<BusinessUnitDto>(),
        assigned: new Set<BusinessUnitDto>()
    };
    strategyObject: IFactorClientBuStrategyObject;

    constructor(
        private readonly _route: ActivatedRoute,
        private readonly _factorClientBuStrategyService: FactorClientBuStrategyService,
        private readonly cdr: ChangeDetectorRef
    ) {}

    ngOnInit() {
        this._route.data.subscribe(data => {
            this.businessUnits = data.businessUnits || [];
        });
        this.strategyObject = this._factorClientBuStrategyService.determineStrategyFromEntity(this.selectedClient);
        this.getFactorClientBusinessUnits();
        this.updateFactorSignal.subscribe(() => {
            this.handleUpdateRequest();
        });
    }

    private getFactorClientBusinessUnits() {
        this.strategyObject.strategy.fetchFactorClientBuByIdClient(this.selectedClient.id)
            .subscribe((factorClientBu: FactorClientBu[]) => {
                const assignedBuIds = factorClientBu ? factorClientBu.map(f => f.idBu) : [];
                this.clientBusinessUnits = this.businessUnits.filter(bu => assignedBuIds.includes(bu.id));
                this.businessUnits = this.businessUnits.filter(bu => !assignedBuIds.includes(bu.id));
                this.cdr.detectChanges();
            });
    }



    toggleSelection(item: BusinessUnitDto, listType: 'available' | 'assigned') {
        const selectedSet = this.selectedItems[listType];
        if (selectedSet.has(item)) {
            selectedSet.delete(item);
        } else {
            selectedSet.add(item);
        }
    }

    isSelected(item: BusinessUnitDto, listType: 'available' | 'assigned'): boolean {
        return this.selectedItems[listType].has(item);
    }

    moveSelected(selectedItems: Set<BusinessUnitDto>, direction: 'available' | 'assigned') {
        if (direction === 'available') {
            selectedItems.forEach(item => {
                const index = this.clientBusinessUnits.indexOf(item);
                if (index !== -1) {
                    this.clientBusinessUnits.splice(index, 1);
                    this.businessUnits.push(item);
                }
            });
        } else {
            selectedItems.forEach(item => {
                const index = this.businessUnits.indexOf(item);
                if (index !== -1) {
                    this.businessUnits.splice(index, 1);
                    this.clientBusinessUnits.push(item);
                }
            });
        }
        this.clearSelection();
    }

    moveAll(direction: 'available' | 'assigned') {
        if (direction === 'available') {
            this.clientBusinessUnits.forEach(bu => this.businessUnits.push(bu));
            this.clientBusinessUnits = [];
        } else {
            this.businessUnits.forEach(bu => this.clientBusinessUnits.push(bu));
            this.businessUnits = [];
        }
        this.clearSelection();
    }

    selectAll(list: 'available' | 'assigned') {
        if (list === 'available') {
            this.selectedItems.available = new Set(this.businessUnits);
        } else {
            this.selectedItems.assigned = new Set(this.clientBusinessUnits);
        }
    }

    clearSelection() {
        this.selectedItems.available.clear();
        this.selectedItems.assigned.clear();
    }

    handleUpdateRequest() {
        const buIds = this.clientBusinessUnits.map(bu => bu.id);

        const request: FactorBatchRequest = {
            clientId: this.selectedClient.id,
            buIds: buIds
        };

        this.strategyObject.strategy.updateFactorClientBu(request).subscribe({
            next: (response: FactorClientBu[]) => {
                console.log('Update successful:', response);
            },
            error: (error) => {
                console.error('Update failed:', error);
            }
        });
    }
}
