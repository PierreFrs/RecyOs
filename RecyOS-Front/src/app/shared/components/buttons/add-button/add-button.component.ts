import { Component, EventEmitter, Input, Output } from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";

@Component({
    selector: 'app-add-button',
    templateUrl: './add-button.component.html',
    imports: [
        MatButtonModule,
        MatIconModule
    ],
    standalone: true
})
export class AddButtonComponent {
    @Input() disabled = false;
    @Input() icon = 'add';
    @Output() addButtonClick = new EventEmitter<void>();

    onClick() {
        this.addButtonClick.emit();
    }
}
