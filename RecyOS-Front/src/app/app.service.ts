import {Injectable} from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class AppService {
    private readonly currentVersion: string = 'v0.3';

    constructor() { }

    public getCurrentVersion(): string {
        return this.currentVersion;
    }
}
