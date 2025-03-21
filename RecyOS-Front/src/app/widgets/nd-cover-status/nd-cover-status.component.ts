import { Component, Input } from '@angular/core';
import { EntrepriseNDCoverDTO } from '../../models/business-nd-cover.type';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-nd-cover-status',
    templateUrl: './nd-cover-status.component.html',
    standalone: true,
    imports: [MatIconModule, MatTooltipModule, CommonModule],
})
export class NdCoverStatusComponent {
    @Input() cover: EntrepriseNDCoverDTO;
}
