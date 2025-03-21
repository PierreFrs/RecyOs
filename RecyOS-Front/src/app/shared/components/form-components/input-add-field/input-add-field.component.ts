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
    FormGroup,
    NG_VALUE_ACCESSOR,
    ReactiveFormsModule,
} from '@angular/forms';
import { RoleAssignmentService } from '../../../../core/services/role-assignment.service';
import { EntityStrategyService } from '../../../../core/strategies/entity-strategy/entity-strategy.service';
import { EntityDto } from '../../../../models/entities-models/entity.type';
import { IStrategyObject } from '../../../../core/strategies/entity-strategy/IStrategyObject';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-input-add-field',
    templateUrl: './input-add-field.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputAddFieldComponent),
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
        MatButtonModule,
    ],
})
export class InputAddFieldComponent implements ControlValueAccessor, OnInit {
    @Input() label: string;
    @Input() errorMessage: string;
    @Input() type: string = 'text';
    @Input() control: FormControl;
    @Input() isReadonly: boolean = false;
    @Input() selectedEntityForm: FormGroup;
    @Input() selectedEntity: EntityDto;
    @Input() canCreateCode: boolean = false;
    strategyObject: IStrategyObject;

    @Output() addButtonClick = new EventEmitter<void>();

    constructor(
        private readonly _roleAssignmentService: RoleAssignmentService,
        private readonly _entityStrategyService: EntityStrategyService
    ) {}

    ngOnInit(): void {
        this.strategyObject =
            this._entityStrategyService.determineStrategyFromEntity(
                this.selectedEntity
            );
    }

    get isAdmin(): boolean {
        return this._roleAssignmentService.isAdmin;
    }

    get isButtonVisible(): boolean {
        return (
            (this.selectedEntityForm?.valid &&
                this.canCreateCode &&
                !this.control.value) ||
            this.isAdmin
        );
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

    onClick(): void {
        this.addButtonClick.emit();
    }
}
