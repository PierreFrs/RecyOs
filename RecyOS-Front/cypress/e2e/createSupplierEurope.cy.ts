import { SupplierDTO } from '../../src/app/models/entities-models/supplier.type';

describe('Create Supplier Europe', () => {
    let entity: SupplierDTO;

    beforeEach(() => {
        // Login to RecyOs
        cy.loginToRecyOs(Cypress.env('email'), Cypress.env('password'));

        // Navigate to the european suppliers page after retrieving the token
        cy.get('.fuse-vertical-navigation-item-title > .ng-tns-c95-15').click();
        cy.get(
            '.fuse-vertical-navigation-item-children > :nth-child(2) > .fuse-vertical-navigation-item-wrapper > .mat-mdc-tooltip-trigger'
        )
            .should('exist')
            .click();

        cy.url().should('include', '/suppliers/europe');

        // Fetch the supplier details via API to get its ID for later use, if it exists
        cy.request({
            method: 'GET',
            url: `${Cypress.env('apiUrl')}/fournisseur_europe/vat/${Cypress.env(
                'tva'
            )}`,
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
        cy.checkAddMethodErrors('Le code TVA ne semble pas valide.');

        // Check for supplier creation with valid fields
        cy.checkAddMethodValid(Cypress.env('tva'), 'TVA', 'tva');
    });

    it('should retrieve all supplier fields', () => {
        // Create a balance before checking it
        cy.postBalance('europe', entity.id);

        // Retrieve the client details
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');

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
        ];

        tabs.forEach((tab) => {
            cy.get('div[role=tab]').eq(tab.index).click();
            tab.fields.forEach((field) => {
                cy.getByDataCy(field).should('exist');
            });
        });
    });

    it('should update the supplier data', () => {
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');

        cy.updateEntityName(
            '#mat-input-3',
            'suppliers/europe',
            'TVA',
            'tva',
            'nomClientEurope'
        );
    });

    it('should create erp codes', () => {
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');
        cy.get('div[role=tab]').eq(1).click();

        // Add a MKGT Code
        cy.get(
            '[data-cy="code-mkgt"] > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mdc-icon-button > .mat-mdc-button-touch-target'
        ).click();
        cy.get('#mat-input-19').should('not.be', 'empty');
        cy.wait(1000);

        // Delete the MKGT Code
        cy.deleteErpCode('fournisseur_europe', entity.id, 'mkgt');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');
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
        cy.deleteErpCode('fournisseur_europe', entity.id, 'frn_gpi');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-19').should('have.value', '');
    });

    it('should update Business Units', () => {
        cy.accessEntity('TVA', Cypress.env('tva'), 'tva');
        cy.get('div[role=tab]').eq(1).click();

        // Update the Business Units
        cy.updateBUs();
    });

    it('should delete the supplier', () => {
        // Ensure the client exists and has been retrieved
        if (entity?.id) {
            // Delete the client
            cy.deleteEntity('fournisseur_europe', entity.id);
        } else {
            throw new Error('Client ID is not available for deletion');
        }

        // Verify the client has been deleted
        cy.verifyDeletedEntity('TVA', 'tva', Cypress.env('tva'));
    });
});
