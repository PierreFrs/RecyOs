import { Injectable } from '@angular/core';
import { EuropeServices } from '../../../services/entity-services/europe.services';
import { Observable } from 'rxjs';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import {
    EuropeDTO,
    EuropeDTOPagination,
    EuropeFormDto,
} from '../../../../models/entities-models/europe.type';
import { IEntityServiceStrategy } from '../IEntityServiceStrategy';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class ClientEuropeServiceStrategy
    implements
        IEntityServiceStrategy<
            EuropeDTO,
            EuropeDTOPagination,
            EuropeFormDto,
            BusinessUnitDto,
            EntityBusinessUnitDto
        >
{
    constructor(private readonly europeServices: EuropeServices) {}

    entities$: Observable<EuropeDTO[]> = this.europeServices.clientsEurope$;
    pagination$: Observable<EuropeDTOPagination> =
        this.europeServices.pagination$;

    getEntities(
        pageIndex: number,
        pageSize: number,
        sortField: string,
        sortDirection: 'asc' | 'desc' | '',
        query?: string,
        filterType?: number
    ): Observable<{
        paginator: EuropeDTOPagination;
        items: EuropeDTO[];
    }> {
        return this.europeServices.getClientsEurope(
            pageIndex,
            pageSize,
            sortField,
            sortDirection,
            query,
            filterType
        );
    }

    getEntityById(clientId: number): Observable<EuropeDTO> {
        return this.europeServices.getClientEuropeById(clientId);
    }

    createEntityFromAdministrativeIdentifier(
        administrativeIdentifier: string
    ): Observable<EuropeDTO> {
        return this.europeServices.createClientEuropeFromVat(
            administrativeIdentifier
        );
    }

    checkIfEntityExists(administrativeIdentifier: string): Observable<boolean> {
        return this.europeServices.checkVat(administrativeIdentifier);
    }

    convertEntityDtoToEntityFormDto(entityDto: EuropeDTO): EuropeFormDto {
        return this.europeServices.convertClientDtoToFormDto(entityDto);
    }

    convertEntityFormDtoToEntityDto(
        clientFormDto: EuropeFormDto,
        originalEntityDto: EuropeDTO
    ): EuropeDTO {
        return this.europeServices.convertFormDtoToClientDto(
            clientFormDto,
            originalEntityDto
        );
    }

    getBusinessUnitsByEntityId(
        clientId: number
    ): Observable<BusinessUnitDto[]> {
        return this.europeServices.getBusinessUnitsByClientId(clientId);
    }

    addBusinessUnitToEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this.europeServices.addBusinessUnitToClient(
            clientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this.europeServices.removeBusinessUnitFromClient(
            clientBusinessUnitDTO
        );
    }

    updateEntity(clientId: number, data: any): Observable<any> {
        return this.europeServices.updateClientEurope(
            clientId,
            data as EuropeDTO
        );
    }

    createOdooEntityFromEntity(clientId: number): Observable<any> {
        return this.europeServices.createOdooClientFromClientEurope(clientId);
    }

    createGpiEntityFromEntity(clientId: number): Observable<any> {
        return this.europeServices.createGpiClientFromClientEurope(clientId);
    }

    createDashdocEntityFromEntity(clientId: number): Observable<any> {
        return this.europeServices.addClientEuropeToDashdoc(clientId);
    }

    updateAdministrativeIdentifier(
        administrativeIdentifier: string,
        clientId: number
    ): Observable<any> {
        throw new Error('Method not implemented.');
    }

    createHubspotEntityFromEntity(entityId: number): Observable<any> {
        return this.europeServices.createHubspotClientFromClientEurope(
            entityId
        );
    }

    createEmptyEntity(administrativeIdentifier: string): Observable<EuropeDTO> {
        return this.europeServices.createEmptyClientEurope(
            administrativeIdentifier
        );
    }

    getGroup(): Observable<Group> {
        return this.europeServices.getGroup();
    }
}
