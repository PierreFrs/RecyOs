import { Directive, Input, OnDestroy, OnInit } from '@angular/core';
import { NgControl } from '@angular/forms';
import {
    debounceTime,
    distinctUntilChanged,
    filter,
    map,
    Subscription,
} from 'rxjs';

@Directive({
    selector: '[formControl][appPhoneMask]',
    standalone: true,
})
export class PhoneMaskDirective implements OnInit, OnDestroy {
    private inputSubscription: Subscription;
    private _isEnabled: boolean = true;

    @Input() set appPhoneMask(isEnabled: boolean) {
        this._isEnabled = isEnabled;
        this.setupControl();
    }
    constructor(public ngControl: NgControl) {}

    ngOnInit() {
        this.setupControl();
    }

    ngOnDestroy() {
        if (this.inputSubscription) {
            this.inputSubscription.unsubscribe();
        }
    }
    private setupControl() {
        if (this.ngControl && this.ngControl.control) {
            this.inputSubscription?.unsubscribe();

            if (!this._isEnabled) {
                return;
            }

            const control = this.ngControl.control;

            this.inputSubscription = control.valueChanges
                .pipe(
                    debounceTime(3000),
                    distinctUntilChanged(),
                    filter(() => this._isEnabled),
                    map((value) => this.normalizeTel(value)),
                )
                .subscribe((newValue) =>
                    control.setValue(newValue, { emitEvent: false }),
                );
        }
    }

    private normalizeTel(tel: string): string | null {
        let countryCode = '';
        if (!tel) {
            return null;
        } else if (tel.startsWith('+')) {
            countryCode = tel.substring(1, 3);
            tel = tel.substring(3);
        } else if (tel.startsWith('00')) {
            countryCode = tel.substring(2, 4);
            tel = tel.substring(4);
        } else {
            countryCode = '33';
            tel = tel.substring(1);
        }
        tel = tel.replace(/[^0-9+]/g, '');
        let formattedPhoneNumber: string;
        if (tel.length === 9 || tel.length >= 10) {
            formattedPhoneNumber = `+${countryCode} ${tel[0]} ${tel.substring(
                1,
                3,
            )} ${tel.substring(3, 5)} ${tel.substring(5, 7)} ${tel.substring(
                7,
                9,
            )}`;
        } else {
            return null;
        }

        return formattedPhoneNumber;
    }
}
