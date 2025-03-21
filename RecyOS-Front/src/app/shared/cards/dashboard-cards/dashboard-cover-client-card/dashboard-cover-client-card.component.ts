import { Component, EventEmitter, Input, Output } from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {MatMenuModule} from "@angular/material/menu";
import {MatIconModule} from "@angular/material/icon";
import {NgClass} from "@angular/common";

@Component({
    selector: 'app-dashboard-cover-client-card',
    templateUrl: './dashboard-cover-client-card.component.html',
    imports: [
        MatButtonModule,
        MatMenuModule,
        MatIconModule,
        NgClass
    ],
    standalone: true
})
export class DashboardCoverClientCardComponent {
    @Input() title: string;
    @Input() number: number;
    @Input() subtitle: string;
    @Input() numberClass: string;
    @Input() textClass: string;
    @Input() menuIcon: string;
    @Input() additionalText: string;
    @Input() additionalNumber: number;

    @Output() demandes = new EventEmitter<void>();
    @Output() demandesAccordees = new EventEmitter<void>();
    @Output() demandesRefusees = new EventEmitter<void>();

    onDemandes() {
        this.demandes.emit();
    }

    onDemandesAccordees() {
        this.demandesAccordees.emit();
    }

    onDemandesRefusees() {
        this.demandesRefusees.emit();
    }
}
