import { Observable } from 'rxjs';
import { ServiceResult } from '../../../models/service-result.type';
import { Group } from 'app/models/group.type';
export interface IEntityServiceStrategy<T, U, V, W, X> {
    // T: Entity DTO
    // U: Pagination DTO
    // V: Entity Form DTO
    // W: Business Unit DTO
    // X: Entity Business Unit DTO

    entities$: Observable<T[]>;
    pagination$: Observable<U>;

    getEntities(
        pageIndex: number,
        pageSize: number,
        sortField: string,
        sortDirection: 'asc' | 'desc' | '',
        query?: string,
        filterType?: number
    ): Observable<{ paginator: U; items: T[] }>;

    getEntityById(entityId: number): Observable<T>;

    createEntityFromAdministrativeIdentifier(
        administrativeIdentifier: string | FormData
    ): Observable<T>;

    checkIfEntityExists(
        argument1: string,
        argument2?: string,
        argument3?: string
    ): Observable<boolean>;

    convertEntityDtoToEntityFormDto(entityDto: T): V;

    convertEntityFormDtoToEntityDto(entityFormDto: V, originalEntityDto?: T): T;

    updateEntity(id: number, entity: T): Observable<T>;

    getBusinessUnitsByEntityId(entityId: number): Observable<W[]>;

    addBusinessUnitToEntity(entityBusinessUnitDTO: X): Observable<X>;

    removeBusinessUnitFromEntity(entityBusinessUnitDTO: X): Observable<unknown>;

    createOdooEntityFromEntity(entityId: number): Observable<any>;

    createGpiEntityFromEntity(entityId: number): Observable<any>;

    createDashdocEntityFromEntity(entityId: number): Observable<any>;

    createHubspotEntityFromEntity(entityId: number): Observable<any>;

    updateAdministrativeIdentifier(
        administrativeIdentifier: string,
        entityId: number
    ): Observable<ServiceResult>;

    createEmptyEntity(administrativeIdentifier: string): Observable<T>;

    getGroup(): Observable<Group>;
}
