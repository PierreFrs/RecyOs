import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class VatService {

  constructor() { }

    isValidVat(vat: string): boolean {
        if (vat && vat.startsWith('FR')) {
            return false;
        }
        const vatRegex = /^[A-Z]{2}([0-9A-Z]{8,12})$/;
        return vatRegex.test(vat);
    }
}
