import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { Group } from 'app/models/group.type';
import { GroupService } from '../services/group.service';

@Injectable({
    providedIn: 'root',
})
export class GroupResolver implements Resolve<Group[]> {
    constructor(private groupService: GroupService) {}

    resolve(): Observable<Group[]> {
        return this.groupService.getGroups();
    }
}
