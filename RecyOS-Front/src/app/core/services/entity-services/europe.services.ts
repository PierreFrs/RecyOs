import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../../backend.api.service';
import {
    BehaviorSubject,
    catchError,
    map,
    Observable,
    of,
    switchMap,
    tap,
    throwError,
} from 'rxjs';
import {
    EuropeDTO,
    EuropeDTOPagination,
    EuropeFormDto,
} from '../../../models/entities-models/europe.type';
import { CategorieClientDto } from '../../../models/entities-models/french.type';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../models/business-unit.type';
import { EuropeEntityFilterService } from '../entity-filter-services/europe-entity-filter.service';
import { ServiceResult } from '../../../models/service-result.type';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class EuropeServices {
    private readonly _clientsEurope: BehaviorSubject<EuropeDTO[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<EuropeDTOPagination> =
        new BehaviorSubject(null);
    private readonly baseUrl = this.apiService.getBaseUrl();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
        private readonly _europeEntityService: EuropeEntityFilterService
    ) {}

    /**
     * Retrieves a list of clients from Europe.
     *
     * @param {number} page - The page number of the results to retrieve. Default is 0.
     * @param {number} size - The number of results to retrieve per page. Default is 10.
     * @param {string} sort - The field to sort the results by. Default is 'Vat'.
     * @param {'asc' | 'desc' | ''} order - The sort order for the results. Default is 'asc'.
     * @param {string} search - The search query to filter the results by. Default is an empty string.
     * @param {number} filterType - The filter type to use. Default is 0.
     *
     * @return {Observable} - An observable that emits an object with a paginator and items properties.
     *                        The paginator property contains pagination information and the items property contains the client objects.
     */
    getClientsEurope(
        page: number = 0,
        size: number = 10,
        sort: string = 'Vat',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0
    ): Observable<{
        paginator: EuropeDTOPagination;
        items: EuropeDTO[];
    }> {
        let filterParam = this._europeEntityService.getfilterParam(filterType);

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
                paginator: EuropeDTOPagination;
                items: EuropeDTO[];
            }>(`${this.baseUrl}/client_europe`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._pagination.next(response.paginator);
                    this._clientsEurope.next(response.items);
                })
            );
    }

    get clientsEurope$(): Observable<EuropeDTO[] | null> {
        return this._clientsEurope.asObservable();
    }

    get pagination$(): Observable<EuropeDTOPagination> {
        return this._pagination.asObservable();
    }

    getClientEuropeById(id: number): Observable<EuropeDTO> {
        return this._httpClient.get<EuropeDTO>(
            `${this.baseUrl}/client_europe/${id}`
        );
    }

    updateClientEurope(
        id: number,
        clientEurope: EuropeDTO
    ): Observable<EuropeDTO> {
        return this._httpClient
            .put<EuropeDTO>(`${this.baseUrl}/client_europe/${id}`, clientEurope)
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                    return throwError(() => error);
                }),
                tap((updatedClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope = this._clientsEurope.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsEurope) {
                        // recuperer l'index du client europe
                        const index = currentClientsEurope.findIndex(
                            (client) => client.id === updatedClientEurope.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le client europe par le nouveau client europe
                            currentClientsEurope[index] = updatedClientEurope;
                            // Publier la nouvelle valeur
                            this._clientsEurope.next(currentClientsEurope);
                        }
                    }
                }),
                switchMap(() => this.getClientEuropeById(id))
            );
    }

    /**
     * Check if the vat number is already used by another ClientEurope
     * @param {string} vat - The vat number to check
     * @returns {Observable<boolean>} - An Observable containing a boolean
     *                                 true if the vat number is already used by another ClientEurope
     *                                 false if the vat number is not used by another ClientEurope
     */
    checkVat(vat: string): Observable<boolean> {
        return this._httpClient
            .get<EuropeDTO>(`${this.baseUrl}/client_europe/vat/${vat}`)
            .pipe(
                map((entity: EuropeDTO) => {
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
     * Create ClientEurope from a vat code
     * Server will create a ClientEurope from the vat code and Vatlayer API datas
     * @param {string} vat - The vat number to check
     * @returns {Observable<EuropeDTO>} - An Observable containing the created ClientEurope
     */
    createClientEuropeFromVat(vat: string): Observable<EuropeDTO> {
        return this._httpClient
            .post<EuropeDTO>(
                `${this.baseUrl}/client_europe/add_by_vat/${vat}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    if (newClientEurope) {
                        // Récupérer la valeur actuelle de _clientsEurope
                        const currentClientsEurope =
                            this._clientsEurope.getValue();

                        // Vérifier si la valeur actuelle n'est pas null
                        if (currentClientsEurope) {
                            // Ajouter le nouveau client à la liste
                            const updatedClientsEurope = [
                                ...currentClientsEurope,
                                newClientEurope,
                            ];

                            // Publier la nouvelle valeur
                            this._clientsEurope.next(updatedClientsEurope);
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

    createOdooClientFromClientEurope(id: number): Observable<EuropeDTO> {
        return this._httpClient
            .post<EuropeDTO>(
                `${this.baseUrl}/partner_odoo/create_europe_client/${id}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope = this._clientsEurope.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsEurope) {
                        // recuperer l'index du client europe
                        const index = currentClientsEurope.findIndex(
                            (client) => client.id === newClientEurope.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le client europe par le nouveau client europe
                            currentClientsEurope[index] = newClientEurope;
                            // Publier la nouvelle valeur
                            this._clientsEurope.next(currentClientsEurope);
                        }
                    }
                })
            );
    }

    createGpiClientFromClientEurope(id: number): Observable<EuropeDTO> {
        return this._httpClient
            .post<EuropeDTO>(
                `${this.baseUrl}/gpi_sync/create_europe_client/${id}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope = this._clientsEurope.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsEurope) {
                        // recuperer l'index du client europe
                        const index = currentClientsEurope.findIndex(
                            (client) => client.id === newClientEurope.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le client europe par le nouveau client europe
                            currentClientsEurope[index] = newClientEurope;
                            // Publier la nouvelle valeur
                            this._clientsEurope.next(currentClientsEurope);
                        }
                    }
                })
            );
    }

    addClientEuropeToDashdoc(id: number): Observable<EuropeDTO> {
        return this._httpClient
            .post<EuropeDTO>(
                `${this.baseUrl}/create_europe_dashdoc_company/${id}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope = this._clientsEurope.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsEurope) {
                        // recuperer l'index du client europe
                        const index = currentClientsEurope.findIndex(
                            (client) => client.id === newClientEurope.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le client europe par le nouveau client europe
                            currentClientsEurope[index] = newClientEurope;
                            // Publier la nouvelle valeur
                            this._clientsEurope.next(currentClientsEurope);
                        }
                    }
                })
            );
    }

    createHubspotClientFromClientEurope(id: number): Observable<EuropeDTO> {
        return this._httpClient.post<EuropeDTO>(
            `${this.baseUrl}/hubspot_company/create_europe_company/${id}`,
            {}
        );
    }

    getBusinessUnitsByClientId(
        etablissementClientId: number
    ): Observable<BusinessUnitDto[]> {
        return this._httpClient.get<BusinessUnitDto[]>(
            `${this.baseUrl}/client-europe-business-unit/${etablissementClientId}`
        );
    }

    addBusinessUnitToClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this._httpClient.post<EntityBusinessUnitDto>(
            `${this.baseUrl}/client-europe-business-unit`,
            etablissementClientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this._httpClient.delete<unknown>(
            `${this.baseUrl}/client-europe-business-unit`,
            { body: etablissementClientBusinessUnitDTO }
        );
    }

    convertClientDtoToFormDto(clientDto: EuropeDTO): EuropeFormDto {
        return {
            id: clientDto.id,
            vat: clientDto.vat,
            client: clientDto.client,
            fournisseur: clientDto.fournisseur,
            idShipperDashdoc: clientDto.idShipperDashdoc,
            noBalance: clientDto.noBalance,
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
        formDto: EuropeFormDto,
        originalClientDto: EuropeDTO
    ): EuropeDTO {
        return {
            id: formDto.id,
            nom: formDto.identificationForm.addressForm.nom,
            idShipperDashdoc: originalClientDto.idShipperDashdoc,
            noBalance: originalClientDto.noBalance,
            vat: formDto.vat,
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
            codeMkgt: formDto.parametrageForm.erpCodesForm.codeMkgt,
            idOdoo: formDto.parametrageForm.erpCodesForm.idOdoo,
            codeGpi: formDto.parametrageForm.erpCodesForm.codeGpi,
            frnCodeGpi: formDto.parametrageForm.erpCodesForm.frnCodeGpi,
            idDashdoc: formDto.parametrageForm.erpCodesForm.idDashdoc,
            clientBloque: formDto.parametrageForm.clientBloque,
            motifBlocage: formDto.parametrageForm.motifBlocage,
            dateBlocage: formDto.parametrageForm.dateBlocage,
            commercialId: formDto.parametrageForm.commercialId,
            categorieId: formDto.parametrageForm.categorieId,
            groupId: formDto.parametrageForm.groupId,
            client: originalClientDto.client,
            fournisseur: originalClientDto.fournisseur,
        };
    }

    updateVat(vat: string, clientId: number): Observable<ServiceResult> {
        throw new Error('Method not implemented.');
    }

    /**
     * Create Client Europe from scratch
     * Server will create an empty Client Europe
     * @param vat - The VAT number of the establishment to create the Client Europe
     * @returns An Observable of the EtablissementClientDTO containing the empty Client Europe data
     */
    createEmptyClientEurope(vat: string): Observable<EuropeDTO> {
        return this._httpClient
            .post<EuropeDTO>(
                `${this.baseUrl}/client_europe/create_from_scratch/${vat}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope = this._clientsEurope.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsEurope) {
                        // Ajouter le nouveau client européen à la liste
                        const updatedClientsEurope = [
                            ...currentClientsEurope,
                            newClientEurope,
                        ];

                        // Mettre à jour le BehaviorSubject _etablissementsClients avec la nouvelle liste
                        this._clientsEurope.next(updatedClientsEurope);
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
