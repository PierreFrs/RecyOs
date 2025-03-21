import { Injectable } from '@angular/core';
import {
    ClientParticulierDto,
    ClientParticulierDtoPagination,
    ClientParticulierFormDto
} from "../../../models/entities-models/particulier.type";
import {BehaviorSubject, catchError, map, Observable, of, tap, throwError} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {BackendApiService} from "../../../backend.api.service";
import {ParticulierEntityFilterService} from "../entity-filter-services/particulier-entity-filter.service";
import {switchMap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class ParticulierService {
    private readonly _clientsParticuliers: BehaviorSubject<ClientParticulierDto[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<ClientParticulierDtoPagination | null> =
        new BehaviorSubject(null);

    private readonly baseUrl = this.apiService.getBaseUrl();

  constructor(
      private readonly _httpClient: HttpClient,
      private readonly apiService: BackendApiService,
      private readonly _particulierEntityFilterService: ParticulierEntityFilterService,
  ) {}

    /**
     * Get Clients Particuliers
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
     * @returns {Observable<{ paginator: ClientParticulierDtoPagination; items: ClientParticulierDto[] }>}
     *          An Observable containing the pagination data and the list of ClientParticulierDto items
     */
    getClientsParticuliers(
        page: number = 0,
        size: number = 10,
        sort: string = 'Nom',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0,
    ): Observable<{
        paginator: ClientParticulierDtoPagination;
        items: ClientParticulierDto[];
    }> {
        let filterParam = this._particulierEntityFilterService.getfilterParam(filterType);

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
                paginator: ClientParticulierDtoPagination;
                items: ClientParticulierDto[];
            }>(`${this.baseUrl}/client_particulier`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._clientsParticuliers.next(response.items);
                    this._pagination.next(response.paginator);
                }),
            );
    }

    /**
     * Get Client Particulier by Id
     * @param id - The unique identifier of the Client Particulier to retrieve
     * @returns An Observable of the ClientParticulierDto containing the Client Particulier data
     */
    getClientParticulierById(id: number): Observable<ClientParticulierDto> {
        return this._httpClient.get<ClientParticulierDto>(
            `${this.baseUrl}/client_particulier/${id}`,
        );
    }

    /**
     * Get Client Particulier by code MKGT
     * @param codeMkgt - The code MKGT of the Client Particulier to retrieve
     * @returns An Observable of the ClientParticulierDto containing the Client Particulier data
     */
    getClientParticulierByCodeMkgt(codeMkgt: string): Observable<ClientParticulierDto> {
        return this._httpClient.get<ClientParticulierDto>(
            `${this.baseUrl}/client_particulier/code_mkgt/${codeMkgt}`,
        );
    }

    /**
     * Get the Clients Particuliers stream
     * @returns An Observable stream of ClientParticulierDto[] or null
     */
    get etablissementsClients$(): Observable<ClientParticulierDto[] | null> {
        return this._clientsParticuliers.asObservable();
    }

    /**
     * Get the pagination stream for Client Particuliers
     * @returns An Observable stream of ClientParticulierDtoPagination or null
     */
    get pagination$(): Observable<ClientParticulierDtoPagination> {
        return this._pagination.asObservable();
    }

    /**
     * Update Client Particulier
     * @param id - the id of the ClientParticulierDto to update
     * @param clientParticulierDto - the ClientParticulierDto object containing the updated data
     * @returns An Observable of the updated ClientParticulierDto
     */
    updateClientParticulier(
        id: number,
        clientParticulierDto: ClientParticulierDto,
    ): Observable<ClientParticulierDto> {

        return this._httpClient
            .put<ClientParticulierDto>(
                `${this.baseUrl}/client_particulier/${id}`,
                clientParticulierDto,
            )
            .pipe(
                catchError((error) => {
                    if (error.status === 480) {
                        return throwError(() => error);
                    }
                    return throwError(() => error);
                }),
                tap((updatedClientParticulier) => {
                    // Update the local BehaviorSubject with the new data
                    const currentClientsParticuliers = this._clientsParticuliers.getValue();

                    if (currentClientsParticuliers) {
                        const index = currentClientsParticuliers.findIndex(
                            (clientParticulier) => clientParticulier.id === updatedClientParticulier.id,
                        );

                        if (index !== -1) {
                            currentClientsParticuliers[index] = updatedClientParticulier;
                            this._clientsParticuliers.next(currentClientsParticuliers);
                        }
                    }
                }),
                switchMap(() => this.getClientParticulierById(id))
            );
    }

    /**
     * Check if the client already exists
     * @param prenom - The prenom to check
     * @param nom - The nom to check
     * @param ville - The ville to check
     * @returns An Observable of boolean
     *         true if the name is already used by another Client Particulier
     *         use the API endpoint /client_particulier/nom/{prenom}/{nom} to check if the SIRET number is already used by another Client Particulier
     */
    checkNomExists(prenom: string, nom: string, ville: string): Observable<boolean> {
        return this._httpClient
            .get<ClientParticulierDto | null>(`${this.baseUrl}/client_particulier/${prenom}/${nom}/${ville}`, { observe: 'response' })
            .pipe(
                map((response) : boolean => {
                    if (response.status === 204) {
                        return false;  // NoContent, meaning client doesn't exist
                    } else return !!(response.status === 200 && response.body);
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
     * Create Client Particulier form siret number
     * Server will create the Client Particulier from the siret number and pappers API datas
     * @param form - The form handling the necessary data to create the Client Particulier for
     * @returns An Observable of the ClientParticulierDto containing the Client Particulier data
     */
    createClientParticulierFromForm(form: FormData): Observable<ClientParticulierDto> {
        return this._httpClient
            .post<ClientParticulierDto>(
                `${this.baseUrl}/client_particulier`,
                form
            )
            .pipe(
                tap((newClientParticulier) => {
                    const currentClientsParticuliers = this._clientsParticuliers.getValue();

                    if (currentClientsParticuliers) {
                        const updatedClientsParticuliers = [
                            ...currentClientsParticuliers,
                            newClientParticulier,
                        ];

                        this._clientsParticuliers.next(updatedClientsParticuliers);
                    }
                }),
            );
    }


    addClientParticulierToDashdoc(id: number): Observable<ClientParticulierDto> {
        return this._httpClient.post<ClientParticulierDto>(
            `${this.baseUrl}/create_particulier_dashdoc_company/${id}`,
            {},
        )
            .pipe(
                tap((newClientParticulier) => {
                    // Récupérer la valeur actuelle de _clientsParticuliers
                    const currentClientsParticuliers =
                        this._clientsParticuliers.getValue();

                    // Vérifier si la valeur actuelle n'est pas null
                    if (currentClientsParticuliers) {
                        // Récupérer l'index du client à mettre à jour
                        const index = currentClientsParticuliers.findIndex(
                            (clientParticulier) =>
                                clientParticulier.id === newClientParticulier.id,
                        );

                        // Vérifier si l'index est différent de -1
                        if (index !== -1) {
                            // Mettre à jour l'établissement client dans la liste
                            currentClientsParticuliers[index] = newClientParticulier;

                            // Mettre à jour le BehaviorSubject _clientsParticuliers avec la nouvelle liste
                            this._clientsParticuliers.next(currentClientsParticuliers,);
                        }
                    }
                }),
            );
    }

    createOdooClientFromClientParticulier(id: number): Observable<ClientParticulierDto> {
        return this._httpClient
            .post<ClientParticulierDto>(
                `${this.baseUrl}/partner_odoo/create_particulier_client/${id}`,
                {}
            )
            .pipe(
                tap(
                    (newClientParticulier) => {
                        const currentClientsParticuliers = this._clientsParticuliers.getValue();

                        if (currentClientsParticuliers) {
                            const index = currentClientsParticuliers.findIndex(
                                (clientParticulier) => clientParticulier.id === newClientParticulier.id
                            );

                            if (index !== -1) {
                                currentClientsParticuliers[index] = newClientParticulier;

                                this._clientsParticuliers.next(currentClientsParticuliers);
                            }
                        }
                    }
                ),
                catchError((error) => {
                    console.error('Error during Odoo client creation request:', error);
                    return throwError(() => error);
                })
            );
    }

    convertClientDtoToFormDto(clientDto: ClientParticulierDto): ClientParticulierFormDto {
        return {
            id: clientDto.id,
            noBalance: clientDto.noBalance,
            idShipperDashdoc: clientDto.idShipperDashdoc,         
            identificationForm: {
                addressForm: {
                    titre: clientDto.titre,
                    nom: clientDto.nom,
                    prenom: clientDto.prenom,
                    adresseFacturation1: clientDto.adresseFacturation1,
                    adresseFacturation2: clientDto.adresseFacturation2,
                    adresseFacturation3: clientDto.adresseFacturation3,
                    codePostalFacturation: clientDto.codePostalFacturation,
                    villeFacturation: clientDto.villeFacturation,
                    paysFacturation: clientDto.paysFacturation,
                },
                contactsForm: {
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
                },
                erpCodesForm: {
                    codeMkgt: clientDto.codeMkgt,
                    idOdoo: clientDto.idOdoo,
                },
                clientBloque: clientDto.clientBloque,
                motifBlocage: clientDto.motifBlocage,
                dateBlocage: clientDto.dateBlocage,
            },
        };
    }

    convertFormDtoToClientDto(
        formDto: ClientParticulierFormDto
    ): ClientParticulierDto {
        return {
            id: formDto.id,
            idShipperDashdoc: formDto.idShipperDashdoc,
            noBalance: formDto.noBalance,
            nom: formDto.identificationForm.addressForm.nom,
            prenom: formDto.identificationForm.addressForm.prenom,
            titre: formDto.identificationForm.addressForm.titre,
            adresseFacturation1:
            formDto.identificationForm.addressForm.adresseFacturation1,
            adresseFacturation2:
            formDto.identificationForm.addressForm.adresseFacturation2,
            adresseFacturation3:
            formDto.identificationForm.addressForm.adresseFacturation3,
            codePostalFacturation:
            formDto.identificationForm.addressForm.codePostalFacturation,
            villeFacturation:
            formDto.identificationForm.addressForm.villeFacturation,
            paysFacturation:
            formDto.identificationForm.addressForm.paysFacturation,
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
            clientBloque: formDto.parametrageForm.clientBloque,
            motifBlocage: formDto.parametrageForm.motifBlocage,
            dateBlocage: formDto.parametrageForm.dateBlocage,
        };
    }
}
