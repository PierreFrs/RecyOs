import {
    Component,
    EventEmitter,
    forwardRef,
    Input,
    Output,
} from '@angular/core';
import {
    ControlValueAccessor,
    FormControl,
    NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
@Component({
    selector: 'app-input-field',
    templateUrl: './input-field.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputFieldComponent),
            multi: true,
        },
    ],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
    ],
})
export class InputFieldComponent implements ControlValueAccessor {
    @Input() label: string;
    @Input() errorMessages: { [key: string]: string };
    @Input() type: string;
    @Input() control: FormControl;
    @Input() readonly: boolean = false;

    @Output() valueChange = new EventEmitter<string>();

    onChange = (_: any) => {};
    onTouched = () => {};

    writeValue(value: any): void {
        if (value !== undefined && this.control.value !== value) {
            this.control?.setValue(value, { emitEvent: false });
        }
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        isDisabled ? this.disable() : this.enable();
    }

    private disable(): void {
        this.control?.disable({ emitEvent: false });
    }

    private enable(): void {
        this.control?.enable({ emitEvent: false });
    }
}
