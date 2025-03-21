import {
    Component,
    ElementRef,
    EventEmitter,
    Input,
    Output,
    ViewChild,
} from '@angular/core';
import {FuseCardModule} from "../../../../../@fuse/components/card";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {NgIf} from "@angular/common";

@Component({
    selector: 'app-operations-card',
    templateUrl: './operations-card.component.html',
    standalone: true,
    imports: [
        FuseCardModule,
        MatIconModule,
        MatButtonModule,
        NgIf
    ]
})
export class OperationsCardComponent {
    @Input() cardIcon: string;
    @Input() title: string;
    @Input() description: string;
    @Input() actionLabel: string;
    @Input() showFileInput: boolean = false;
    @Input() fileInputClickCallback: () => void;
    @Input() actionType: 'import' | 'export' | 'update';

    @Output() actionTriggered = new EventEmitter<File | void>();
    @Output() handleFileInput = new EventEmitter<File>();
    @ViewChild('fileInput')
    fileInput: ElementRef<HTMLInputElement>;
    onAction() {
        if (
            this.actionType === 'import' &&
            this.showFileInput &&
            this.fileInput
        ) {
            this.fileInput.nativeElement.click();
        } else if (this.actionType === 'import') {
            this.actionTriggered.emit();
        } else if (this.actionType === 'export') {
            this.actionTriggered.emit();
        } else if (this.actionType === 'update') {
            this.actionTriggered.emit();
        }
    }

    onHandleFileInput(event: Event) {
        const inputElement = event.target as HTMLInputElement;
        if (inputElement.files && inputElement.files.length > 0) {
            const file = inputElement.files[0];

            this.handleFileInput.emit(file);
        }

        if (this.fileInput?.nativeElement) {
            this.fileInput.nativeElement.value = '';
        }
    }
}
