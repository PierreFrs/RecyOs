import {
    Component,
    EventEmitter,
    forwardRef,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import {
    ControlValueAccessor,
    FormControl,
    NG_VALUE_ACCESSOR,
    ReactiveFormsModule,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import { SharedModule } from '../../../../../../shared/shared.module';
import { PhoneMaskDirective } from '../../../../../../core/directives/phone-mask.directive';
@Component({
    selector: 'app-commercial-phone-input-field',
    templateUrl: './commercial-phone-input-field.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CommercialPhoneInputFieldComponent),
            multi: true,
        },
    ],
    standalone: true,
    imports: [
        MatInputModule,
        ReactiveFormsModule,
        NgIf,
        SharedModule,
        PhoneMaskDirective,
    ],
})
export class CommercialPhoneInputFieldComponent
    implements OnInit, ControlValueAccessor
{
    @Input() label: string;
    @Input() errorMessages: { [key: string]: string };
    @Input() type: string;
    @Input() control: FormControl;
    @Input() clientContext: string;
    @Input() readonly: boolean = false;

    @Output() valueChange = new EventEmitter<string>();

    constructor() {}

    ngOnInit(): void {
        this.control.markAsTouched();
    }
    onChange = (_: any) => {};
    onTouched = () => {};

    writeValue(value: any): void {
        if (value !== undefined && this.control.value !== value) {
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
            this.control.disable({ emitEvent: false });
        } else {
            this.control.enable({ emitEvent: false });
        }
    }
}
