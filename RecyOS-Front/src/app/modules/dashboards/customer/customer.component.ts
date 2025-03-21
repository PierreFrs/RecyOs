import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { DashboardCustomerService } from './customer.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { customerDashboardDto } from './customer.type';

@Component({
    selector: 'customer-dashboard',
    templateUrl: './customer.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardCustomerComponent implements OnInit, OnDestroy {
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    dashboardCustomer$: Observable<customerDashboardDto>;
    dashboardCustomer: customerDashboardDto;

    /**
     * Constructor
     */
    constructor(
        private readonly router: Router,
        private readonly _customerDashboardService: DashboardCustomerService,
        private readonly _changeDetectorRef: ChangeDetectorRef
    ) {}

    ngOnInit(): void {
        this.dashboardCustomer$ =
            this._customerDashboardService.customerDashboard$;
        this._customerDashboardService.customerDashboard$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((dashboardCustomer: customerDashboardDto) => {
                this.dashboardCustomer = dashboardCustomer;
                this._changeDetectorRef.markForCheck();
            });
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    afficherListeClients() {
        this.router.navigate(['/customers/businesses']);
    }

    afficherClientsRadies() {
        this.router.navigate(['/dashboards/customer/lists'], {
            queryParams: { mode: 'radie' },
        });
    }

    afficherClientsBadMail() {
        this.router.navigate(['/dashboards/customer/lists'], {
            queryParams: { mode: 'badmail' },
        });
    }

    afficherClientsBadTel() {
        this.router.navigate(['/dashboards/customer/lists'], {
            queryParams: { mode: 'badtel' },
        });
    }

    afficherClientsFactor() {
        this.router.navigate(['/dashboards/customer/lists'], {
            queryParams: { mode: 'factor' },
        });
    }

    afficherDemandesCouverture() {
        this.router.navigate(['/dashboards/customer-coverage/lists'], {
            queryParams: { mode: 'all' },
        });
    }

    afficherDemandesCouvertureAccordees() {
        this.router.navigate(['/dashboards/customer-coverage/lists'], {
            queryParams: { mode: 'accord' },
        });
    }

    afficherDemandesCouvertureRefusees() {
        this.router.navigate(['/dashboards/customer-coverage/lists'], {
            queryParams: { mode: 'refus' },
        });
    }

    afficherDemandesNDCover() {
        this.router.navigate(['/dashboards/customer-nd-cover/lists'], {
            queryParams: { mode: 'all' },
        });
    }

    afficherDemandesNDCoverAccordees() {
        this.router.navigate(['/dashboards/customer-nd-cover/lists'], {
            queryParams: { mode: 'accord' },
        });
    }

    afficherDemandesNDCoverRefusees() {
        this.router.navigate(['/dashboards/customer-nd-cover/lists'], {
            queryParams: { mode: 'refus' },
        });
    }

    afficherClientsSansCommerciaux() {
        this.router.navigate(
            ['/dashboards/customer-without-sales-agent/lists'],
            {
                queryParams: { mode: 'cliSansCom' },
            }
        );
    }
}
