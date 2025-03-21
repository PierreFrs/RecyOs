import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SupplierDTOPagination } from '../../../../models/entities-models/supplier.type';
import {
    EtablissementDTOPagination,
    FournisseurFranceFormDto,
    FrenchDTO,
} from '../../../../models/entities-models/french.type';
import { IEntityServiceStrategy } from '../IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { ServiceResult } from 'app/models/service-result.type';
import { FrenchSuppliersService } from '../../../services/entity-services/french-suppliers.service';
import { Group } from 'app/models/group.type';

@Injectable({
    providedIn: 'root',
})
export class FrenchSupplierServiceStrategy
    implements
        IEntityServiceStrategy<
            FrenchDTO,
            EtablissementDTOPagination,
            FournisseurFranceFormDto,
            BusinessUnitDto,
            EntityBusinessUnitDto
        >
{
    constructor(
        private readonly frenchSuppliersService: FrenchSuppliersService
    ) {}

    createDashdocEntityFromEntity(entityId: number): Observable<any> {
        throw new Error('Method not implemented.');
    }

    entities$: Observable<FrenchDTO[]> =
        this.frenchSuppliersService.frenchSuppliers$;
    pagination$: Observable<SupplierDTOPagination> =
        this.frenchSuppliersService.pagination$;

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
        return this.frenchSuppliersService.getFrenchSuppliers(
            pageIndex,
            pageSize,
            sortField,
            sortDirection,
            query,
            filterType
        );
    }

    getEntityById(supplierId: number): Observable<FrenchDTO> {
        return this.frenchSuppliersService.getFrenchSupplierById(supplierId);
    }

    createEntityFromAdministrativeIdentifier(
        siret: string
    ): Observable<FrenchDTO> {
        return this.frenchSuppliersService.createFrenchSupplierFromSiret(siret);
    }

    checkIfEntityExists(siret: string): Observable<boolean> {
        return this.frenchSuppliersService.checkIfSiretExists(siret);
    }

    convertEntityDtoToEntityFormDto(
        supplierDto: FrenchDTO
    ): FournisseurFranceFormDto {
        return this.frenchSuppliersService.convertFranceSupplierDtoToFranceSupplierFormDto(
            supplierDto
        );
    }

    convertEntityFormDtoToEntityDto(
        supplierFormDto: FournisseurFranceFormDto,
        originalSupplierDto: FrenchDTO
    ): FrenchDTO {
        return this.frenchSuppliersService.convertFranceSupplierFormDtoToFranceSupplierDto(
            supplierFormDto,
            originalSupplierDto
        );
    }

    getBusinessUnitsByEntityId(
        clientId: number
    ): Observable<BusinessUnitDto[]> {
        return this.frenchSuppliersService.getBusinessUnitsBySupplierId(
            clientId
        );
    }

    addBusinessUnitToEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<EntityBusinessUnitDto> {
        return this.frenchSuppliersService.addBusinessUnitToSupplier(
            clientBusinessUnitDTO
        );
    }

    removeBusinessUnitFromEntity(
        clientBusinessUnitDTO: EntityBusinessUnitDto
    ): Observable<unknown> {
        return this.frenchSuppliersService.removeBusinessUnitFromSupplier(
            clientBusinessUnitDTO
        );
    }

    updateEntity(id: number, supplier: FrenchDTO): Observable<FrenchDTO> {
        return this.frenchSuppliersService.updateFrenchSupplier(id, supplier);
    }

    createOdooEntityFromEntity(clientId: number): Observable<any> {
        return this.frenchSuppliersService.createOdooSupplierFromSupplierFrance(
            clientId
        );
    }

    createGpiEntityFromEntity(clientId: number): Observable<any> {
        return this.frenchSuppliersService.createGpiSupplierFromSupplierFrance(
            clientId
        );
    }

    updateAdministrativeIdentifier(
        administrativeIdentifier: string,
        entityId: number
    ): Observable<ServiceResult> {
        return this.frenchSuppliersService.updateSiret(
            administrativeIdentifier,
            entityId
        );
    }

    createHubspotEntityFromEntity(entityId: number): Observable<any> {
        throw new Error('Method not implemented.');
    }

    createEmptyEntity(administrativeIdentifier: string): Observable<FrenchDTO> {
        return this.frenchSuppliersService.createEmptySupplier(
            administrativeIdentifier
        );
    }

    getGroup(): Observable<Group> {
        return this.frenchSuppliersService.getGroup();
    }
}
