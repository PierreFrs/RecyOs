import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'stringToDate',
    standalone: true,
})
export class StringToDatePipe implements PipeTransform {
    transform(value: string): Date {
        if (!value) return null;

        const parts = value.split('/');
        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1;
        const yearParts = parts[2].split(' ');
        const year = parseInt(yearParts[0], 10);

        return new Date(year, month, day);
    }
}
