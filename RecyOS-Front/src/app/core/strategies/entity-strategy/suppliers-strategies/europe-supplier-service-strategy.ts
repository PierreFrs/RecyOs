import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SupplierDTOPagination } from '../../../../models/entities-models/supplier.type';
import {
    EuropeDTO,
    EuropeDTOPagination,
    FournisseurEuropeFormDto,
} from '../../../../models/entities-models/europe.type';
import { IEntityServiceStrategy } from '../IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { ServiceResult } from 'app/models/service-result.type';
import { EuropeSuppliersService } from '../../../services/entity-services/europe-suppliers.service';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class EuropeSupplierServiceStrategy
    implements
        IEntityServiceStrategy<
            EuropeDTO,
            EuropeDTOPagination,
            FournisseurEuropeFormDto,
            BusinessUnitDto,
            EntityBusinessUnitDto
        >
{
    constructor(
        private readonly europeSuppliersService: EuropeSuppliersService
    ) {}

    createDashdocEntityFromEntity(entityId: number): Observable<any> {
        throw new Error('Method not implemented.');
    }

    entities$: Observable<EuropeDTO[]> =
        this.europeSuppliersService.suppliersEurope$;
    pagination$: Observable<SupplierDTOPagination> =
        this.europeSuppliersService.pagination$;

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
        return this.europeSuppliersService.getSuppliersEurope(
            pageIndex,
            pageSize,
            sortField,
            sortDirection,
            query,
            filterType
        );
    }

    getEntityById(clientId: number): Observable<EuropeDTO> {
        return this.europeSuppliersService.getSupplierEuropeById(clientId);
    }

    createEntityFromAdministrativeIdentifier(
        administrativeIdentifier: string
    ): Observable<EuropeDTO> {
        return this.europeSuppliersService.createEuropeSupplierFromVat(
            administrativeIdentifier
        );
    }

    checkIfEntityExists(administrativeIdentifier: string): Observable<boolean> {
        return this.europeSuppliersService.checkIfVatExists(
            administrativeIdentifier
        );
    }

    convertEntityDtoToEntityFormDto(
        entityDto: EuropeDTO
    ): FournisseurEuropeFormDto {
        return this.europeSuppliersService.convertEuropeSupplierDtoToEuropeSupplierFormDto(
            entityDto
        );
    }

    convertEntityFormDtoToEntityDto(
        entityFormDto: FournisseurEuropeFormDto,
        originalSupplierDto: EuropeDTO
    ): EuropeDTO {
        return this.europeSuppliersService.convertEuropeSupplierFormDtoToEuropeSupplierDto(
            entityFormDto,
            originalSupplierDto
        );
    }

    getBusinessUnitsByEntityId(
        clientId: number
    ): Observable<BusinessUnitDto[]> {
        return this.europeSuppliersService.getBusinessUnitsByClientId(clientId);
    }

    addBusinessUnitToEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this.europeSuppliersService.addBusinessUnitToClient(
            clientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this.europeSuppliersService.removeBusinessUnitFromClient(
            clientBusinessUnitDTO
        );
    }

    updateEntity(clientId: number, data: any): Observable<any> {
        return this.europeSuppliersService.updateSupplierEurope(
            clientId,
            data as EuropeDTO
        );
    }

    createOdooEntityFromEntity(clientId: number): Observable<any> {
        return this.europeSuppliersService.createOdooSupplierFromSupplierEurope(
            clientId
        );
    }

    createGpiEntityFromEntity(clientId: number): Observable<any> {
        return this.europeSuppliersService.createGpiSupplierFromSupplierEurope(
            clientId
        );
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

    createEmptyEntity(administrativeIdentifier: string): Observable<EuropeDTO> {
        return this.europeSuppliersService.createEmptySupplierEurope(
            administrativeIdentifier
        );
    }

    getGroup(): Observable<Group> {
        return this.europeSuppliersService.getGroup();
    }
}
