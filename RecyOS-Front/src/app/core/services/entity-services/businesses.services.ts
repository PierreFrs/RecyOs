import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
    BehaviorSubject,
    map,
    Observable,
    of,
    switchMap,
    tap,
    throwError,
} from 'rxjs';
import {
    CategorieClientDto,
    EntrepriseBaseDTO,
    FrenchDTO,
    EtablissementDTOPagination,
    EtablissementFormDto,
    EtablissementFicheDTO,
} from '../../../models/entities-models/french.type';
import { BackendApiService } from '../../../backend.api.service';
import { catchError } from 'rxjs/operators';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../models/business-unit.type';
import { FrenchEntityFilterService } from '../entity-filter-services/french-entity-filter.service';
import { ServiceResult } from '../../../models/service-result.type';
import { SiretUpdateRequest } from '../../../models/Requests/siret-update-request';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class BusinessesServices {
    private readonly _etablissementsClients: BehaviorSubject<
        FrenchDTO[] | null
    > = new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<EtablissementDTOPagination | null> =
        new BehaviorSubject(null);

    private readonly baseUrl = this.apiService.getBaseUrl();
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
        private readonly _frenchEntityFilterService: FrenchEntityFilterService
    ) {}

    /**
     * Get Etablissements Clients
     * @param {number} [page=0] - The current page (default is 0)
     * @param {number} [size=10] - The number of items per page (default is 10)
     * @param {string} [sort='Siret'] - The sorting field (default is 'Siret')
     * @param {'asc' | 'desc' | ''} [order='asc'] - The order of sorting: 'asc' or 'desc' (default is 'asc')
     * @param {string} [search=''] - The search keyword (default is an empty string)
     * @param {number} [filterType=0] - The filter type, based on the user's choice (default is 0):
     *                                   0: Filter by Nom
     *                                   1: Filter by Siret
     *                                   2: Filter by Code MKGT
     *                                   3: Filter by Id Odoo
     * @returns {Observable<{ paginator: EtablissementDTOPagination; items: FrenchDTO[] }>}
     *          An Observable containing the pagination data and the list of EtablissementClientDTO items
     */
    getEtablissementsClients(
        page: number = 0,
        size: number = 10,
        sort: string = 'Siret',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0
    ): Observable<{
        paginator: EtablissementDTOPagination;
        items: FrenchDTO[];
    }> {
        let filterParam =
            this._frenchEntityFilterService.getfilterParam(filterType);

        const params: { [key: string]: string } = {
            PageNumber: '' + page,
            PageSize: '' + size,
            SortBy: '' + sort,
            OrderBy: '' + order,
        };

        if (search) {
            params[filterParam] = search;
        }

        return this._httpClient
            .get<{
                paginator: EtablissementDTOPagination;
                items: FrenchDTO[];
            }>(`${this.baseUrl}/etablissement_client`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._etablissementsClients.next(response.items);
                    this._pagination.next(response.paginator);
                })
            );
    }

    /**
     * Get Etablissement Client by Id
     * @param id - The unique identifier of the Etablissement Client to retrieve
     * @returns An Observable of the EtablissementClientDTO containing the Etablissement Client data
     */
    getEtablissementClientById(id: number): Observable<FrenchDTO> {
        return this._httpClient.get<FrenchDTO>(
            `${this.baseUrl}/etablissement_client/${id}`
        );
    }

    /**
     * Get Etablissement Client by code MKGT
     * @param codeMkgt - The code MKGT of the Etablissement Client to retrieve
     * @returns An Observable of the EtablissementClientDTO containing the Etablissement Client data
     */
    getEtablissementClientByCodeMkgt(codeMkgt: string): Observable<FrenchDTO> {
        return this._httpClient.get<FrenchDTO>(
            `${this.baseUrl}/etablissement_client/code_mkgt/${codeMkgt}`
        );
    }

    /**
     * Get Etablissement Client by SIRET
     * @param siret - The SIRET number of the Etablissement Client to retrieve
     * @returns An Observable of the EtablissementClientDTO containing the Etablissement Client data
     */
    getEtablissementClientBySiret(siret: string): Observable<FrenchDTO> {
        return this._httpClient.get<FrenchDTO>(
            `${this.baseUrl}/etablissement_client/siret/${siret}`
        );
    }

    /**
     * Get the Etablissements Clients stream
     * @returns An Observable stream of EtablissementClientDTO[] or null
     */
    get etablissementsClients$(): Observable<FrenchDTO[] | null> {
        return this._etablissementsClients.asObservable();
    }

    /**
     * Get the pagination stream for Etablissement Clients
     * @returns An Observable stream of EtablissementClientDTOPagination or null
     */
    get pagination$(): Observable<EtablissementDTOPagination> {
        return this._pagination.asObservable();
    }

    getEntrepriseBaseBySiren(siren: string): Observable<EntrepriseBaseDTO> {
        return this._httpClient.get<EntrepriseBaseDTO>(
            `${this.baseUrl}/entreprise_base/siren/${siren}`
        );
    }

    getEntrepriseBaseBySiret(siret: string): Observable<EntrepriseBaseDTO> {
        const siretWithoutHyphens = siret.replace(/-/g, '');
        // On extrait les neuf premiers chiffres pour obtenir le SIREN
        const siren = siretWithoutHyphens.substring(0, 9);
        return this._httpClient.get<EntrepriseBaseDTO>(
            `${this.baseUrl}/entreprise_base/siren/${siren}`
        );
    }

    /**
     * Get Etablissement Fiche by SIRET
     * @param siret - The SIRET number of the establishment to retrieve the fiche for
     * @returns An Observable of the EtablissementFicheDTO containing the establishment fiche data
     */
    getEtablissementFicheBySiret(
        siret: string
    ): Observable<EtablissementFicheDTO> {
        const siretWithoutHyphens = siret.replace(/-/g, '');
        return this._httpClient.get<EtablissementFicheDTO>(
            `${this.baseUrl}/etablissement_fiche/siret/${siretWithoutHyphens}`
        );
    }

    /**
     * Update Etablissement Client
     * @param id - the id of the EtablissementClientDTO to update
     * @param etablissementClientDTO - the EtablissementClientDTO object containing the updated data
     * @returns An Observable of the updated EtablissementClientDTO
     */
    updateEtablissementClient(
        id: number,
        etablissementClientDTO: FrenchDTO
    ): Observable<FrenchDTO> {
        return this._httpClient
            .put<FrenchDTO>(
                `${this.baseUrl}/etablissement_client/${id}`,
                etablissementClientDTO
            )
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                    return throwError(() => error);
                }),
                tap((updatedEtablissementClient) => {
                    // Update the local BehaviorSubject with the new data
                    const currentEtablissementsClients =
                        this._etablissementsClients.getValue();

                    if (currentEtablissementsClients) {
                        const index = currentEtablissementsClients.findIndex(
                            (etablissementClient) =>
                                etablissementClient.id ===
                                updatedEtablissementClient.id
                        );

                        if (index !== -1) {
                            currentEtablissementsClients[index] =
                                updatedEtablissementClient;
                            this._etablissementsClients.next(
                                currentEtablissementsClients
                            );
                        }
                    }
                }),
                switchMap(() => this.getEtablissementClientById(id))
            );
    }

    /**
     * Check if the SIRET number is already used by another Etablissement Client
     * @param siret - The SIRET number to check
     * @returns An Observable of boolean
     *         true if the SIRET number is already used by another Etablissement Client
     *         use the API endpoint /etablissement_client/siret/{siret} to check if the SIRET number is already used by another Etablissement Client
     */
    checkSiretExists(siret: string): Observable<boolean> {
        return this._httpClient
            .get<FrenchDTO>(
                `${this.baseUrl}/etablissement_client/siret/${siret}`
            )
            .pipe(
                map((entity: FrenchDTO) => {
                    return !!entity?.client;
                }),
                catchError((error) => {
                    if (error.status === 404) {
                        return of(false);
                    } else {
                        throw new Error(error);
                    }
                })
            );
    }

    /**
     * Create Etablissement Client form siret number
     * Server will create the Etablissement Client from the siret number and pappers API datas
     * @param siret - The SIRET number of the establishment to create the Etablissement Client for
     * @returns An Observable of the EtablissementClientDTO containing the Etablissement Client data
     */
    createEtablissementClientFromSiret(siret: string): Observable<FrenchDTO> {
        return this._httpClient
            .post<FrenchDTO>(
                `${this.baseUrl}/etablissement_client/add_by_siret/${siret}`,
                {}
            )
            .pipe(
                tap((newEtablissementClient) => {
                    if (newEtablissementClient) {
                        // Récupérer la valeur actuelle de _etablissementsClients
                        const currentEtablissementsClients =
                            this._etablissementsClients.getValue();

                        // Vérifier si la valeur actuelle n'est pas null
                        if (currentEtablissementsClients) {
                            // Ajouter le nouvel établissement client à la liste
                            const updatedEtablissementsClients = [
                                ...currentEtablissementsClients,
                                newEtablissementClient,
                            ];

                            // Mettre à jour le BehaviorSubject _etablissementsClients avec la nouvelle liste
                            this._etablissementsClients.next(
                                updatedEtablissementsClients
                            );
                        }
                    }
                }),
                catchError((error) => {
                    if (error.status === 404) {
                        return of(null);
                    }
                    return throwError(() => error);
                })
            );
    }

    getCategorieClientList(): Observable<CategorieClientDto[]> {
        return this._httpClient.get<CategorieClientDto[]>(
            `${this.baseUrl}/categories-clients`
        );
    }

    createOdooClientFromEtablissementClient(id: number): Observable<FrenchDTO> {
        console.log(
            `Starting createOdooClientFromEtablissementClient for ID: ${id}`
        );

        return this._httpClient
            .post<FrenchDTO>(
                `${this.baseUrl}/partner_odoo/create_french_client/${id}`,
                {}
            )
            .pipe(
                tap((newEtablissementClient) => {
                    console.log(
                        'Odoo client creation successful:',
                        newEtablissementClient
                    );

                    const currentEtablissementsClients =
                        this._etablissementsClients.getValue();
                    console.log(
                        'Current etablissementsClients:',
                        currentEtablissementsClients
                    );

                    if (currentEtablissementsClients) {
                        const index = currentEtablissementsClients.findIndex(
                            (etablissementClient) =>
                                etablissementClient.id ===
                                newEtablissementClient.id
                        );

                        console.log('Found index of updated client:', index);

                        if (index !== -1) {
                            currentEtablissementsClients[index] =
                                newEtablissementClient;
                            console.log(
                                'Updated etablissementsClients list:',
                                currentEtablissementsClients
                            );

                            this._etablissementsClients.next(
                                currentEtablissementsClients
                            );
                        }
                    }
                }),
                catchError((error) => {
                    console.error(
                        'Error during Odoo client creation request:',
                        error
                    );
                    return throwError(() => error);
                })
            );
    }

    createGpiClientFromEtablissementClient(id: number): Observable<FrenchDTO> {
        return this._httpClient
            .post<FrenchDTO>(
                `${this.baseUrl}/gpi_sync/create_french_client/${id}`,
                {}
            )
            .pipe(
                tap((newEtablissementClient) => {
                    // Récupérer la valeur actuelle de _etablissementsClients
                    const currentEtablissementsClients =
                        this._etablissementsClients.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentEtablissementsClients) {
                        // Récupérer l'index de l'établissement client à mettre à jour
                        const index = currentEtablissementsClients.findIndex(
                            (etablissementClient) =>
                                etablissementClient.id ===
                                newEtablissementClient.id
                        );

                        // Vérifier si l'index est différent de -1
                        if (index !== -1) {
                            // Mettre à jour l'établissement client dans la liste
                            currentEtablissementsClients[index] =
                                newEtablissementClient;

                            // Mettre à jour le BehaviorSubject _etablissementsClients avec la nouvelle liste
                            this._etablissementsClients.next(
                                currentEtablissementsClients
                            );
                        }
                    }
                })
            );
    }

    addEtablissementClientToDashdoc(id: number): Observable<FrenchDTO> {
        return this._httpClient
            .post<FrenchDTO>(
                `${this.baseUrl}/create_french_dashdoc_company/${id}`,
                {}
            )
            .pipe(
                tap((newEtablissementClient) => {
                    // Récupérer la valeur actuelle de _etablissementsClients
                    const currentEtablissementsClients =
                        this._etablissementsClients.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentEtablissementsClients) {
                        // Récupérer l'index de l'établissement client à mettre à jour
                        const index = currentEtablissementsClients.findIndex(
                            (etablissementClient) =>
                                etablissementClient.id ===
                                newEtablissementClient.id
                        );

                        // Vérifier si l'index est différent de -1
                        if (index !== -1) {
                            // Mettre à jour l'établissement client dans la liste
                            currentEtablissementsClients[index] =
                                newEtablissementClient;

                            // Mettre à jour le BehaviorSubject _etablissementsClients avec la nouvelle liste
                            this._etablissementsClients.next(
                                currentEtablissementsClients
                            );
                        }
                    }
                })
            );
    }

    createHubspotClientFromEtablissementClient(
        id: number
    ): Observable<FrenchDTO> {
        return this._httpClient.post<FrenchDTO>(
            `${this.baseUrl}/hubspot_company/create_french_company/${id}`,
            {}
        );
    }

    getBusinessUnitsByClientId(
        etablissementClientId: number
    ): Observable<BusinessUnitDto[]> {
        return this._httpClient.get<BusinessUnitDto[]>(
            `${this.baseUrl}/etablissement-client-business-unit/${etablissementClientId}`
        );
    }

    addBusinessUnitToClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this._httpClient.post<EntityBusinessUnitDto>(
            `${this.baseUrl}/etablissement-client-business-unit`,
            etablissementClientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this._httpClient.delete<unknown>(
            `${this.baseUrl}/etablissement-client-business-unit`,
            { body: etablissementClientBusinessUnitDTO }
        );
    }

    convertClientDtoToFormDto(clientDto: FrenchDTO): EtablissementFormDto {
        return {
            id: clientDto.id,
            idShipperDashdoc: clientDto.idShipperDashdoc,
            noBalance: clientDto.noBalance,
            clientGroupe: clientDto.clientGroupe,
            siret: clientDto.siret,
            siren: clientDto.siren,
            client: clientDto.client,
            fournisseur: clientDto.fournisseur,
            identificationForm: {
                addressForm: {
                    nom: clientDto.nom,
                    adresseFacturation1: clientDto.adresseFacturation1,
                    adresseFacturation2: clientDto.adresseFacturation2,
                    adresseFacturation3: clientDto.adresseFacturation3,
                    villeFacturation: clientDto.villeFacturation,
                    codePostalFacturation: clientDto.codePostalFacturation,
                    paysFacturation: clientDto.paysFacturation,
                },
                bankInfosForm: {
                    iban: clientDto.iban,
                    bic: clientDto.bic,
                },
                contactsForm: {
                    contactFacturation: clientDto.contactFacturation,
                    emailFacturation: clientDto.emailFacturation,
                    telephoneFacturation: clientDto.telephoneFacturation,
                    portableFacturation: clientDto.portableFacturation,
                    contactAlternatif: clientDto.contactAlternatif,
                    emailAlternatif: clientDto.emailAlternatif,
                    telephoneAlternatif: clientDto.telephoneAlternatif,
                    portableAlternatif: clientDto.portableAlternatif,
                },
            },
            parametrageForm: {
                billingInfosForm: {
                    conditionReglement: clientDto.conditionReglement,
                    modeReglement: clientDto.modeReglement,
                    delaiReglement: clientDto.delaiReglement,
                    tauxTva: clientDto.tauxTva,
                    compteComptable: clientDto.compteComptable,
                    encoursMax: clientDto.encoursMax,
                    frnConditionReglement: clientDto.frnConditionReglement,
                    frnModeReglement: clientDto.frnModeReglement,
                    frnDelaiReglement: clientDto.frnDelaiReglement,
                    frnTauxTva: clientDto.frnTauxTva,
                    frnCompteComptable: clientDto.frnCompteComptable,
                    frnEncoursMax: clientDto.frnEncoursMax,
                },
                erpCodesForm: {
                    codeMkgt: clientDto.codeMkgt,
                    idOdoo: clientDto.idOdoo,
                    codeGpi: clientDto.codeGpi,
                    frnCodeGpi: clientDto.frnCodeGpi,
                    idDashdoc: clientDto.idDashdoc,
                },
                tva: clientDto.tva,
                clientBloque: clientDto.clientBloque,
                motifBlocage: clientDto.motifBlocage,
                dateBlocage: clientDto.dateBlocage,
                commercialId: clientDto.commercialId,
                categorieId: clientDto.categorieId,
                groupId: clientDto.groupId,
            },
        };
    }

    convertFormDtoToClientDto(
        formDto: EtablissementFormDto,
        originalClientDto: FrenchDTO
    ): FrenchDTO {
        return {
            id: formDto.id,
            nom: formDto.identificationForm.addressForm.nom,
            siret: formDto.siret,
            siren: formDto.siren,
            noBalance: originalClientDto.noBalance,
            clientGroupe: originalClientDto.clientGroupe,
            idShipperDashdoc: originalClientDto.idShipperDashdoc,
            adresseFacturation1:
                formDto.identificationForm.addressForm.adresseFacturation1,
            adresseFacturation2:
                formDto.identificationForm.addressForm.adresseFacturation2,
            adresseFacturation3:
                formDto.identificationForm.addressForm.adresseFacturation3,
            villeFacturation:
                formDto.identificationForm.addressForm.villeFacturation,
            codePostalFacturation:
                formDto.identificationForm.addressForm.codePostalFacturation,
            paysFacturation:
                formDto.identificationForm.addressForm.paysFacturation,
            contactFacturation:
                formDto.identificationForm.contactsForm.contactFacturation,
            emailFacturation:
                formDto.identificationForm.contactsForm.emailFacturation,
            telephoneFacturation:
                formDto.identificationForm.contactsForm.telephoneFacturation,
            portableFacturation:
                formDto.identificationForm.contactsForm.portableFacturation,
            contactAlternatif:
                formDto.identificationForm.contactsForm.contactAlternatif,
            emailAlternatif:
                formDto.identificationForm.contactsForm.emailAlternatif,
            telephoneAlternatif:
                formDto.identificationForm.contactsForm.telephoneAlternatif,
            portableAlternatif:
                formDto.identificationForm.contactsForm.portableAlternatif,
            iban: formDto.identificationForm.bankInfosForm.iban,
            bic: formDto.identificationForm.bankInfosForm.bic,
            frnConditionReglement:
                formDto.parametrageForm.billingInfosForm.frnConditionReglement,
            frnModeReglement:
                formDto.parametrageForm.billingInfosForm.frnModeReglement,
            frnDelaiReglement:
                formDto.parametrageForm.billingInfosForm.frnDelaiReglement,
            frnTauxTva: formDto.parametrageForm.billingInfosForm.frnTauxTva,
            frnCompteComptable:
                formDto.parametrageForm.billingInfosForm.frnCompteComptable,
            frnEncoursMax:
                formDto.parametrageForm.billingInfosForm.frnEncoursMax,
            conditionReglement:
                formDto.parametrageForm.billingInfosForm.conditionReglement,
            modeReglement:
                formDto.parametrageForm.billingInfosForm.modeReglement,
            delaiReglement:
                formDto.parametrageForm.billingInfosForm.delaiReglement,
            tauxTva: formDto.parametrageForm.billingInfosForm.tauxTva,
            compteComptable:
                formDto.parametrageForm.billingInfosForm.compteComptable,
            encoursMax: formDto.parametrageForm.billingInfosForm.encoursMax,
            codeMkgt: formDto.parametrageForm.erpCodesForm.codeMkgt,
            idOdoo: formDto.parametrageForm.erpCodesForm.idOdoo,
            codeGpi: formDto.parametrageForm.erpCodesForm.codeGpi,
            frnCodeGpi: formDto.parametrageForm.erpCodesForm.frnCodeGpi,
            idDashdoc: formDto.parametrageForm.erpCodesForm.idDashdoc,
            tva: formDto.parametrageForm.tva,
            clientBloque: formDto.parametrageForm.clientBloque,
            motifBlocage: formDto.parametrageForm.motifBlocage,
            dateBlocage: formDto.parametrageForm.dateBlocage,
            commercialId: formDto.parametrageForm.commercialId,
            categorieId: formDto.parametrageForm.categorieId,
            groupId: formDto.parametrageForm.groupId,
            client: originalClientDto.client,
            fournisseur: originalClientDto.fournisseur,
            isDeleted: originalClientDto.isDeleted,
            entrepriseBase: originalClientDto.entrepriseBase,
        };
    }

    updateSiret(siret: string, id: number): Observable<ServiceResult> {
        const url = `${this.baseUrl}/etablissement_client/change-siret/${id}`;
        const body: SiretUpdateRequest = { siret: siret };

        return this._httpClient.put<ServiceResult>(url, body).pipe(
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    /**
     * Create Etablissement Client from scratch
     * Server will create an empty Etablissement Client
     * @param siret - The SIRET number of the establishment to create the Etablissement Client
     * @returns An Observable of the EtablissementClientDTO containing the empty Etablissement Client data
     */
    createEmptyEtablissementClient(siret: string): Observable<FrenchDTO> {
        return this._httpClient
            .post<FrenchDTO>(
                `${this.baseUrl}/etablissement_client/create_from_scratch/${siret}`,
                {}
            )
            .pipe(
                tap((newEtablissementClient) => {
                    // Récupérer la valeur actuelle de _etablissementsClients
                    const currentEtablissementsClients =
                        this._etablissementsClients.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentEtablissementsClients) {
                        // Ajouter le nouvel établissement client à la liste
                        const updatedEtablissementsClients = [
                            ...currentEtablissementsClients,
                            newEtablissementClient,
                        ];

                        // Mettre à jour le BehaviorSubject _etablissementsClients avec la nouvelle liste
                        this._etablissementsClients.next(
                            updatedEtablissementsClients
                        );
                    }
                }),
                catchError((error) => {
                    return throwError(() => error);
                })
            );
    }

    getGroup(): Observable<Group> {
        return this._httpClient.get<Group>(`${this.baseUrl}/group`);
    }
}
