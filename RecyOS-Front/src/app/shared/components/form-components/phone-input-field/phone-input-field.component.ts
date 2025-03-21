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
} from '@angular/forms';
import { EntityStrategyService } from '../../../../core/strategies/entity-strategy/entity-strategy.service';
import { EntityDto } from '../../../../models/entities-models/entity.type';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { PhoneMaskDirective } from '../../../../core/directives/phone-mask.directive';

@Component({
    selector: 'app-phone-input-field',
    templateUrl: './phone-input-field.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PhoneInputFieldComponent),
            multi: true,
        },
    ],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        PhoneMaskDirective,
    ],
})
export class PhoneInputFieldComponent implements OnInit, ControlValueAccessor {
    @Input() label: string;
    @Input() errorMessages: { [key: string]: string };
    @Input() type: string;
    @Input() control: FormControl;
    @Input() selectedEntity: EntityDto;
    entityRegion: string;

    @Output() valueChange = new EventEmitter<string>();

    constructor(
        private readonly _entityStrategyService: EntityStrategyService
    ) {}

    ngOnInit(): void {
        this.entityRegion = this.determineRegion(this.selectedEntity);
        this.control.markAsTouched();
    }
    onChange = (_: any) => {};
    onTouched = () => {};

    determineRegion(entity: EntityDto) {
        if (
            entity.paysFacturation === 'FRANCE' ||
            entity.paysFacturation === 'FR' ||
            entity.paysFacturation === 'France'
        ) {
            return 'france';
        }
        return 'europe';
    }

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
