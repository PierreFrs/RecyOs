import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'formatFrenchPhone',
    standalone: true,
})
export class FormatFrenchPhonePipe implements PipeTransform {
    transform(phoneValue: string | number): string {
        let phoneStr = phoneValue.toString();
        phoneStr = phoneStr.replace(/\D/g, '');

        if (phoneStr.length === 10) {
            return phoneStr.replace(/(\d{2})(?=\d)/g, '$1 ').trim();
        }

        return phoneValue.toString();
    }
}
