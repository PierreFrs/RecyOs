import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'formatSiren',
    standalone: true,
})
export class FormatSirenPipe implements PipeTransform {
    transform(value: string, typeIdentifier: string): string {
        if (typeIdentifier === 'SIREN' && value) {
            // Format the SIREN as '000 000 000'
            return value.replace(/(\d{3})(\d{3})(\d{3})/, '$1 $2 $3');
        }
        return value; // Return the original value if it's not a SIREN
    }
}
