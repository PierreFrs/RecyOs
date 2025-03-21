import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SiretService {

  constructor() { }

    isValidSiret(siret: string): boolean {
        if (siret.length !== 14 || !/^\d{14}$/.test(siret)) {
            return false;
        }

        let sum = 0;
        for (let i = 0; i < siret.length; i++) {
            let digit = parseInt(siret[i], 10);
            if (i % 2 === 0) {
                digit *= 2;
                if (digit > 9) {
                    digit -= 9;
                }
            }
            sum += digit;
        }

        return sum % 10 === 0;
    }
}
