import {
    ClientParticulierDto,
    ClientParticulierDtoPagination,
    ClientParticulierFormDto,
} from '../../../../models/entities-models/particulier.type';
import { IEntityServiceStrategy } from '../IEntityServiceStrategy';
import { Injectable } from '@angular/core';
import { ParticulierService } from '../../../services/entity-services/particulier.service';
import { Observable } from 'rxjs';
import { ServiceResult } from 'app/models/service-result.type';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class ParticulierServiceStrategy
    implements
        IEntityServiceStrategy<
            ClientParticulierDto,
            ClientParticulierDtoPagination,
            ClientParticulierFormDto,
            any,
            any
        >
{
    constructor(private readonly particulierService: ParticulierService) {}

    entities$: Observable<ClientParticulierDto[]> =
        this.particulierService.etablissementsClients$;
    pagination$: Observable<ClientParticulierDtoPagination> =
        this.particulierService.pagination$;

    getEntities(
        pageIndex: number,
        pageSize: number,
        sortField: string,
        sortDirection: 'asc' | 'desc' | '',
        query?: string,
        filterType?: number
    ): Observable<{
        paginator: ClientParticulierDtoPagination;
        items: ClientParticulierDto[];
    }> {
        return this.particulierService.getClientsParticuliers(
            pageIndex,
            pageSize,
            sortField,
            sortDirection,
            query,
            filterType
        );
    }

    getEntityById(clientId: number): Observable<ClientParticulierDto> {
        return this.particulierService.getClientParticulierById(clientId);
    }

    createEntityFromAdministrativeIdentifier(
        form: FormData
    ): Observable<ClientParticulierDto> {
        return this.particulierService.createClientParticulierFromForm(form);
    }

    checkIfEntityExists(
        prenom: string,
        nom: string,
        ville: string
    ): Observable<boolean> {
        return this.particulierService.checkNomExists(prenom, nom, ville);
    }

    convertEntityDtoToEntityFormDto(
        entityDto: ClientParticulierDto
    ): ClientParticulierFormDto {
        return this.particulierService.convertClientDtoToFormDto(entityDto);
    }

    convertEntityFormDtoToEntityDto(
        clientFormDto: ClientParticulierFormDto,
        originalEntityDto?: ClientParticulierDto
    ): ClientParticulierDto {
        return this.particulierService.convertFormDtoToClientDto(clientFormDto);
    }

    updateEntity(
        clientId: number,
        entity: ClientParticulierDto
    ): Observable<ClientParticulierDto> {
        return this.particulierService.updateClientParticulier(
            clientId,
            entity
        );
    }

    createOdooEntityFromEntity(clientId: number): Observable<any> {
        return this.particulierService.createOdooClientFromClientParticulier(
            clientId
        );
    }

    createDashdocEntityFromEntity(entityId: number): Observable<any> {
        return this.particulierService.addClientParticulierToDashdoc(entityId);
    }

    getBusinessUnitsByEntityId(entityId: number): Observable<any[]> {
        throw new Error('Method not implemented.');
    }
    addBusinessUnitToEntity(entityBusinessUnitDTO: any): Observable<any> {
        throw new Error('Method not implemented.');
    }
    removeBusinessUnitFromEntity(
        entityBusinessUnitDTO: any
    ): Observable<unknown> {
        throw new Error('Method not implemented.');
    }
    createGpiEntityFromEntity(entityId: number): Observable<any> {
        throw new Error('Method not implemented.');
    }
    updateAdministrativeIdentifier(
        administrativeIdentifier: string,
        entityId: number
    ): Observable<ServiceResult> {
        throw new Error('Method not implemented.');
    }
    createHubspotEntityFromEntity(entityId: number): Observable<any> {
        throw new Error('Method not implemented.');
    }
    createEmptyEntity(
        administrativeIdentifier: string
    ): Observable<ClientParticulierDto> {
        throw new Error('Method not implemented.');
    }
    getGroup(): Observable<Group> {
        throw new Error('Method not implemented.');
    }
}
