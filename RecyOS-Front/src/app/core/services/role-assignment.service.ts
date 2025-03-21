import { Injectable } from '@angular/core';
import { UserService } from './user.service';

@Injectable({
    providedIn: 'root',
})
export class RoleAssignmentService {
    userRoles: {
        isAdmin: boolean;
        canReadClient: boolean;
        canCreateClient: boolean;
        canUpdateClient: boolean;
        canReadFournisseur: boolean;
        canCreateFournisseur: boolean;
        canUpdateFournisseur: boolean;
        canWriteBankInfos: boolean;
        canUpdateSiret: boolean;
        canUpdateParticulier: boolean;
        canCreateMkgt: boolean;
        canCreateOdoo: boolean;
        canCreateGpi: boolean;
        canCreateDashdoc: boolean;
    } = {
        isAdmin: false,
        canReadClient: false,
        canCreateClient: false,
        canUpdateClient: false,
        canReadFournisseur: false,
        canCreateFournisseur: false,
        canUpdateFournisseur: false,
        canWriteBankInfos: false,
        canUpdateSiret: false,
        canUpdateParticulier: false,
        canCreateMkgt: false,
        canCreateOdoo: false,
        canCreateGpi: false,
        canCreateDashdoc: false,
    };
    constructor(private readonly _userService: UserService) {
        this.assignUserRole();
    }

    private assignUserRole(): void {
        this._userService.user$.subscribe((user) => {
            if (user?.id) {
                this.userRoles.isAdmin = user.roles.some(
                    (role) => role.name === 'admin'
                );
                this.userRoles.canReadClient = user.roles.some(
                    (role) => role.name === 'read_client'
                );
                this.userRoles.canCreateClient = user.roles.some(
                    (role) => role.name === 'create_client'
                );
                this.userRoles.canUpdateClient = user.roles.some(
                    (role) => role.name === 'update_client'
                );
                this.userRoles.canReadFournisseur = user.roles.some(
                    (role) => role.name === 'read_fournisseur'
                );
                this.userRoles.canCreateFournisseur = user.roles.some(
                    (role) => role.name === 'create_fournisseur'
                );
                this.userRoles.canUpdateFournisseur = user.roles.some(
                    (role) => role.name === 'update_fournisseur'
                );
                this.userRoles.canWriteBankInfos = user.roles.some(
                    (role) => role.name === 'write_bank_infos'
                );
                this.userRoles.canUpdateSiret = user.roles.some(
                    (role) => role.name === 'update_siret'
                );
                this.userRoles.canUpdateParticulier = user.roles.some(
                    (role) => role.name === 'write_particulier'
                );
                this.userRoles.canCreateMkgt = user.roles.some(
                    (role) => role.name === 'creation_mkgt'
                );
                this.userRoles.canCreateOdoo = user.roles.some(
                    (role) => role.name === 'creation_odoo'
                );
                this.userRoles.canCreateGpi = user.roles.some(
                    (role) => role.name === 'creation_gpi'
                );
                this.userRoles.canCreateDashdoc = user.roles.some(
                    (role) => role.name === 'creation_dashdoc'
                );
            }
        });
    }

    getCanUpdate(type?: string, status?: string): boolean {
        if (!type || !status) {
            return false;
        }
        const key = `${type}_${status}`;

        switch (key) {
            case 'client_professional':
                return this.userRoles.canUpdateClient;
            case 'supplier_professional':
                return this.userRoles.canUpdateFournisseur;
            case 'client_particulier':
                return this.userRoles.canUpdateParticulier;
            default:
                return false;
        }
    }

    get isAdmin(): boolean {
        return this.userRoles.isAdmin;
    }

    get canWriteBankInfos(): boolean {
        return this.userRoles.canWriteBankInfos;
    }
}
