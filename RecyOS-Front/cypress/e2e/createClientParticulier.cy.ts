import {ClientParticulierDto} from "../../src/app/models/entities-models/particulier.type";

describe('Create Client Particulier', () => {
    let entity: ClientParticulierDto;

    beforeEach(() => {
        // Login to RecyOs
        cy.loginToRecyOs(Cypress.env('email'), Cypress.env('password'));

        // Navigate to the Clients Particuliers page after retrieving the token
        cy.get('[data-cy="clients-list-btn"]').first().click();
        cy.get('.fuse-vertical-navigation-item-children > :nth-child(3) > .fuse-vertical-navigation-item-wrapper > .mat-mdc-tooltip-trigger')
            .should('exist')
            .click();

        cy.url().should('include', '/customers/particuliers');

        // Fetch the client details via API to get its ID for later use, if it exists
        cy.request({
            method: 'GET',
            url: `${Cypress.env('apiUrl')}/client_particulier/${Cypress.env("prenom")}/${Cypress.env("nom")}/${Cypress.env("ville")}`,
            failOnStatusCode: false // Prevents the test from failing on 404
        }).then((response) => {
            if (response.status === 200) {
                // Client exists
                entity = response.body;
            } else if (response.status === 404 || response.status === 204) {
                // Client does not exist, proceed with test creation steps
                cy.log('Client does not exist yet.');
            } else {
                // Handle other unexpected status codes if necessary
                throw new Error(`Unexpected status code: ${response.status}`);
            }
        });
    });

    it('should successfully create a client with valid details', () => {
        const errorMessages =  [
            'Le titre est requis pour un particulier',
            'Le nom est requis pour un particulier',
            'Le prénom est requis pour un particulier',
            'L\'adresse est requise pour un particulier',
            'Le code postal est requis pour un particulier',
            'La ville est requise pour un particulier',
            'Le pays est requis pour un particulier',
            'L\'adresse Email est requise pour un particulier',
            'Le numéro de téléphone est requis pour un particulier',
        ];

        // Check for errors when adding a client with invalid fields
        cy.getByDataCy("add-entity-button").click();
        cy.getByDataCy("create-entity-button").as('addEntityBtn').then(el => {
            expect(el).to.have.attr('disabled');
        })

        // Fill in the form with empty values
        // Open the dropdown by clicking on it
        // Open the dropdown and select an invalid/empty value to trigger validation
        cy.getByDataCy('titre-particulier').find('mat-select').click();
        cy.get('body').click();
        cy.contains(errorMessages[0]).should('exist');

        cy.getByDataCy("nom-particulier").find("input").focus().blur();
        cy.contains(errorMessages[1]).should('exist');

        cy.getByDataCy("prenom-particulier").find("input").focus().blur();
        cy.contains(errorMessages[2]).should('exist');

        cy.getByDataCy("adresse1-particulier").find("input").focus().blur();
        cy.contains(errorMessages[3]).should('exist');

        cy.getByDataCy("code-postal-particulier").find("input").focus().blur();
        cy.contains(errorMessages[4]).should('exist');

        cy.getByDataCy("ville-particulier").find("input").focus().blur();
        cy.contains(errorMessages[5]).should('exist');

        cy.getByDataCy("pays-particulier").find("input").focus().blur();
        cy.contains(errorMessages[6]).should('exist');

        cy.getByDataCy("email-facturation-particulier").find("input").focus().blur();
        cy.contains(errorMessages[7]).should('exist');

        cy.getByDataCy("telephone-facturation-particulier").find("input").focus().blur();
        cy.contains(errorMessages[8]).should('exist');

        // Check for client creation with valid fields
        // Fill in the form with valid values
        cy.getByDataCy("titre-particulier").find("mat-select").click();
        cy.get("mat-option").contains(Cypress.env("titre")).click();
        cy.getByDataCy("nom-particulier").find("input").type(Cypress.env("nom"));
        cy.getByDataCy("prenom-particulier").find("input").type(Cypress.env("prenom"));
        cy.getByDataCy("adresse1-particulier").find("input").type(Cypress.env("adresse1"));
        cy.getByDataCy("adresse2-particulier").find("input").type(Cypress.env("adresse2"));
        cy.getByDataCy("adresse3-particulier").find("input").type(Cypress.env("adresse3"));
        cy.getByDataCy("code-postal-particulier").find("input").type(Cypress.env("codePostal"));
        cy.getByDataCy("ville-particulier").find("input").type(Cypress.env("ville"));
        cy.getByDataCy("pays-particulier").find("input").type(Cypress.env("pays"));
        cy.getByDataCy("email-facturation-particulier").find("input").type(Cypress.env("email-facturation"));
        cy.getByDataCy("telephone-facturation-particulier").find("input").type(Cypress.env("telephone-facturation"));
        cy.getByDataCy("portable-facturation-particulier").find("input").type(Cypress.env("portable-facturation"));
        cy.getByDataCy("contact-alternatif-particulier").find("input").type(Cypress.env("contact-alternatif"));
        cy.getByDataCy("emails-alternatifs-particulier").find("input").type(Cypress.env("email-alternatif"));
        cy.getByDataCy("telephone-alternatif-particulier").find("input").type(Cypress.env("telephone-alternatif"));
        cy.getByDataCy("portable-alternatif-particulier").find("input").type(Cypress.env("portable-alternatif"));

        // Submit the form
        cy.getByDataCy('create-entity-button').click();

        // Verify successful creation by checking for an element that only appears after creation
        cy.getByDataCy("search-filter").click();
        cy.get("mat-option").contains("Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")}`);
        cy.getByDataCy(`client-particulier`).should('contain', `${Cypress.env("complete-name")}`);
    });

    it('should retrieve all client fields', () => {
        // Retrieve the client details
        cy.getByDataCy("search-filter").click();
        cy.get("mat-option").contains("Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")}`);
        cy.wait(1000);
        cy.getByDataCy(`client-particulier`).should('contain', `${Cypress.env("complete-name")}`);
        cy.getByDataCy(`client-particulier`).click();
        cy.getByDataCy("toggle-details-button").click();

        // Verify client details across tabs
        // Check the "identification" tab
        const identificationFields = [
            "titre", "nom", "prenom", "adresseFacturation1", "adresseFacturation2", "adresseFacturation3",
            "codePostalFacturation", "villeFacturation", "paysFacturation",
            "contactAlternatif", "emailFacturation",
            "emailAlternatif", "telephoneFacturation", "telephoneAlternatif",
            "portableFacturation", "portableAlternatif"
        ];
        identificationFields.forEach(field => {
            cy.getByDataCy(field).should('exist');
        });

        // Check other tabs similarly
        const tabs = [
            { index: 1, fields: [
                "conditions-reglement",
                "mode-reglement",
                "delais-reglement",
                "taux-tva",
                "compte-comptable",
                "encours-max",
                "code-mkgt",
                "code-odoo",
                ] },
        ];

        tabs.forEach(tab => {
            cy.get('div[role=tab]').eq(tab.index).click();
            tab.fields.forEach(field => {
                cy.getByDataCy(field).should('exist');
            });
        });
    });

    it('should update the client data', () => {
        cy.getByDataCy("search-filter").click();
        cy.get("mat-option").contains("Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")}`);
        cy.wait(1000);
        cy.getByDataCy(`client-particulier`).should('contain', `${Cypress.env("complete-name")}`);
        cy.getByDataCy(`client-particulier`).click();
        cy.getByDataCy("toggle-details-button").click();

        cy.wait(1000);
        cy.get('#mat-input-4').clear();
        cy.get('#mat-input-4').click().type('Updated Name');
        cy.getByDataCy("update-entity-button").click();
        cy.visit(`${Cypress.env("baseUrl")}/customers/particuliers`);
        cy.getByDataCy("search-filter").click();
        cy.contains("mat-option", `Nom`).click();
        cy.get('input[placeholder="Chercher établissement"]').type('Updated Name');
        cy.wait(1000);
        cy.getByDataCy("toggle-details-button").click();
        cy.wait(1000);
        cy.get('#mat-input-1').should('have.value', 'UPDATED NAME');

        // Reset the name
        cy.get('#mat-input-1').clear();
        cy.get('#mat-input-1').click().type(Cypress.env("nom"));
        cy.getByDataCy("update-entity-button").click();
    });

    it('should create erp codes', () => {
        cy.getByDataCy("search-filter").click();
        cy.get("mat-option").contains("Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")}`);
        cy.wait(1000);
        cy.getByDataCy(`client-particulier`).should('contain', `${Cypress.env("complete-name")}`);
        cy.getByDataCy(`client-particulier`).click();
        cy.getByDataCy("toggle-details-button").click();

        cy.get('div[role=tab]').eq(1).click();

        // Add a MKGT Code
        cy.get('[data-cy="code-mkgt"] > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mdc-icon-button > .mat-mdc-button-touch-target').click();
        cy.get('#mat-input-19').should('not.be', 'empty');
        cy.wait(1000);

        // Delete the MKGT Code
        cy.deleteErpCode('client_particulier', entity.id, 'mkgt');

        // Reload the page to verify the code has been deleted
        cy.reload();
        cy.getByDataCy("search-filter").click();
        cy.get("mat-option").contains("Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")}`);
        cy.wait(1000);
        cy.getByDataCy(`client-particulier`).should('contain', `${Cypress.env("complete-name")}`);
        cy.getByDataCy(`client-particulier`).click();
        cy.getByDataCy("toggle-details-button").click();
        cy.wait(1000);
        cy.get('div[role=tab]').eq(1).click();
        cy.get('#mat-input-16').should('have.value', '');
    });

    it('should delete the client', () => {
        // Ensure the client exists and has been retrieved
        if (entity?.id) {
            // Delete the client
            cy.deleteEntity('client_particulier', entity.id);
        } else {
            throw new Error('Client ID is not available for deletion');
        }

        // Verify the client has been deleted
        cy.getByDataCy("search-filter").click();
        cy.contains('mat-option', "Nom").click();
        cy.get('input[placeholder="Chercher établissement"]').type(`${Cypress.env("nom")} ${Cypress.env("prenom")}`);
        cy.getByDataCy(`client-particulier`).should('not.exist');
    });
});
