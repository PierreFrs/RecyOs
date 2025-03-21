import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, of, tap, throwError } from 'rxjs';
import {
    FrenchDTO,
    EtablissementDTOPagination,
    FournisseurFranceFormDto,
} from '../../../models/entities-models/french.type';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../../backend.api.service';
import { FrenchEntityFilterService } from '../entity-filter-services/french-entity-filter.service';
import { catchError } from 'rxjs/operators';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../models/business-unit.type';
import { SupplierDTOPagination } from '../../../models/entities-models/supplier.type';
import { ServiceResult } from '../../../models/service-result.type';
import { SiretUpdateRequest } from '../../../models/Requests/siret-update-request';
import { Group } from '../../../models/group.type';

@Injectable({
    providedIn: 'root',
})
export class FrenchSuppliersService {
    private readonly _frenchSuppliers: BehaviorSubject<FrenchDTO[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<SupplierDTOPagination> =
        new BehaviorSubject(null);
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
        private readonly _frenchEntityService: FrenchEntityFilterService
    ) {}

    /**
     * Retrieves suppliers from France based on the provided parameters.
     *
     * @param {number} page - The page number to retrieve. Default value is 0.
     * @param {number} size - The number of items per page. Default value is 10.
     * @param {string} sort - The field to sort the results by. Default value is 'Siret'.
     * @param {'asc' | 'desc' | ''} order - The order to sort the results in. Default value is 'asc'.
     * @param {string} search - The search term to filter the results by. Default value is an empty string.
     * @param {number} filterType - The type of filter to apply. Default value is 0.
     *
     * @return {Observable<{paginator: EtablissementDTOPagination, items: FrenchDTO[]}>} - An observable containing the paginator metadata and the list of supplier items.
     */
    getFrenchSuppliers(
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
        const baseUrl = this.apiService.getBaseUrl();
        let filterParam = this._frenchEntityService.getfilterParam(filterType);

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
            }>(`${baseUrl}/etablissement_fournisseur`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._frenchSuppliers.next(response.items);
                    this._pagination.next(response.paginator);
                })
            );
    }

    /**
     * Retrieves the French suppliers as an Observable.
     *
     * @return {Observable<FrenchDTO[] | null>} The French suppliers as an Observable.
     */
    get frenchSuppliers$(): Observable<FrenchDTO[] | null> {
        return this._frenchSuppliers.asObservable();
    }

    /**
     * Retrieves the pagination$ Observable for the EtablissementDTOPagination.
     * This method returns an Observable<EtablissementDTOPagination> object which can be
     * subscribed to in order to get notified when the pagination$ value changes.
     *
     * @return {Observable<EtablissementDTOPagination>} The pagination$ Observable for the EtablissementDTOPagination.
     *
     * @example
     * pagination$.subscribe((pagination) => {
     *   console.log(pagination);
     * });
     */
    get pagination$(): Observable<SupplierDTOPagination> {
        return this._pagination.asObservable();
    }

    /**
     * Retrieves the French supplier by the given ID.
     *
     * @param {number} id - The ID of the supplier to retrieve.
     *
     * @return {Observable<FrenchDTO>} - An Observable that emits the supplier data.
     */
    getFrenchSupplierById(id: number): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<FrenchDTO>(
            `${baseUrl}/etablissement_fournisseur/${id}`
        );
    }

    /**
     * Update Fournisseur
     * @param id - the id of the FrenchDTO to update
     * @param frenchSupplierDTO - the FrenchDTO object containing the updated data
     * @returns An Observable of the updated FrenchDTO
     */
    updateFrenchSupplier(
        id: number,
        frenchSupplierDTO: FrenchDTO
    ): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .put<FrenchDTO>(
                `${baseUrl}/etablissement_fournisseur/${id}`,
                frenchSupplierDTO
            )
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                }),
                tap((updatedFrenchSupplier) => {
                    const currentFrenchSuppliers =
                        this._frenchSuppliers.getValue();
                    if (currentFrenchSuppliers) {
                        const index = currentFrenchSuppliers.findIndex(
                            (frenchSupplier) =>
                                frenchSupplier.id === updatedFrenchSupplier.id
                        );
                        if (index !== -1) {
                            currentFrenchSuppliers[index] =
                                updatedFrenchSupplier;
                            this._frenchSuppliers.next(currentFrenchSuppliers);
                        }
                    }
                })
            );
    }

    /**
     * Creates a French supplier from the given SIRET number.
     *
     * @param {string} siret - The SIRET number to create the supplier from.
     *
     * @return {Observable<FrenchDTO>} - An Observable that emits the created supplier data.
     */

    createFrenchSupplierFromSiret(siret: string): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<FrenchDTO>(
                `${baseUrl}/etablissement_fournisseur/add_by_siret/${siret}`,
                {}
            )
            .pipe(
                tap((newFrenchSupplier) => {
                    if (newFrenchSupplier) {
                        const currentSuppliers =
                            this._frenchSuppliers.getValue();

                        if (currentSuppliers) {
                            const updatedSuppliers = [
                                ...currentSuppliers,
                                newFrenchSupplier,
                            ];
                            this._frenchSuppliers.next(updatedSuppliers);
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

    checkIfSiretExists(siret: string): Observable<boolean> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .get<FrenchDTO>(
                `${baseUrl}/etablissement_fournisseur/siret/${siret}`
            )
            .pipe(
                map((entity: FrenchDTO) => {
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

    createOdooSupplierFromSupplierFrance(id: number): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<FrenchDTO>(
                `${baseUrl}/partner_odoo/create_french_fournisseur/${id}`,
                {}
            )
            .pipe(
                tap((newsupplierFrance) => {
                    // Récupérer la valeur actuelle de _clientsFrance
                    const currentSuppliersFrance =
                        this._frenchSuppliers.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentSuppliersFrance) {
                        // recuperer l'index du fournisseur france
                        const index = currentSuppliersFrance.findIndex(
                            (client) => client.id === newsupplierFrance.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le fournisseur france par le nouveau fournisseur france
                            currentSuppliersFrance[index] = newsupplierFrance;
                            // Publier la nouvelle valeur
                            this._frenchSuppliers.next(currentSuppliersFrance);
                        }
                    }
                })
            );
    }

    createGpiSupplierFromSupplierFrance(id: number): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<FrenchDTO>(
                `${baseUrl}/gpi_sync/create_french_fournisseur/${id}`,
                {}
            )
            .pipe(
                tap((newSupplierFrance) => {
                    // Récupérer la valeur actuelle de _clientsFrance
                    const currentSuppliersFrance =
                        this._frenchSuppliers.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentSuppliersFrance) {
                        // recuperer l'index du fournisseur france
                        const index = currentSuppliersFrance.findIndex(
                            (client) => client.id === newSupplierFrance.id
                        );

                        // verifier si l'index est different de -1
                        if (index !== -1) {
                            // remplacer le fournisseur france par le nouveau fournisseur france
                            currentSuppliersFrance[index] = newSupplierFrance;
                            // Publier la nouvelle valeur
                            this._frenchSuppliers.next(currentSuppliersFrance);
                        }
                    }
                })
            );
    }

    getBusinessUnitsBySupplierId(
        supplierId: number
    ): Observable<BusinessUnitDto[]> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<BusinessUnitDto[]>(
            `${baseUrl}/etablissement-client-business-unit/${supplierId}`
        );
    }

    addBusinessUnitToSupplier(
        supplierBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.post<EntityBusinessUnitDto>(
            `${baseUrl}/etablissement-client-business-unit`,
            supplierBusinessUnitDTO
        );
    }

    removeBusinessUnitFromSupplier(
        supplierBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.delete<unknown>(
            `${baseUrl}/etablissement-client-business-unit`,
            { body: supplierBusinessUnitDTO }
        );
    }

    convertFranceSupplierDtoToFranceSupplierFormDto(
        supplierDto: FrenchDTO
    ): FournisseurFranceFormDto {
        return {
            id: supplierDto.id,
            siret: supplierDto.siret,
            siren: supplierDto.siren,
            client: supplierDto.client,
            idShipperDashdoc: supplierDto.idDashdoc,
            noBalance: supplierDto.noBalance,
            clientGroupe: supplierDto.clientGroupe,
            fournisseur: supplierDto.fournisseur,
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
                tva: supplierDto.tva,
                clientBloque: supplierDto.clientBloque,
                motifBlocage: supplierDto.motifBlocage,
                dateBlocage: supplierDto.dateBlocage,
                categorieId: supplierDto.categorieId,
                commercialId: supplierDto.commercialId,
            },
        };
    }

    convertFranceSupplierFormDtoToFranceSupplierDto(
        formDto: FournisseurFranceFormDto,
        originalFournisseurDto: FrenchDTO
    ): FrenchDTO {
        return {
            id: formDto.id,
            nom: formDto.identificationForm.addressForm.nom,
            siret: formDto.siret,
            siren: formDto.siren,
            idShipperDashdoc: originalFournisseurDto.idDashdoc,
            noBalance: originalFournisseurDto.noBalance,
            clientGroupe: originalFournisseurDto.clientGroupe,
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
            tva: formDto.parametrageForm.tva,
            clientBloque: formDto.parametrageForm.clientBloque,
            motifBlocage: formDto.parametrageForm.motifBlocage,
            dateBlocage: formDto.parametrageForm.dateBlocage,
            categorieId: formDto.parametrageForm.categorieId,
            commercialId: formDto.parametrageForm.commercialId,
            client: originalFournisseurDto.client,
            fournisseur: originalFournisseurDto.fournisseur,
            isDeleted: originalFournisseurDto.isDeleted,
            entrepriseBase: originalFournisseurDto.entrepriseBase,
        };
    }

    updateSiret(siret: string, id: number): Observable<ServiceResult> {
        const baseUrl = this.apiService.getBaseUrl();
        const url = `${baseUrl}/etablissement_fournisseur/change-siret/${id}`;
        const body: SiretUpdateRequest = { siret: siret };

        return this._httpClient.put<ServiceResult>(url, body).pipe(
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    /**
     * Creates an empty French supplier with the given SIRET number.
     *
     * @param {string} siret - The SIRET number to create the supplier with.
     *
     * @return {Observable<FrenchDTO>} - An Observable that emits the created supplier data.
     */

    createEmptySupplier(siret: string): Observable<FrenchDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .post<FrenchDTO>(
                `${baseUrl}/etablissement_fournisseur/create_from_scratch/${siret}`,
                {}
            )
            .pipe(
                tap((newFrenchSupplier) => {
                    const currentSuppliers = this._frenchSuppliers.getValue();

                    if (currentSuppliers) {
                        const updatedSuppliers = [
                            ...currentSuppliers,
                            newFrenchSupplier,
                        ];
                        this._frenchSuppliers.next(updatedSuppliers);
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
