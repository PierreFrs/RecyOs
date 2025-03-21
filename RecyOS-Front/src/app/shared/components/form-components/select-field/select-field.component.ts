import {
    Component,
    EventEmitter,
    forwardRef,
    Input,
    Output,
} from '@angular/core';
import {
    ControlValueAccessor,
    NG_VALUE_ACCESSOR,
    FormControl,
    ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'app-select-field',
    templateUrl: './select-field.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SelectFieldComponent),
            multi: true,
        },
    ],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
    ],
})
export class SelectFieldComponent implements ControlValueAccessor {
    @Input() label: string;
    @Input() errorMessage: string;
    @Input() type: string;
    @Input() control: FormControl;
    @Input() options: { value: string | number; label: string }[];

    @Output() valueChange = new EventEmitter<string>();

    onChange = (_: any) => {};
    onTouched = () => {};

    writeValue(value: any): void {
        if (value !== undefined && this.control?.value !== value) {
            this.control.setValue(value, { emitEvent: false });
        }
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.control?.disable({ emitEvent: false });
        } else {
            this.control?.enable({ emitEvent: false });
        }
    }
}
