import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function emailListValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value: string = control.value || '';

        if (!value.trim()) {
            return null;
        }

        if (value.startsWith(';') || value.endsWith(';') || /;;/.test(value)) {
            return {
                emailListInvalid: true,
                reason: 'Invalid start/end or consecutive semicolons',
            };
        }

        const emails = value.split(';');
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        for (let email of emails) {
            if (!emailRegex.test(email) || email.includes(' ')) {
                return {
                    emailListInvalid: true,
                    reason: 'Email format invalid or contains spaces',
                };
            }
        }
        return null;
    };
}
