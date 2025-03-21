import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'trimZero',
    standalone: true,
})
export class TrimZeroPipe implements PipeTransform {
    transform(value: any): any {
        if (typeof value === 'string' && !isNaN(parseInt(value))) {
            return Number(value).toString();
        }
        return value;
    }
}
