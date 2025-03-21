// ***********************************************
// This example namespace declaration will help
// with Intellisense and code completion in your
// IDE or Text Editor.
// ***********************************************

declare namespace Cypress {
    interface Chainable<Subject = any> {
        /**
         * Custom command to login by filling out the login form and submitting it
         * @param email - The user's email
         * @param password - The user's password
         */
        loginToRecyOs(email: string, password: string): Chainable<void>;

        /**
         * Custom command to get elements by data-cy attribute
         * @param selector - The value of the data-cy attribute
         */
        getByDataCy(selector: string): Chainable<JQuery<HTMLElement>>;

        /**
         * Custom command to check errors when adding a new entity
         * @param errorMessage - The error message to check
         */
        checkAddMethodErrors(errorMessage: string): Chainable<void>;

        /**
         * Custom command to check valid creation of a new entity
         * @param envVar - The environment variable to use
         * @param option - The option to select in the search filter
         * @param administrativeIdentifier - The administrative identifier of the entity
         */
        checkAddMethodValid(
            envVar: string,
            option: string,
            administrativeIdentifier: string
        ): Chainable<void>;

        /**
         * Custom command to post a new balance
         * @param entity - The entity to post the balance to
         * @param id - The ID of the entity to post the balance to
         */
        postBalance(entity: string, id: number): Chainable<void>;

        /**
         * Custom command to access an entity
         * @param entityOption - The option to select in the search filter
         * @param envVariable - The environment variable to use
         * @param administrativeIdentifier - The administrative identifier of the entity
         */
        accessEntity(
            entityOption: string,
            envVariable: string,
            administrativeIdentifier: string
        ): Chainable<void>;

        /**
         * Custom command to update the name of an entity
         * @param input - The input to update
         * @param urlExtension - The extension to add to the URL
         * @param entityOption - The option to select in the search filter
         * @param entityType - The type of entity to access
         * @param envVariable - The environment variable to use
         */
        updateEntityName(
            input: string,
            urlExtension: string,
            entityOption: string,
            entityType: string,
            envVariable: string
        ): Chainable<void>;

        /**
         * Custom command to delete an ERP code
         * @param entityType - The type of entity to delete the ERP code for
         * @param entityId - The ID of the entity to delete the ERP code for
         * @param erpType - The type of ERP code to delete
         */
        deleteErpCode(
            entityType: string,
            entityId: number,
            erpType: string
        ): Chainable<void>;

        /**
         * Custom command to update the Business Units
         */
        updateBUs(): Chainable<void>;

        /**
         * Custom command to upload a file to an entity
         */
        entityFilesUpload(): Chainable<void>;

        /**
         * Custom command to update the factor dual box
         * @param urlExtension - The extension to add to the URL
         * @param entityOption - The option to select in the search filter
         * @param entityAdministrativeIdentifier - The administrative identifier of the entity
         * @param entityType - The type of entity to access
         */
        updateFactorDualBox(
            urlExtension: string,
            entityOption: string,
            entityAdministrativeIdentifier: string,
            entityType: string
        ): Chainable<void>;

        /**
         * Custom command to post a new balance
         * @param entityRegion - The region of the entity to post the balance to
         * @param id - The ID of the entity to post the balance to
         * @param entityOption - The option to select in the search filter
         * @param administrativeIdentifierType - The type of administrative identifier to use
         * @param administrativeIdentifier - The administrative identifier of the entity
         */
        testBalance(
            entityRegion: string,
            id: number,
            entityOption: string,
            administrativeIdentifierType: string,
            administrativeIdentifier: string
        ): Chainable<void>;

        /**
         * Custom command to delete an entity
         * @param entityType - The type of entity to delete
         * @param entityId - The ID of the entity to delete
         */
        deleteEntity(entityType: string, entityId: number): Chainable<void>;

        /**
         * Custom command to verify that an entity has been deleted
         * @param entityOption - The option to select in the search filter
         * @param envVar - The environment variable to use
         * @param administrativeIdentifier - The administrative identifier of the entity
         */
        verifyDeletedEntity(
            entityOption: string,
            envVar: string,
            administrativeIdentifier: string
        ): Chainable<void>;
    }
}

//
// function customCommand(param: any): void {
//   console.warn(param);
// }
//
// NOTE: You can use it like so:
// Cypress.Commands.add('customCommand', customCommand);
//
// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

Cypress.Commands.add('loginToRecyOs', (email: string, password: string) => {
    cy.visit('/sign-in');
    cy.get('input[id="email"]').type(email);
    cy.get('input[id="password"]').type(password);
    cy.contains('button', 'Connexion').click();
});

Cypress.Commands.add('getByDataCy', (selector: string) => {
    return cy.get(`[data-cy=${selector}]`);
});

Cypress.Commands.add('checkAddMethodErrors', (errorMessage: string) => {
    cy.getByDataCy('add-entity-button').click();
    cy.getByDataCy('create-entity-button')
        .as('addEntityBtn')
        .then((el) => {
            expect(el).to.have.attr('disabled');
        });

    // Fill in the form with empty values
    cy.getByDataCy('new-entity-input').as('newEntityInput').focus().blur();
    cy.contains(
        'Le numéro est requis pour créer un nouvel établissement.'
    ).should('exist');
    cy.contains(errorMessage).should('exist');

    // Fill in the form with invalid values
    cy.get('@newEntityInput').type('invalid');
    cy.contains(errorMessage).should('exist');
    cy.get('@newEntityInput').clear();
});

Cypress.Commands.add(
    'checkAddMethodValid',
    (envVar: string, option: string, administrativeIdentifier: string) => {
        // Fill in the form with valid values
        cy.get('@newEntityInput').type(envVar);
        cy.get('@addEntityBtn').click();

        // Verify successful creation by checking for an element that only appears after creation
        cy.getByDataCy('search-filter').click();
        cy.get('mat-option').contains(option).click();
        cy.get('input[placeholder="Chercher établissement"]').type(envVar);
        cy.getByDataCy(`client-${administrativeIdentifier}`).should(
            'contain',
            envVar
        );
    }
);

Cypress.Commands.add('postBalance', (entity: string, id: number) => {
    cy.request({
        method: 'POST',
        url: `${Cypress.env('apiUrl')}/balances-${entity}/`,
        body: {
            clientId: id,
            societeId: 1,
            montant: 1234,
        },
    }).then((response) => {
        expect(response.status).to.eq(200);
    });
});

Cypress.Commands.add(
    'accessEntity',
    (
        entityOption: string,
        envVariable: string,
        administrativeIdentifier: string
    ) => {
        // Searches for the client to inspect
        cy.getByDataCy('search-filter').click();
        cy.contains('mat-option', entityOption).click();
        cy.get('input[placeholder="Chercher établissement"]').type(envVariable);
        cy.wait(1200);
        cy.getByDataCy(`client-${administrativeIdentifier}`).should(
            'contain',
            envVariable
        );

        // Clicks on the client to inspect
        cy.getByDataCy('toggle-details-button').click();
    }
);

Cypress.Commands.add(
    'updateEntityName',
    (
        input: string,
        urlExtension: string,
        entityOption: string,
        entityIdentifier: string,
        entityNameEnvVariable: string
    ) => {
        // Update the client data
        cy.wait(1000);
        cy.get(`${input}`).clear();
        cy.get(`${input}`).click().type('Updated Name');
        cy.getByDataCy('update-entity-button').click();
        cy.visit(`${Cypress.env('baseUrl')}/${urlExtension}`);
        cy.getByDataCy('search-filter').click();
        cy.contains('mat-option', `${entityOption}`).click();
        cy.get('input[placeholder="Chercher établissement"]').type(
            Cypress.env(entityIdentifier)
        );
        cy.wait(1000);
        cy.getByDataCy('toggle-details-button').click();
        cy.wait(1000);
        cy.get('#mat-input-1').should('have.value', 'Updated Name');

        // Reset the name
        cy.get('#mat-input-1').clear();
        cy.get('#mat-input-1').click().type(Cypress.env(entityNameEnvVariable));
        cy.getByDataCy('update-entity-button').click();
    }
);

Cypress.Commands.add(
    'deleteErpCode',
    (entityType: string, entityId: number, erpType: string) => {
        cy.request({
            method: 'PUT',
            url: `${Cypress.env(
                'apiUrl'
            )}/${entityType}/code-erp/${entityId}/${erpType}/`,
        }).then((response) => {
            expect(response.status).to.eq(200);
        });
    }
);

Cypress.Commands.add('updateBUs', () => {
    // Update the Business Units
    cy.getByDataCy('add-bu-btn').click();
    cy.get('#mat-mdc-checkbox-2-input').click();
    cy.get('#mat-mdc-checkbox-3-input').click();

    // Click outside the panel to simulate closing it
    cy.get('body').click(0, 0);

    // Step 3: Verify the Business Units have been updated
    cy.contains('Recynov').should('exist');
    cy.contains('Transport').should('exist');

    // Remove the Business Units
    cy.getByDataCy('edit-bu-btn').click();
    cy.get('#mat-mdc-checkbox-7-input').click();
    cy.get('#mat-mdc-checkbox-8-input').click();
    cy.get('body').click(0, 0);

    // Verify the Business Units have been removed
    cy.contains('Recynov').should('not.exist');
    cy.contains('Valotrans').should('not.exist');
});

Cypress.Commands.add(
    'updateFactorDualBox',
    (
        urlExtension: string,
        entityOption: string,
        entityAdministrativeIdentifier: string,
        entityType: string
    ) => {
        // Update the factor dual box
        cy.get('[data-cy="available-list"] > :nth-child(1)').click();
        cy.getByDataCy('move-selected-right').click();
        cy.getByDataCy('update-entity-button').click();

        // Verify the factor dual box has been updated
        cy.visit(`${Cypress.env('baseUrl')}/customers/${urlExtension}`);
        cy.accessEntity(
            `${entityOption}`,
            Cypress.env(entityAdministrativeIdentifier),
            `${entityAdministrativeIdentifier}`
        );
        cy.get('div[role=tab]').eq(2).click();
        cy.get('[data-cy="assigned-list"] > :nth-child(1)').should('exist');

        // Move the element back to the available list
        cy.get('[data-cy="assigned-list"] > :nth-child(1)').click();
        cy.getByDataCy('move-selected-left').click();
        cy.getByDataCy('update-entity-button').click();

        // Verify the element has been moved back to the available list
        cy.visit(`${Cypress.env('baseUrl')}/customers/${urlExtension}`);
        cy.getByDataCy('search-filter').click();
        cy.contains('mat-option', `${entityOption}`).click();
        cy.intercept('GET', `**/${entityType}?*`).as('searchRequest');
        cy.get('input[placeholder="Chercher établissement"]').type(
            Cypress.env(entityAdministrativeIdentifier)
        );
        cy.wait('@searchRequest').its('response.statusCode').should('eq', 200);
        cy.getByDataCy('toggle-details-button').click();
        cy.get('div[role=tab]').eq(2).click();
        cy.get('[data-cy="assigned-list"] > :nth-child(1)').should('not.exist');
    }
);

Cypress.Commands.add('entityFilesUpload', () => {
    // Upload a file
    cy.getByDataCy('add-pdf-button').click();
    cy.getByDataCy('type-select-dropdown').click();
    cy.contains('mat-option', 'KBIS').click();
    cy.getByDataCy('upload-pdf-button').should('exist').click();
    cy.get('input[type="file"]').selectFile('cypress/fixtures/test_pdf.pdf', {
        force: true,
    });
    cy.getByDataCy('confirm-upload').should('not.be.disabled').click();

    // Verify the file has been uploaded and capture the document ID
    cy.get('.mat-mdc-selection-list > :nth-child(3)').click();
    cy.get('.mat-mdc-list > :nth-child(1) > .mat-mdc-list-item').click();
    cy.getByDataCy('file-preview-col').contains('test_pdf.pdf');

    // Assuming the document ID is stored in a data attribute called "data-doc-id"
    cy.getByDataCy('download-pdf-button').click();

    cy.readFile('cypress/downloads/test_pdf.pdf');

    // Delete the file
    cy.getByDataCy('delete-pdf-button').should('exist').click({ force: true });
    cy.getByDataCy('confirm-delete').should('exist').click();
    cy.getByDataCy('pdf-viewer').should('not.exist');
});

Cypress.Commands.add(
    'testBalance',
    (
        entityRegion: string,
        id: number,
        entityOption: string,
        administrativeIdentifierType: string,
        administrativeIdentifier: string
    ) => {
        let balanceId: number;
        cy.request({
            method: 'POST',
            url: `${Cypress.env('apiUrl')}/balances-${entityRegion}/`,
            body: {
                clientId: id,
                societeId: 1,
                montant: 1234,
            },
        }).then((response) => {
            expect(response.status).to.eq(200);
            balanceId = response.body.id;

            // Now that balanceId is set, perform the rest of the operations

            // Search for the establishment by administrative identifier and verify the balance has been added
            cy.accessEntity(
                `${entityOption}`,
                administrativeIdentifier,
                `${administrativeIdentifierType}`
            );
            cy.get('div[role=tab]').eq(3).click();
            cy.contains('1 234,00 €');

            // Delete the balance
            cy.request({
                method: 'DELETE',
                url: `${Cypress.env(
                    'apiUrl'
                )}/balances-${entityRegion}/${balanceId}`,
            }).then((deleteResponse) => {
                expect(deleteResponse.status).to.eq(200);

                // Verify the balance has been deleted
                cy.getByDataCy('toggle-details-button').click();
                cy.getByDataCy('toggle-details-button').click();
                cy.get('div[role=tab]').eq(3).click();
                cy.should('not.contain', '1234');
            });
        });
    }
);

Cypress.Commands.add('deleteEntity', (entityType: string, entityId: number) => {
    cy.request({
        method: 'DELETE',
        url: `${Cypress.env('apiUrl')}/${entityType}/${entityId}`,
    }).then((response) => {
        expect(response.status).to.eq(200);
    });
});

Cypress.Commands.add(
    'verifyDeletedEntity',
    (
        entityOption: string,
        administrativeIdentifier: string,
        envVar: string
    ) => {
        cy.getByDataCy('search-filter').click();
        cy.contains('mat-option', entityOption).click();
        cy.get('input[placeholder="Chercher établissement"]').type(envVar);
        cy.getByDataCy(`client-${administrativeIdentifier}`).should(
            'not.exist'
        );
    }
);
