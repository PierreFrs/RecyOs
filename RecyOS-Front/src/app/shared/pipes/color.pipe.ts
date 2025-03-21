import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'color',
    standalone: true,
})
export class ColorPipe implements PipeTransform {
    transform(value: any, args?: any): any {
        if (value <= 2) {
            return 'rounded px-2 bg-green-100 text-green-600 font-bold text-xl';
        } else if (value <= 4) {
            return 'rounded px-2 bg-lime-100 text-lime-600 font-bold text-xl';
        } else if (value <= 6) {
            return 'rounded px-2 bg-yellow-100 text-yellow-600 font-bold text-xl';
        } else if (value <= 8) {
            return 'rounded px-2 bg-orange-100 text-orange-600 font-bold text-xl';
        } else if (value <= 9 || value == 'NA') {
            return 'rounded px-2 bg-red-100 text-red-600 font-bold text-xl';
        }
        return '';
    }
}
