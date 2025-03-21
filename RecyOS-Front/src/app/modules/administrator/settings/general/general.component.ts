import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Parameter } from '../settings.types';

@Component({
    selector: 'app-general',
    templateUrl: './general.component.html',
    styleUrls: ['./general.component.scss'],
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class GeneralComponent implements OnInit {
    
    generalForm: FormGroup;
    parameters: Parameter[] = [
        {
          id: 1,
          module: 'general',
          nom: 'pappers-api-key',
          valeur: '3385492b9bada8f36bf1947cd21c349be96e19cd3024422b',
          type: 'String',
          controlType: 'text',
          createDate: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          createdBy: 'admin',
          updatedBy: 'admin',
          isDeleted: false,
          placeholder: 'Entrez votre clé API Pappers',
          prefixIcon: 'heroicons_solid:key',
          label: 'Clé API Pappers'
        },
        {
          id: 2,
          module: 'general',
          nom: 'vatlayer-api-key',
          valeur: 'af0107065a5848c0e93c2acba24c95c9',
          type: 'String',
          controlType: 'text',
          createDate: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          createdBy: 'admin',
          updatedBy: 'admin',
          isDeleted: false,
          placeholder: 'Entrez votre clé API VATLayer',
          prefixIcon: 'heroicons_solid:key',
          label: 'Clé API VATLayer'
        },
        {
          id: 3,
          module: 'general',
          nom: 'MkgtDatabase',
          valeur: 'Server=tcp:127.0.0.1,1433;TrustServerCertificate=true;Database=Mkgt;User=sa;Password=<YourStrong@Passw0rd>',
          type: 'String',
          controlType: 'text',
          createDate: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          createdBy: 'admin',
          updatedBy: 'admin',
          isDeleted: false,
          placeholder: 'Entrez votre chaine de connexion à la base de données MKGT',
          label: 'Chaine de connexion à la base de données MKGT',
          prefixIcon: 'heroicons_solid:database',
        }
      ];
    

    constructor(private readonly _formBuilder: FormBuilder) {
        // Initialisation du formulaire
        this.generalForm = this._formBuilder.group({
            // Ajoutez ici les contrôles de formulaire nécessaires
            parameter1: [''], // Exemple de champ
            parameter2: [''], // Exemple de champ
        });
    }

    ngOnInit(): void {
        // Logique d'initialisation si nécessaire
    }

    onSubmit(): void {
        if (this.generalForm.valid) {
            // Traitez les données du formulaire ici
            console.log(this.generalForm.value);
        }
    }
}
