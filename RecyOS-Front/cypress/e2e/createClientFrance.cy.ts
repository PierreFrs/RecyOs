import { FrenchDTO } from '../../src/app/models/entities-models/french.type';

describe('Create Etablissement Client', () => {
    let entity: FrenchDTO;

    beforeEach(() => {
        // Login to RecyOs
        cy.loginToRecyOs(Cypress.env('email'), Cypress.env('password'));

        // Navigate to the Clients page after retrieving the token
        cy.get('[data-cy="clients-list-btn"]').first().click();

        // Fetch the client details via API to get its ID for later use, if it exists
        cy.request({
            method: 'GET',
            url: `${Cypress.env(
                'apiUrl'
            )}/etablissement_client/siret/${Cypress.env('siret')}`,
            failOnStatusCode: false, // Prevents the test from failing on 404
        }).then((response) => {
            if (response.status === 200) {
                // Client exists
                entity = response.body;
            } else if (response.status === 404) {
                // Client does not exist, proceed with test creation steps
                cy.log('Client does not exist yet.');
            } else {
                // Handle other unexpected status codes if necessary
                throw new Error(`Unexpected status code: ${response.status}`);
            }
        });
    });

    it('should successfully create a client with valid details', () => {
        // Check for errors when adding a client with invalid fields
        cy.checkAddMethodErrors(
            'Le SIRET doit être un numéro valide à 14 chiffres.'
        );

        // Check for client creation with valid fields
        cy.checkAddMethodValid(Cypress.env('siret'), 'Siret', 'siret');
    });

    it('should retrieve all client fields', () => {
        // Create a balance before checking it
        cy.postBalance('france', entity.id);

        // Retrieve the client details
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');

        // Verify client details across tabs
        // Check the "identification" tab
        const identificationFields = [
            'nom',
            'adresseFacturation1',
            'adresseFacturation2',
            'adresseFacturation3',
            'codePostalFacturation',
            'villeFacturation',
            'paysFacturation',
            'contactFacturation',
            'contactAlternatif',
            'emailFacturation',
            'emailAlternatif',
            'telephoneFacturation',
            'telephoneAlternatif',
            'portableFacturation',
            'portableAlternatif',
        ];
        identificationFields.forEach((field) => {
            cy.getByDataCy(field).should('exist');
        });

        // Check other tabs similarly
        const tabs = [
            {
                index: 1,
                fields: [
                    'conditions-reglement',
                    'mode-reglement',
                    'delais-reglement',
                    'taux-tva',
                    'compte-comptable',
                    'encours-max',
                    'code-mkgt',
                    'code-odoo',
                    'code-gpi',
                    'commercial',
                    'groupe',
                    'add-bu-btn',
                ],
            },
            {
                index: 2,
                fields: [
                    'files-types-col',
                    'files-list-col',
                    'file-preview-col',
                ],
            },
            { index: 3, fields: ['balances-table'] },
            { index: 4, fields: ['couverture-component'] },
            { index: 5, fields: ['siren-component'] },
        ];

        tabs.forEach((tab) => {
            cy.get('div[role=tab]').eq(tab.index).click();
            tab.fields.forEach((field) => {
                cy.getByDataCy(field).should('exist');
            });
        });
    });

    it('should update the client data', () => {
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');

        cy.updateEntityName(
            '#mat-input-3',
            'customers/businesses',
            'Siret',
            'siret',
            'nomEtablissementClient'
        );
    });

    it('should create erp codes', () => {
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.get('div[role=tab]').eq(1).click();

        // Add a MKGT Code
        cy.get(
            '[data-cy="code-mkgt"] > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mdc-icon-button > .mat-mdc-button-touch-target'
        ).click();
        cy.get('#mat-input-19').should('not.be', 'empty');
        cy.wait(1000);

        // Delete the MKGT Code
        cy.deleteErpCode('etablissement_client', entity.id, 'mkgt');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-16').should('have.value', '');

        // Add a GPI Code
        cy.get(
            '[data-cy="code-gpi"] > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mdc-icon-button > .mat-mdc-button-touch-target'
        ).click();
        cy.get('#mat-input-19').should('not.be', 'empty');
        cy.wait(1000);

        // Delete the GPI Code
        cy.deleteErpCode('etablissement_client', entity.id, 'gpi');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-20').should('have.value', '');
    });

    it('should update Business Units', () => {
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.get('div[role=tab]').eq(1).click();

        // Update the Business Units
        cy.updateBUs();
    });

    // it('should update factor dual box', () => {
    //     cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
    //     cy.get('div[role=tab]').eq(2).click();

    //     // Test the factor dual box
    //     cy.updateFactorDualBox(
    //         'businesses',
    //         'Siret',
    //         'siret',
    //         'etablissement_client'
    //     );
    // });

    it('should upload a file, download it back, and delete it', () => {
        // Search for the establishment by SIRET
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.get('div[role=tab]').eq(2).click();

        // Verify file upload
        cy.entityFilesUpload();
    });

    it('should add a new balance field, retrieve it, and delete it', () => {
        // Add a new balance
        if (entity?.id) {
            cy.testBalance(
                'france',
                entity.id,
                'Siret',
                'siret',
                Cypress.env('siret')
            );
        } else {
            throw new Error('Client ID is not available for deletion');
        }
    });

    it('should delete the client', () => {
        // Ensure the client exists and has been retrieved
        if (entity?.id) {
            // Delete the client
            cy.deleteEntity('etablissement_client', entity.id);
        } else {
            throw new Error('Client ID is not available for deletion');
        }

        // Verify the client has been deleted
        cy.verifyDeletedEntity('Siret', 'siret', Cypress.env('siret'));
    });
});
