import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../../backend.api.service';
import {
    BehaviorSubject,
    catchError,
    map,
    Observable,
    of,
    tap,
    throwError,
} from 'rxjs';
import {
    EuropeDTO,
    EuropeDTOPagination,
    FournisseurEuropeFormDto,
} from '../../../models/entities-models/europe.type';
import { EuropeEntityFilterService } from '../entity-filter-services/europe-entity-filter.service';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../models/business-unit.type';
import { Group } from '../../../models/group.type';

@Injectable({
    providedIn: 'root',
})
export class EuropeSuppliersService {
    private readonly _europeSuppliers: BehaviorSubject<EuropeDTO[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<EuropeDTOPagination> =
        new BehaviorSubject(null);
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
        private readonly _europeEntityService: EuropeEntityFilterService
    ) {}

    /**
     * Retrieves suppliers from Europe based on the provided parameters.
     *
     * @param {number} page - The page number to retrieve. Default value is 0.
     * @param {number} size - The number of items per page. Default value is 10.
     * @param {string} sort - The field to sort the results by. Default value is 'Vat'.
     * @param {'asc' | 'desc' | ''} order - The order to sort the results in. Default value is 'asc'.
     * @param {string} search - The search term to filter the results by. Default value is an empty string.
     * @param {number} filterType - The type of filter to apply. Default value is 0.
     *
     * @return {Observable<{paginator: EuropeDTOPagination, items: EuropeDTO[]}>} - An observable containing the paginator metadata and the list of supplier items.
     */
    getSuppliersEurope(
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
        const baseUrl = this.apiService.getBaseUrl();
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
            }>(`${baseUrl}/fournisseur_europe`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._pagination.next(response.paginator);
                    this._europeSuppliers.next(response.items);
                })
            );
    }

    get suppliersEurope$(): Observable<EuropeDTO[] | null> {
        return this._europeSuppliers.asObservable();
    }

    get pagination$(): Observable<EuropeDTOPagination> {
        return this._pagination.asObservable();
    }

    getSupplierEuropeById(id: number): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<EuropeDTO>(
            `${baseUrl}/fournisseur_europe/${id}`
        );
    }

    /**
     * Updates the supplier in Europe with the given ID.
     *
     * @param {number} id - The ID of the supplier to update.
     * @param {EuropeDTO} supplier - The updated supplier data.
     * @returns {Observable<EuropeDTO>} - An Observable that emits the updated supplier data.
     *
     * @throws {Error} - If the HTTP request fails with a status code of 480.
     */
    updateSupplierEurope(
        id: number,
        supplier: EuropeDTO
    ): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .put<EuropeDTO>(`${baseUrl}/fournisseur_europe/${id}`, supplier)
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                }),
                tap((updatedSupplierEurope) => {
                    // Get the current list of suppliers
                    const suppliersEurope = this._europeSuppliers.getValue();

                    // If the list is not null
                    if (suppliersEurope) {
                        // Find the index of the updated supplier
                        const index = suppliersEurope.findIndex(
                            (s) => s.id === id
                        );

                        // If the supplier is in the list
                        if (index !== -1) {
                            // Update the supplier in the list
                            suppliersEurope[index] = updatedSupplierEurope;
                            // Update the BehaviorSubject
                            this._europeSuppliers.next(suppliersEurope);
                        }
                    }
                })
            );
    }

    /**
     * Update Fournisseur
     * @param id - the id of the EuropeDTO to update
     * @param europeSupplierDTO - the EuropeDTO object containing the updated data
     * @returns An Observable of the updated EuropeDTO
     */
    updateEuropeSupplier(
        id: number,
        europeSupplierDTO: EuropeDTO
    ): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .put<EuropeDTO>(
                `${baseUrl}/etablissement_fournisseur/${id}`,
                europeSupplierDTO
            )
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                }),
                tap((updatedEuropeSupplier) => {
                    const currentEuropeSuppliers =
                        this._europeSuppliers.getValue();
                    if (currentEuropeSuppliers) {
                        const index = currentEuropeSuppliers.findIndex(
                            (europeSupplier) =>
                                europeSupplier.id === updatedEuropeSupplier.id
                        );
                        if (index !== -1) {
                            currentEuropeSuppliers[index] =
                                updatedEuropeSupplier;
                            this._europeSuppliers.next(currentEuropeSuppliers);
                        }
                    }
                })
            );
    }

    /**
     * Creates a supplier in Europe based on the provided VAT number.
     *
     * @param {string} vat - The VAT number of the supplier.
     * @return {Observable<EuropeDTO>} An Observable that emits the created supplier in Europe.
     */
    createEuropeSupplierFromVat(vat: string): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<EuropeDTO>(
                `${baseUrl}/fournisseur_europe/add_by_vat/${vat}`,
                {}
            )
            .pipe(
                tap((supplierEurope) => {
                    if (supplierEurope) {
                        // Get the current list of suppliers
                        const suppliersEurope =
                            this._europeSuppliers.getValue();

                        // If the list is not null
                        if (suppliersEurope) {
                            const updatedSuppliersEurope = [
                                ...suppliersEurope,
                                supplierEurope,
                            ];

                            // Update the BehaviorSubject
                            this._europeSuppliers.next(updatedSuppliersEurope);
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

    checkIfVatExists(vat: string): Observable<boolean> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .get<EuropeDTO>(`${baseUrl}/fournisseur_europe/vat/${vat}`)
            .pipe(
                map((entity: EuropeDTO) => {
                    return !!entity?.fournisseur;
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

    createOdooSupplierFromSupplierEurope(id: number): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<EuropeDTO>(
                `${baseUrl}/partner_odoo/create_europe_fournisseur/${id}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope =
                        this._europeSuppliers.getValue();

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
                            this._europeSuppliers.next(currentClientsEurope);
                        }
                    }
                })
            );
    }

    createGpiSupplierFromSupplierEurope(id: number): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<EuropeDTO>(
                `${baseUrl}/gpi_sync/create_europe_fournisseur/${id}`,
                {}
            )
            .pipe(
                tap((newClientEurope) => {
                    // Récupérer la valeur actuelle de _clientsEurope
                    const currentClientsEurope =
                        this._europeSuppliers.getValue();

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
                            this._europeSuppliers.next(currentClientsEurope);
                        }
                    }
                })
            );
    }

    getBusinessUnitsByClientId(
        supplierId: number
    ): Observable<BusinessUnitDto[]> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<BusinessUnitDto[]>(
            `${baseUrl}/client-europe-business-unit/${supplierId}`
        );
    }

    addBusinessUnitToClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.post<EntityBusinessUnitDto>(
            `${baseUrl}/client-europe-business-unit`,
            etablissementClientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromClient(
        etablissementClientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.delete<unknown>(
            `${baseUrl}/client-europe-business-unit`,
            { body: etablissementClientBusinessUnitDTO }
        );
    }

    convertEuropeSupplierDtoToEuropeSupplierFormDto(
        supplierDto: EuropeDTO
    ): FournisseurEuropeFormDto {
        return {
            id: supplierDto.id,
            vat: supplierDto.vat,
            client: supplierDto.client,
            fournisseur: supplierDto.fournisseur,
            idShipperDashdoc: supplierDto.idShipperDashdoc,
            noBalance: supplierDto.noBalance,
            identificationForm: {
                addressForm: {
                    nom: supplierDto.nom,
                    adresseFacturation1: supplierDto.adresseFacturation1,
                    adresseFacturation2: supplierDto.adresseFacturation2,
                    adresseFacturation3: supplierDto.adresseFacturation3,
                    villeFacturation: supplierDto.villeFacturation,
                    codePostalFacturation: supplierDto.codePostalFacturation,
                    paysFacturation: supplierDto.paysFacturation,
                },
                bankInfosForm: {
                    iban: supplierDto.iban,
                    bic: supplierDto.bic,
                },
                contactsForm: {
                    contactFacturation: supplierDto.contactFacturation,
                    emailFacturation: supplierDto.emailFacturation,
                    telephoneFacturation: supplierDto.telephoneFacturation,
                    portableFacturation: supplierDto.portableFacturation,
                    contactAlternatif: supplierDto.contactAlternatif,
                    emailAlternatif: supplierDto.emailAlternatif,
                    telephoneAlternatif: supplierDto.telephoneAlternatif,
                    portableAlternatif: supplierDto.portableAlternatif,
                },
            },
            parametrageForm: {
                billingInfosForm: {
                    frnConditionReglement: supplierDto.frnConditionReglement,
                    frnModeReglement: supplierDto.frnModeReglement,
                    frnDelaiReglement: supplierDto.frnDelaiReglement,
                    frnTauxTva: supplierDto.frnTauxTva,
                    frnCompteComptable: supplierDto.frnCompteComptable,
                    frnEncoursMax: supplierDto.frnEncoursMax,
                    conditionReglement: supplierDto.conditionReglement,
                    modeReglement: supplierDto.modeReglement,
                    delaiReglement: supplierDto.delaiReglement,
                    tauxTva: supplierDto.tauxTva,
                    compteComptable: supplierDto.compteComptable,
                    encoursMax: supplierDto.encoursMax,
                },
                erpCodesForm: {
                    codeMkgt: supplierDto.codeMkgt,
                    idOdoo: supplierDto.idOdoo,
                    codeGpi: supplierDto.codeGpi,
                    frnCodeGpi: supplierDto.frnCodeGpi,
                },
                clientBloque: supplierDto.clientBloque,
                motifBlocage: supplierDto.motifBlocage,
                dateBlocage: supplierDto.dateBlocage,
                categorieId: supplierDto.categorieId,
                commercialId: supplierDto.commercialId,
            },
        };
    }

    convertEuropeSupplierFormDtoToEuropeSupplierDto(
        formDto: FournisseurEuropeFormDto,
        originalSupplierDto: EuropeDTO
    ): EuropeDTO {
        return {
            id: formDto.id,
            nom: formDto.identificationForm.addressForm.nom,
            vat: formDto.vat,

            idShipperDashdoc: originalSupplierDto. idShipperDashdoc,

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
            clientBloque: formDto.parametrageForm.clientBloque,
            motifBlocage: formDto.parametrageForm.motifBlocage,
            dateBlocage: formDto.parametrageForm.dateBlocage,
            categorieId: formDto.parametrageForm.categorieId,
            commercialId: formDto.parametrageForm.commercialId,
            client: originalSupplierDto.client,
            fournisseur: originalSupplierDto.fournisseur,
            noBalance: originalSupplierDto.noBalance,
        };
    }

    /**
     * Creates a supplier in Europe with the provided VAT number.
     *
     * @param {string} vat - The VAT number of the supplier.
     * @return {Observable<EuropeDTO>} An Observable that emits the created supplier in Europe.
     */
    createEmptySupplierEurope(vat: string): Observable<EuropeDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<EuropeDTO>(
                `${baseUrl}/fournisseur_europe/create_from_scratch/${vat}`,
                {}
            )
            .pipe(
                tap((supplierEurope) => {
                    // Get the current list of suppliers
                    const suppliersEurope = this._europeSuppliers.getValue();

                    // If the list is not null
                    if (suppliersEurope) {
                        const updatedSuppliersEurope = [
                            ...suppliersEurope,
                            supplierEurope,
                        ];

                        // Update the BehaviorSubject
                        this._europeSuppliers.next(updatedSuppliersEurope);
                    }
                }),
                catchError((error) => {
                    return throwError(() => error);
                })
            );
    }

    getGroup(): Observable<Group> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<Group>(`${baseUrl}/group`);
    }
}
