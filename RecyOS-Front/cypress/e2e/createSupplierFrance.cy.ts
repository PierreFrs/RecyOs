import { SupplierDTO } from '../../src/app/models/entities-models/supplier.type';

describe('Create Supplier France', () => {
    let entity: SupplierDTO;

    beforeEach(() => {
        // Login to RecyOs
        cy.loginToRecyOs(Cypress.env('email'), Cypress.env('password'));

        // Navigate to the french suppliers page after retrieving the token
        cy.get('.fuse-vertical-navigation-item-title > .ng-tns-c95-15').click();
        cy.get(
            ':nth-child(1) > .fuse-vertical-navigation-item-wrapper > .mat-mdc-tooltip-trigger > .fuse-vertical-navigation-item-title-wrapper > .fuse-vertical-navigation-item-title > span'
        )
            .should('exist')
            .click();

        cy.url().should('include', '/suppliers/businesses');

        // Fetch the supplier details via API to get its ID for later use, if it exists
        cy.request({
            method: 'GET',
            url: `${Cypress.env(
                'apiUrl'
            )}/etablissement_fournisseur/siret/${Cypress.env('siret')}`,
            failOnStatusCode: false, // Prevents the test from failing on 404
        }).then((response) => {
            if (response.status === 200) {
                // Client exists
                entity = response.body;
            } else if (response.status === 404) {
                // Client does not exist, proceed with test creation steps
                cy.log('Supplier does not exist yet.');
            } else {
                // Handle other unexpected status codes if necessary
                throw new Error(`Unexpected status code: ${response.status}`);
            }
        });
    });

    it('should successfully create a supplier with valid details', () => {
        // Check for errors when adding a supplier with invalid fields
        cy.checkAddMethodErrors(
            'Le SIRET doit être un numéro valide à 14 chiffres.'
        );

        // Check for supplier creation with valid fields
        cy.checkAddMethodValid(Cypress.env('siret'), 'Siret', 'siret');
    });

    it('should retrieve all supplier fields', () => {
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
                    'code-gpi-frn',
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
            { index: 3, fields: ['siren-component'] },
        ];

        tabs.forEach((tab) => {
            cy.get('div[role=tab]').eq(tab.index).click();
            tab.fields.forEach((field) => {
                cy.getByDataCy(field).should('exist');
            });
        });
    });

    it('should update the supplier data', () => {
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');

        cy.updateEntityName(
            '#mat-input-3',
            'suppliers/businesses',
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
        cy.deleteErpCode('etablissement_fournisseur', entity.id, 'mkgt');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-16').should('have.value', '');

        // Add a GPI Code
        cy.get(
            '[data-cy="code-gpi-frn"] > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mdc-icon-button > .mat-mdc-button-touch-target'
        ).click();
        cy.get('#mat-input-19').should('not.be', 'empty');
        cy.wait(1000);

        // Delete the GPI Code
        cy.deleteErpCode('etablissement_fournisseur', entity.id, 'frn_gpi');

        // Reload the page to verify the code has been delete, accessToken
        cy.reload();
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-19').should('have.value', '');
    });

    it('should update Business Units', () => {
        cy.accessEntity('Siret', Cypress.env('siret'), 'siret');
        cy.get('div[role=tab]').eq(1).click();

        // Update the Business Units
        cy.updateBUs();
    });

    it('should delete the supplier', () => {
        // Ensure the client exists and has been retrieved
        if (entity?.id) {
            // Delete the client
            cy.deleteEntity('etablissement_fournisseur', entity.id);
        } else {
            throw new Error('Client ID is not available for deletion');
        }

        // Verify the client has been deleted
        cy.verifyDeletedEntity('Siret', 'siret', Cypress.env('siret'));
    });
});
