import { Component, Input, OnInit, Self, Optional, SimpleChanges, OnChanges } from "@angular/core";
import { ControlValueAccessor, NgControl } from "@angular/forms";
import { ParameterDto } from "../settings.types";
import { SettingsService } from "../settings.service";
import { BehaviorSubject, Observable } from "rxjs";


@Component({
    selector: 'app-db-parameter-controle',
    templateUrl: './db-parameter-controle.component.html',
    styleUrls: ['./db-parameter-controle.component.scss'],
})
export class DbParameterControleComponent implements OnInit, ControlValueAccessor, OnChanges {
    @Input() module: string = '';
    @Input() name: string = '';
    
    private readonly _parameter = new BehaviorSubject<ParameterDto | null>(null);
    parameter$: Observable<ParameterDto | null> = this._parameter.asObservable();
    
    private _value: any = null;
    disabled: boolean = false;
    touched: boolean = false;

    onChange = (value: any) => {};
    onTouched = () => {};

    constructor(
        private readonly settingsService: SettingsService,
        @Self() @Optional() public ngControl: NgControl
    ) {
        if (this.ngControl) {
            this.ngControl.valueAccessor = this;
        }
    }

    ngOnInit(): void {
        if (this.module && this.name) {
            this.loadParameter();
        }
    }

    ngOnChanges(changes: SimpleChanges): void {
        if ((changes['module'] || changes['name']) && this.module && this.name) {
            this.loadParameter();
        }
    }

    private loadParameter(): void {
        this.settingsService.getParameterByModuleAndName(this.module, this.name)
            .subscribe({
                next: (parameter: ParameterDto) => {
                    this._parameter.next(parameter);
                    // Mettre à jour la valeur si elle existe
                    if (parameter.valeur !== undefined) {
                        this.setValue(parameter.valeur);
                    }
                },
                error: (error) => {
                    console.error('Erreur lors du chargement du paramètre:', error);
                }
            });
    }

    get value(): any {
        return this._value;
    }

    set value(val: any) {
        this._value = val;
        this.onChange(val);
        this.onTouched();
    }

    writeValue(value: any): void {
        if (value !== undefined) {
            this._value = value;
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

    // Méthodes utilitaires
    setValue(value: any): void {
        if (!this.disabled) {
            this._value = value;
            this.onChange(value);
            this.markAsTouched();
        }
    }

    markAsTouched(): void {
        if (!this.touched) {
            this.touched = true;
            this.onTouched();
        }
    }

    // Méthode pour sauvegarder les modifications
    saveChanges(): void {
        const currentParameter = this._parameter.getValue();
        if (currentParameter) {
            currentParameter.valeur = this._value;
            this.settingsService.updateParameter(currentParameter.id, currentParameter).subscribe({
                next: (updatedParameter) => {
                    this._parameter.next(updatedParameter);
                },
                error: (error) => {
                    console.error('Erreur lors de la mise à jour du paramètre:', error);
                }
            });
        }
    }
}