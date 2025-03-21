import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'creditAmount',
    standalone: true
})
export class CreditAmountPipe implements PipeTransform {
    transform(value: number): string {
        if (!value) return '0,00 €';
        
        const amount = value;
        
        // Formatter le nombre avec des espaces comme séparateurs de milliers
        const formattedNumber = Math.abs(amount).toLocaleString('fr-FR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
        
        // Si le montant est négatif, on l'entoure de parenthèses
        return amount < 0 ? `(${formattedNumber} €)` : `${formattedNumber} €`;
    }
} 