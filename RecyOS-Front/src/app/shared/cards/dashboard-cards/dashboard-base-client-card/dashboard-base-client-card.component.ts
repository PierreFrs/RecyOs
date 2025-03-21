import { Component, EventEmitter, Input, Output } from '@angular/core';
import {NgClass, NgIf} from "@angular/common";

@Component({
    selector: 'app-dashboard-base-client-card',
    templateUrl: './dashboard-base-client-card.component.html',
    imports: [
        NgClass,
        NgIf
    ],
    standalone: true
})
export class DashboardBaseClientCardComponent {
    @Input() title: string;
    @Input() number: number;
    @Input() subtitle: string;
    @Input() numberClass: string;
    @Input() textClass: string;
    @Input() tooltip: string;
    @Input() icon: string;
    @Input() additionalText: string;
    @Input() additionalNumber: number;
    isPushed: boolean = false;

    @Output() displayListClick = new EventEmitter<void>();

    onDisplayListClick() {
        this.isPushed = true;
        setTimeout(() => (this.isPushed = false), 100);
        this.displayListClick.emit();
    }

    protected readonly onkeydown = onkeydown;
}
