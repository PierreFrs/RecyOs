import { Component } from '@angular/core';
import {AuthService} from "./core/auth/auth.service";

@Component({
    selector   : 'app-root',
    templateUrl: './app.component.html',
    styleUrls  : ['./app.component.scss']
})
export class AppComponent
{
    /**
     * Constructor
     */
    constructor(private readonly authService: AuthService)
    {
        this.initializeApplication();
    }

    private initializeApplication(): void {
        this.authService.initializeUser().subscribe((user) => {
            if (user) {
                console.log('User is initialized');
            } else {
                console.log('User is not initialized');
            }
        })
    }
}
