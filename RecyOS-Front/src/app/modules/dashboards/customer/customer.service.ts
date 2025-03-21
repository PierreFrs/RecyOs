import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {BackendApiService} from "../../../backend.api.service";
import {BehaviorSubject, Observable, tap} from "rxjs";
import {customerDashboardDto} from "./customer.type";

@Injectable({
    providedIn: 'root'
})
export class DashboardCustomerService {
    private _baseUrl: string;
    private _customerDashboard: BehaviorSubject<customerDashboardDto | null> = new BehaviorSubject<customerDashboardDto | null>(null);

    constructor(private _httpClient: HttpClient, private apiService: BackendApiService) {
        this._baseUrl = this.apiService.getBaseUrl();
    }

    /**
     * Get Customer Dashboard
     */
    getDashboartCustomer(): Observable<customerDashboardDto> {
        return this._httpClient.get<customerDashboardDto>(`${this._baseUrl}/dashboard/customer`).pipe(
            tap((customerDashboard: customerDashboardDto) => {
                this._customerDashboard.next(customerDashboard);
            })
        );
    }

    /**
     * Getter for customerDashboard
     */
    get customerDashboard$(): Observable<customerDashboardDto> {
        return this._customerDashboard.asObservable();
    }
}
