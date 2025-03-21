import { Injectable } from '@angular/core';
import { BusinessesServices } from '../../../services/entity-services/businesses.services';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { Observable } from 'rxjs';
import {
    EtablissementDTOPagination,
    EtablissementFormDto,
    FrenchDTO,
} from '../../../../models/entities-models/french.type';
import { IEntityServiceStrategy } from '../IEntityServiceStrategy';
import { Group } from 'app/models/group.type';
@Injectable({
    providedIn: 'root',
})
export class EtablissementClientServiceStrategy
    implements
        IEntityServiceStrategy<
            FrenchDTO,
            EtablissementDTOPagination,
            EtablissementFormDto,
            BusinessUnitDto,
            EntityBusinessUnitDto
        >
{
    constructor(private readonly businessesServices: BusinessesServices) {}

    entities$: Observable<FrenchDTO[]> =
        this.businessesServices.etablissementsClients$;
    pagination$: Observable<EtablissementDTOPagination> =
        this.businessesServices.pagination$;

    getEntities(
        pageIndex: number,
        pageSize: number,
        sortField: string,
        sortDirection: 'asc' | 'desc' | '',
        query?: string,
        filterType?: number
    ): Observable<{
        paginator: EtablissementDTOPagination;
        items: FrenchDTO[];
    }> {
        return this.businessesServices.getEtablissementsClients(
            pageIndex,
            pageSize,
            sortField,
            sortDirection,
            query,
            filterType
        );
    }

    getEntityById(clientId: number): Observable<FrenchDTO> {
        return this.businessesServices.getEtablissementClientById(clientId);
    }

    createEntityFromAdministrativeIdentifier(
        administrativeIdentifier: string
    ): Observable<FrenchDTO> {
        return this.businessesServices.createEtablissementClientFromSiret(
            administrativeIdentifier
        );
    }

    checkIfEntityExists(administrativeIdentifier: string): Observable<boolean> {
        return this.businessesServices.checkSiretExists(
            administrativeIdentifier
        );
    }

    convertEntityDtoToEntityFormDto(
        entityDto: FrenchDTO
    ): EtablissementFormDto {
        return this.businessesServices.convertClientDtoToFormDto(entityDto);
    }

    convertEntityFormDtoToEntityDto(
        clientFormDto: EtablissementFormDto,
        originalEntityDto: FrenchDTO
    ): FrenchDTO {
        return this.businessesServices.convertFormDtoToClientDto(
            clientFormDto,
            originalEntityDto
        );
    }

    getBusinessUnitsByEntityId(
        clientId: number
    ): Observable<BusinessUnitDto[]> {
        return this.businessesServices.getBusinessUnitsByClientId(clientId);
    }

    addBusinessUnitToEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this.businessesServices.addBusinessUnitToClient(
            clientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this.businessesServices.removeBusinessUnitFromClient(
            clientBusinessUnitDTO
        );
    }

    updateEntity(clientId: number, data: any): Observable<any> {
        return this.businessesServices.updateEtablissementClient(
            clientId,
            data as FrenchDTO
        );
    }

    createOdooEntityFromEntity(clientId: number): Observable<any> {
        return this.businessesServices.createOdooClientFromEtablissementClient(
            clientId
        );
    }

    createGpiEntityFromEntity(clientId: number): Observable<any> {
        return this.businessesServices.createGpiClientFromEtablissementClient(
            clientId
        );
    }

    createDashdocEntityFromEntity(entityId: number): Observable<any> {
        return this.businessesServices.addEtablissementClientToDashdoc(
            entityId
        );
    }

    updateAdministrativeIdentifier(
        administrativeIdentifier: string,
        clientId: number
    ): Observable<any> {
        return this.businessesServices.updateSiret(
            administrativeIdentifier,
            clientId
        );
    }

    createHubspotEntityFromEntity(entityId: number): Observable<any> {
        return this.businessesServices.createHubspotClientFromEtablissementClient(
            entityId
        );
    }

    createEmptyEntity(administrativeIdentifier: string): Observable<FrenchDTO> {
        return this.businessesServices.createEmptyEtablissementClient(
            administrativeIdentifier
        );
    }

    getGroup(): Observable<Group> {
        return this.businessesServices.getGroup();
    }
}
