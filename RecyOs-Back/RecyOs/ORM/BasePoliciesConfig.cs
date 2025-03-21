using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;

namespace RecyOs.ORM;

public static class BasePoliciesConfig
{
    public static void RegisterPolicies(this AuthorizationOptions opt)
    {
        opt.AddPolicy("All", policy => policy.RequireRole(Roles.Admin, Roles.Operator, Roles.Installator, Roles.User));
        opt.AddPolicy("AdminOnly", policy => policy.RequireRole(Roles.Admin));
        opt.AddPolicy("OperatorOnly", policy => policy.RequireRole(Roles.Admin, Roles.Operator));
        opt.AddPolicy("InstallatorOnly", policy => policy.RequireRole(Roles.Admin, Roles.Installator));
        opt.AddPolicy("UserOnly", policy => policy.RequireRole(Roles.Admin, Roles.User));
        opt.AddPolicy("MenuClient", policy => policy.RequireRole(Roles.Admin, Roles.MenuClient));
        opt.AddPolicy("CreateClient", policy => policy.RequireRole(Roles.CreateClient));
        opt.AddPolicy("UpdateClient", policy => policy.RequireRole(Roles.UpdateClient));
        opt.AddPolicy("ReadClient", policy => policy.RequireRole(Roles.ReadClient));
        opt.AddPolicy("Dashboard", policy => policy.RequireRole(Roles.Dashboard));
        opt.AddPolicy("DashboardClient", policy => policy.RequireRole(Roles.DashboardClient));
        opt.AddPolicy("AddPdf", policy => policy.RequireRole(Roles.AddPdf));
        opt.AddPolicy("UpdatePdf", policy => policy.RequireRole(Roles.UpdatePdf));
        opt.AddPolicy("DeletePdf", policy => policy.RequireRole(Roles.DeletePdf));
        opt.AddPolicy("DownloadPdf", policy => policy.RequireRole(Roles.DownloadPdf));
        opt.AddPolicy("GpiSync", policy => policy.RequireRole(Roles.GpiSync));
        opt.AddPolicy("MenuFournisseur", policy => policy.RequireRole(Roles.MenuFournisseur));
        opt.AddPolicy("CreateFournisseur", policy => policy.RequireRole(Roles.CreateFournisseur));
        opt.AddPolicy("UpdateFournisseur", policy => policy.RequireRole(Roles.UpdateFournisseur));
        opt.AddPolicy("ReadFournisseur", policy => policy.RequireRole(Roles.ReadFournisseur));
        opt.AddPolicy("CreateCommercial", policy => policy.RequireRole(Roles.CreateCommercial));
        opt.AddPolicy("UpdateCommercial", policy => policy.RequireRole(Roles.UpdateCommercial));
        opt.AddPolicy("DeleteCommercial", policy => policy.RequireRole(Roles.DeleteCommercial));
        opt.AddPolicy("WriteBankInfos", policy => policy.RequireRole(Roles.WriteBankInfos));
        opt.AddPolicy("UpdateSiret", policy => policy.RequireRole(Roles.UpdateSiret));
        opt.AddPolicy("ReadParticulier", policy => policy.RequireRole(Roles.ReadParticulier));
        opt.AddPolicy("WriteParticulier", policy => policy.RequireRole(Roles.WriteParticulier));
        opt.AddPolicy("CreationMkgt", policy => policy.RequireRole(Roles.CreationMkgt));
        opt.AddPolicy("CreationOdoo", policy => policy.RequireRole(Roles.CreationOdoo));
        opt.AddPolicy("CreationGpi", policy => policy.RequireRole(Roles.CreationGpi));
        opt.AddPolicy("CreationDashdoc", policy => policy.RequireRole(Roles.CreationDashdoc));
        opt.AddPolicy("CreateGroup", policy => policy.RequireRole(Roles.CreateGroup));
        opt.AddPolicy("UpdateGroup", policy => policy.RequireRole(Roles.UpdateGroup));
        opt.AddPolicy("DeleteGroup", policy => policy.RequireRole(Roles.DeleteGroup));
        opt.AddPolicy("ReadUsers", policy => policy.RequireRole(Roles.ReadUsers));
        opt.AddPolicy("SuperAdmin", policy => policy.RequireRole(Roles.SuperAdmin));
    }
}