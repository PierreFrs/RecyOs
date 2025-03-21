import { Component, forwardRef, Input, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Parameter } from "../settings.types";
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatIconModule } from "@angular/material/icon";

@Component({
    selector: 'app-parameter-control',
    templateUrl: './parameter-control.component.html',
    styleUrls: ['./parameter-control.component.scss'],
    standalone: true,
    imports: [
        CommonModule, 
        MatFormFieldModule, 
        MatInputModule, 
        MatSelectModule, 
        MatDatepickerModule, 
        MatNativeDateModule, 
        MatSlideToggleModule, 
        MatIconModule, 
        FormsModule
    ],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ParameterControlComponent),
            multi: true
        }
    ]
})
export class ParameterControlComponent implements ControlValueAccessor {
    @Input() parameter!: Parameter;
    
    private _value: any = null;
    disabled: boolean = false;
    touched: boolean = false;

    // Fonctions de callback pour le ControlValueAccessor
    onChange = (_: any) => {};
    onTouched = () => {};

    // Getter et Setter pour la valeur
    get value(): any {
        return this._value;
    }

    set value(val: any) {
        if (this._value !== val) {
            this._value = val;
            this.onChange(val);
            this.onTouched();
        }
    }

    // Implémentation de ControlValueAccessor
    writeValue(value: any): void {
        if (value !== undefined && value !== this._value) {
            this._value = value;
            // Mettre à jour la valeur du paramètre
            if (this.parameter) {
                this.parameter.valeur = value;
            }
        }
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    // Méthode pour gérer les changements de valeur
    onValueChange(newValue: any): void {
        if (!this.disabled) {
            this.value = newValue;
            if (this.parameter) {
                this.parameter.valeur = newValue;
            }
        }
    }

    // Méthode pour marquer le contrôle comme touché
    markAsTouched(): void {
        if (!this.touched) {
            this.touched = true;
            this.onTouched();
        }
    }
}