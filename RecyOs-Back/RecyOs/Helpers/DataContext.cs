// /** DataContext.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 03/01/2021
//  * Fichier Modifié le : 16/01/2024
//  * Code développé pour le projet : RecyOS.Articles
//  */
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace RecyOs.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;

public class DataContext: DbContext
{
    public ContextSession Session { get; set; }
    protected readonly IConfiguration Configuration;
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserClaim> UserClaims { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EntrepriseBase> EntrepriseBase { get; set; }
    public DbSet<EtablissementFiche> EtablissementFiche { get; set; }
    public DbSet<EtablissementClient> EtablissementClient { get; set; }
    public DbSet<EntrepriseCouverture> EntrepriseCouverture { get; set; }
    public DbSet<ClientEurope> ClientEurope { get; set; }
    public DbSet<EngineSyncStatus> EngineSyncStatus { get; set; }
    public DbSet<FuseNavigationItem> FuseNavigationItems { get; set; }
    public DbSet<FuseNavigationItemBadge> FuseNavigationItemBadges { get; set; }
    public DbSet<DocumentPdf> DocumentPdfs { get; set; }
    public DbSet<DocumentPdfEurope> DocumentPdfEuropes { get; set; }
    public DbSet<TypeDocumentPdf> TypeDocumentPdfs { get; set; }
    public DbSet<CategorieClient> CategorieClients { get; set; }
    public DbSet<EntrepriseNDCover> EntrepriseNDCovers { get; set; }
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<EtablissementClientBusinessUnit> EtablissementClientBusinessUnits { get; set; }
    public DbSet<ClientEuropeBusinessUnit> ClientEuropeBusinessUnits { get; set; }
    public DbSet<Counter> Counters { get; set; }
    public DbSet<Societe> Societes { get; set; }
    public DbSet<BalanceFrance> BalanceFrances { get; set; }
    public DbSet<BalanceEurope> BalanceEuropes { get; set; }
    public DbSet<BalanceParticulier> BalanceParticuliers { get; set; }
    public DbSet<ClientParticulier> ClientParticuliers { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<Commercial> Commerciaux { get; set; }
    public DbSet<FactorClientFranceBu> FactorClientFranceBus { get; set; }
    public DbSet<FactorClientEuropeBu> FactorClientEuropeBus { get; set; }
    public DbSet<MessageMail> MessageMails { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<AccountMoveLine> AccountMoveLines { get; set; }
    public DbSet<TemporaryNdCoverExport> TemporaryNdCoverExports { get; set; }

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    public DataContext(DbContextOptions<DataContext> options,
        IConfiguration configuration,
        IWebHostEnvironment env) : base(options)
    {
        _configuration = configuration;
        _env = env;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Check if the DbContext has already been configured
        if (!optionsBuilder.IsConfigured)
        {
            if (_env == null)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
            }
            else if (_env.IsEnvironment("Test"))
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }
            else
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
            }
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<User>();

        modelBuilder.Entity<EntrepriseBase>()
            .HasIndex(c => c.Siren)
            .IsUnique();
        
        modelBuilder.Entity<EntrepriseCouverture>()
            .HasIndex(c => c.Siren)
            .IsUnique();
        
        modelBuilder.Entity<EntrepriseCouverture>()
            .HasIndex(c => c.EhId)
            .IsUnique();

        modelBuilder.Entity<EntrepriseCouverture>()
            .HasOne(e => e.EntrepriseBase)
            .WithOne(e => e.EntrepriseCouverture)
            .HasForeignKey<EntrepriseCouverture>(e => e.Siren)
            .HasPrincipalKey<EntrepriseBase>(e => e.Siren)
            .IsRequired(false);
        
         modelBuilder.Entity<EtablissementClient>(entity => {
            entity.ToTable( "EtablissementClient" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("EtablissementClientHistory");
                }));
        });

        modelBuilder.Entity<EtablissementClient>()
            .HasOne(e => e.EntrepriseBase)
            .WithMany(e => e.EtablissementClient)
            .HasForeignKey(e => e.Siren)
            .HasPrincipalKey(e => e.Siren)
            .IsRequired(false);

        modelBuilder.Entity<EtablissementClient>()
            .HasOne(e => e.EtablissementFiche)
            .WithOne(e => e.EtablissementClient)
            .HasForeignKey<EtablissementClient>(e => e.Siret)
            .HasPrincipalKey<EtablissementFiche>(e => e.Siret)
            .IsRequired(false);
        
        modelBuilder.Entity<EntrepriseNDCover>()
            .HasIndex(c => c.Siren)
            .IsUnique();
        
        modelBuilder.Entity<EntrepriseNDCover>()
            .HasIndex(c => c.EhId)
            .IsUnique();
        
        modelBuilder.Entity<EntrepriseNDCover>()
            .HasOne(e => e.EntrepriseBase)
            .WithOne(e => e.EntrepriseNDCover)
            .HasForeignKey<EntrepriseNDCover>(e => e.Siren)
            .HasPrincipalKey<EntrepriseBase>(e => e.Siren)
            .IsRequired(false);
        
        modelBuilder.Entity<EtablissementFiche>()
            .HasIndex(c => c.Siret)
            .IsUnique();

        modelBuilder.Entity<EtablissementClient>()
            .HasIndex(c => c.Siret)
            .IsUnique();

        modelBuilder.Entity<EtablissementClient>()
            .HasIndex(c => c.CodeMkgt)
            .IsUnique();

        modelBuilder.Entity<ClientEurope>(entity => {
            entity.ToTable( "ClientEurope" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("ClientEuropeHistory");
                }));
        });

        modelBuilder.Entity<ClientEurope>()
            .HasIndex(c => c.Vat)
            .IsUnique();

        modelBuilder.Entity<ClientParticulier>(entity => {
            entity.ToTable( "ClientParticuliers" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("ClientParticuliersHistory");
                }));
        });
        
        modelBuilder.Entity<EngineSyncStatus>()
            .HasIndex(c => c.ModuleName)
            .IsUnique();
        
        // Configure TaskStatus to string conversion
        var statusConverter = new ValueConverter<TaskStatus, string>(
            v => v.ToString(),                 // Convert enum to string for database
            v => (TaskStatus)Enum.Parse(typeof(TaskStatus), v) // Convert string to enum for app
        );

        modelBuilder.Entity<MessageMail>()
            .Property(m => m.Status)
            .HasConversion(statusConverter)
            .HasMaxLength(15); // Ensure database field size is consistent

        modelBuilder.Entity<EngineSyncStatus>()
            .HasData(
                new EngineSyncStatus
                {
                    Id = 1,
                    ModuleName = "MkgtClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 2,
                    ModuleName = "OdooClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 3,
                    ModuleName = "OdooEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 4,
                    ModuleName = "MkgtEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 5,
                    ModuleName = "GpiEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,
                    ModuleEnabled = false,
                },
                new EngineSyncStatus
                {
                    Id = 6,
                    ModuleName = "GpiClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true, // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,
                    ModuleEnabled = false,
                },
                new EngineSyncStatus
                {
                    Id = 7,
                    ModuleName = "HubSpotEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,     // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans HubSpot n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 8,
                    ModuleName = "HubSpotClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,     // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans HubSpot n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 9,
                    ModuleName = "DashDocClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans Dashdoc n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 10,
                    ModuleName = "DashDocEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans Dashdoc n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 11,
                    ModuleName = "MkgtClientParticulierModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 12,
                    ModuleName = "OdooClientParticulierModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = false,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                },
                new EngineSyncStatus
                {
                    Id = 13,
                    ModuleName = "MkgtShipperClientParticulierModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                    IdCreatedByDest = true
                },
                new EngineSyncStatus
                {
                    Id = 14,
                    ModuleName = "MkgtShipperClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                    IdCreatedByDest = true
                },
                new EngineSyncStatus
                {
                    Id = 15,
                    ModuleName = "MkgtShipperEuropeClientModule",
                    LastCreate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    SyncUpd = true,    // Le moteur s'occupe de répliquer les mises à jour
                    SyncCre = true,   // La création dans odoo n'est pas faite par le moteur
                    ModuleEnabled = true,
                    IdCreatedByDest = true
                }
            );
        
        modelBuilder.Entity<User>()
            .HasData(
                new User{
                    Id = 1,
                    UserName = "Benjamin ROLLIN",
                    Password = hasher.HashPassword(null, "RecyOs"),
                    FirstName = "Benjamin",
                    LastName = "ROLLIN",
                    Email = "b.rollin@recygroup.fr",
                    Avatar = "assets/images/avatars/benjamin-rollin.jpg",
                    Status = "online"
                });
        
        modelBuilder.Entity<Role>()
            .HasData(
                new Role{Id = 500, Name = "super_admin"},
                new Role{Id = 1, Name = "admin"},
                new Role{Id = 2, Name = "user"},
                new Role{Id = 3, Name = "operator"},
                new Role{Id = 4, Name = "instalator"},
                new Role{Id = 5, Name = "menu_clients"},
                new Role{Id = 6, Name = "create_client"},
                new Role{Id = 7, Name = "update_client"},
                new Role{Id = 8, Name = "read_client"},
                new Role{Id = 9, Name = "dashboards"},
                new Role{Id = 10, Name = "dashboard_customer"},
                new Role{Id = 11, Name = "add_PDF"},
                new Role{Id = 12, Name = "update_PDF"},
                new Role{Id = 13, Name = "delete_PDF"},
                new Role{Id = 14, Name = "download_PDF"},
                new Role{Id = 15, Name = "gpi_sync"},
                new Role{Id = 16, Name = "menu_fournisseurs"},
                new Role{Id = 17, Name = "create_fournisseur"},
                new Role{Id = 18, Name = "update_fournisseur"},
                new Role{Id = 19, Name = "read_fournisseur"},
                new Role{Id = 20, Name = "write_bank_infos"},
                new Role{Id = 21, Name = "create_commercial"},
                new Role{Id = 22, Name = "update_commercial"},
                new Role{Id = 23, Name = "delete_commercial"},
                new Role{Id = 24, Name = "update_siret"},
                new Role{Id = 25, Name = "read_particulier"},
                new Role{Id = 26, Name = "write_particulier"},
                new Role{Id = 27, Name = "creation_mkgt"},
                new Role{Id = 28, Name = "creation_odoo"},
                new Role{Id = 29, Name = "creation_gpi"},
                new Role{Id = 30, Name = "creation_dashdoc"},
                new Role{Id = 31, Name = "finance"},
                new Role{Id = 32, Name = "balance_clients"},
                new Role{Id = 33, Name = "create_group"},
                new Role{Id = 34, Name = "update_group"},
                new Role{Id = 35, Name = "delete_group"},
                new Role{Id = 36, Name = "responsable_bu"},
                new Role{Id = 37, Name = "compta"},
                new Role{Id = 38, Name = "commercial"},
                new Role{Id = 39, Name = "read_users"}
        );
        
        modelBuilder.Entity<UserRole>().HasKey("UserId", "RoleId");
        
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole() {UserId = 1, RoleId = 500},
            new UserRole() {UserId = 1, RoleId = 1},
            new UserRole() {UserId = 1, RoleId = 2},
            new UserRole() {UserId = 1, RoleId = 3},
            new UserRole() {UserId = 1, RoleId = 4},
            new UserRole() {UserId = 1, RoleId = 5},
            new UserRole() {UserId = 1, RoleId = 6},
            new UserRole() {UserId = 1, RoleId = 7},
            new UserRole() {UserId = 1, RoleId = 8},
            new UserRole() {UserId = 1, RoleId = 9},
            new UserRole() {UserId = 1, RoleId = 10},
            new UserRole() {UserId = 1, RoleId = 11},
            new UserRole() {UserId = 1, RoleId = 12},
            new UserRole() {UserId = 1, RoleId = 13},
            new UserRole() {UserId = 1, RoleId = 14},
            new UserRole() {UserId = 1, RoleId = 15},
            new UserRole() {UserId = 1, RoleId = 16},
            new UserRole() {UserId = 1, RoleId = 17},
            new UserRole() {UserId = 1, RoleId = 18},
            new UserRole() {UserId = 1, RoleId = 19},
            new UserRole() {UserId = 1, RoleId = 20},
            new UserRole() {UserId = 1, RoleId = 21},
            new UserRole() {UserId = 1, RoleId = 22},
            new UserRole() {UserId = 1, RoleId = 23},
            new UserRole() {UserId = 1, RoleId = 24},
            new UserRole() {UserId = 1, RoleId = 25},
            new UserRole() {UserId = 1, RoleId = 26},
            new UserRole() {UserId = 1, RoleId = 27},
            new UserRole() {UserId = 1, RoleId = 28},
            new UserRole() {UserId = 1, RoleId = 29},
            new UserRole() {UserId = 1, RoleId = 30},
            new UserRole() {UserId = 1, RoleId = 31},
            new UserRole() {UserId = 1, RoleId = 32},
            new UserRole() {UserId = 1, RoleId = 33},
            new UserRole() {UserId = 1, RoleId = 34},
            new UserRole() {UserId = 1, RoleId = 35},
            new UserRole() {UserId = 1, RoleId = 36},
            new UserRole() {UserId = 1, RoleId = 37},
            new UserRole() {UserId = 1, RoleId = 38},
            new UserRole() {UserId = 1, RoleId = 39}
        );
        
        modelBuilder.Entity<FuseNavigationItemBadge>().HasData(
            new FuseNavigationItemBadge()
            {
                Id = 1,
                Title = "v0.3",
                Classes = "px-2 bg-yellow-300 text-black rounded-full",
            }
        );

        modelBuilder.Entity<CategorieClient>().HasData(
            new CategorieClient()
            {
                Id = 1,
                CategorieLabel = "N/A"
            }
        );

        modelBuilder.Entity<TypeDocumentPdf>().HasData(
            new TypeDocumentPdf()
            {
                Id = 1,
                Label = "KBIS"
            }
            
        );

        modelBuilder.Entity<FuseNavigationItem>().HasData(
            new FuseNavigationItem()
            {
                Id = 1,
                MenuId = 1,
                MenuPosition = 0,
                Title = "Administration",
                Subtitle = "Panel d'administration",
                Type = "collapsable",
                Icon = "mat_solid:settings",
                RoleId = 500
            },
            new FuseNavigationItem()
            {
                Id = 2,
                MenuId = 0, 
                MenuPosition = 2,
                Title = "Utilisateurs",
                Type = "basic",
                Icon = "heroicons_solid:users",
                RoleId = 500,
                ParentId = 1,
                Link = "/administrator/users"
            },
            new FuseNavigationItem()
            {
                Id = 7,
                MenuId = 0,
                MenuPosition = 1,
                Title = "Paramètres",
                Type = "basic",
                Icon = "heroicons_solid:cog",
                RoleId = 500,
                ParentId = 1,
                Link = "/administrator/settings"
            },
            new FuseNavigationItem()
            {
                Id = 3,
                MenuId = 1,
                MenuPosition = 1,
                Type = "divider",
                RoleId = 1,
            },
            new FuseNavigationItem()
            {
                Id = 100,
                MenuId = 1, 
                MenuPosition = 2,
                Title = "Dashboard",
                Subtitle = "Retrouvez tous vos tableaux de bords",
                Type = "group",
                Icon = "heroicons_outline:home",
                RoleId = 9,
                ParentId = null,
                Link = null
            },
            new FuseNavigationItem()
            {
                Id = 101,
                MenuId = 0, 
                MenuPosition = 1,
                Title = "Fichier client",
                Subtitle = null,
                Type = "basic",
                Icon = "heroicons_outline:chart-pie",
                RoleId = 10,
                ParentId = 100,
                Link = "/dashboards/customer"
            },
            new FuseNavigationItem()
            {
                Id = 102,
                MenuId = 1,
                MenuPosition = 2,
                Type = "divider",
                RoleId = 9,
            },
            new FuseNavigationItem()
            {
                Id = 200,
                MenuId = 1,
                MenuPosition = 3,
                Title = "Referentiels",
                Type = "group",
                RoleId = 8,
            },
            new FuseNavigationItem()
            {
                Id = 201,
                MenuId = 1,
                MenuPosition = 4,
                Title = "Clients",
                Subtitle = "Base clients Recygroup",
                Type = "collapsable",
                Icon = "mat_solid:account_box",
                RoleId = 8
            },
            new FuseNavigationItem()
            {
                Id = 202,
                MenuId = 0,
                MenuPosition = 1,
                Title = "France",
                Type = "basic",
                Icon = "mat_solid:business",
                RoleId = 8,
                ParentId = 201,
                Link = "/customers/businesses"
            },
            new FuseNavigationItem()
            {
                Id = 204,
                MenuId = 0,
                MenuPosition = 2,
                Title = "Europe",
                Type = "basic",
                Icon = "mat_solid:euro",
                RoleId = 8,
                ParentId = 201,
                Link = "/customers/europe"
            },
            new FuseNavigationItem()
            {
                 Id = 205,
                 MenuId = 0,
                 MenuPosition = 3,
                 Title = "Particuliers",
                 Type = "basic",
                 Icon = "mat_solid:person",
                 RoleId = 8,
                 ParentId = 201,
                 Link = "/customers/particuliers"
            },
            new FuseNavigationItem()
            {
                Id = 299,
                MenuId = 0,
                MenuPosition = 100,
                Title = "Opérations",
                Type = "basic",
                Icon = "mat_solid:settings",
                RoleId = 8,
                ParentId = 201,
                Link = "/customers/operations"
            },
            new FuseNavigationItem()
            {
                Id = 301,
                MenuId = 1,
                MenuPosition = 5,
                Title = "Fournisseurs",
                Subtitle = "Base fournisseurs Recygroup",
                Type = "collapsable",
                Icon = "mat_solid:store",
                RoleId = 16
            },
            new FuseNavigationItem()
            {
                Id = 302,
                MenuId = 0,
                MenuPosition = 1,
                Title = "France",
                Type = "basic",
                Icon = "mat_solid:business",
                RoleId = 16,
                ParentId = 301,
                Link = "/suppliers/businesses"
            },
            new FuseNavigationItem()
            {
                Id = 304,
                MenuId = 0,
                MenuPosition = 2,
                Title = "Europe",
                Type = "basic",
                Icon = "mat_solid:euro",
                RoleId = 16,
                ParentId = 301,
                Link = "/suppliers/europe"
            },
            new FuseNavigationItem()
            {
                Id = 203,
                MenuId = 1,
                MenuPosition = 69,
                Type = "divider",
                RoleId = 8,
            },
            new FuseNavigationItem()
            {
                Id = 4,
                MenuId = 1,
                MenuPosition = 100,
                Type = "group",
                RoleId = 2,
                Title = "Documentation",
                Icon = "heroicons_outline:support",
                Subtitle = "Tout ce que vous devez savoir sur RecyOs"
            },
            new FuseNavigationItem()
            {
                Id = 5,
                MenuId = 0,
                MenuPosition = 1,
                Type = "basic",
                RoleId = 2,
                ParentId = 4,
                Link = "/changelog",
                Title = "Changelog",
                Icon = "heroicons_outline:speakerphone",
                BadgeId = 1
            },
            new FuseNavigationItem()
             {
                 Id = 6,
                 MenuId = 1,
                 MenuPosition = 70,
                 Title = "Paramètres",
                 Type = "group",
                 RoleId = 1,
             },
             new FuseNavigationItem()
             {
                 Id = 601,
                 MenuId = 0,
                 MenuPosition = 1,
                 Title = "Commerciaux",
                 Type = "basic",
                 Icon = "mat_outline:people",
                 RoleId = 1,
                 ParentId = 6,
                 Link = "/commerciaux"
             },
             new FuseNavigationItem()
             {
                 Id = 602,
                 MenuId = 0,
                 MenuPosition = 2,
                 Title = "Groupes",
                 Type = "basic",
                 Icon = "mat_solid:corporate_fare",
                 RoleId = 1,
                 ParentId = 6,
                 Link = "/groupes"
             },
             new FuseNavigationItem()
             {
                 Id = 603,
                 MenuId = 0,
                 MenuPosition = 3,
                 Title = "Sociétés",
                 Type = "basic",
                 Icon = "mat_outline:business_center",
                 RoleId = 1,
                 ParentId = 6,
                 Link = "/societes"
             },
             new FuseNavigationItem()
             {
                 Id = 403,
                 MenuId = 1,
                 MenuPosition = 99,
                 Type = "divider",
                 RoleId = 9,
             },
             new FuseNavigationItem()
             {
                 Id = 8,
                 MenuId = 1,
                 MenuPosition = 30,
                 Title = "Finances",
                 Type = "collapsable",
                 Icon = "mat_outline:attach_money",
                 RoleId = 31,
                 Link = "/finance"
             },
             new FuseNavigationItem()
             {
                 Id = 801,
                 MenuId = 0,
                 MenuPosition = 2,
                 Title = "Balances Clients Français",
                 Type = "basic",
                 Icon = "mat_outline:account_balance",
                 RoleId = 32,
                 ParentId = 8,
                 Link = "/finance/balance-clients-france"
             },
             new FuseNavigationItem()
             {
                 Id = 802,
                 MenuId = 1,
                 MenuPosition = 29,
                 Type = "divider",
                 RoleId = 31,
             },
             new FuseNavigationItem()
             {
                 Id = 803,
                 MenuId = 0,
                 MenuPosition = 3,
                 Title = "Balances Clients Européens",
                 Type = "basic",
                 Icon = "mat_outline:account_balance",
                 RoleId = 32,
                 ParentId = 8,
                 Link = "/finance/balance-clients-europe"
             },
             new FuseNavigationItem()
             {
                 Id = 804,
                 MenuId = 0,
                 MenuPosition = 4,
                 Title = "Balances Clients Particuliers",
                 Type = "basic",
                 Icon = "mat_outline:account_balance",
                 RoleId = 32,
                 ParentId = 8,
                 Link = "/finance/balance-clients-particuliers"
             }
        );
        
        modelBuilder.Entity<BusinessUnit>().HasData(
            new BusinessUnit()
            {
                Id = 1,
                Libelle = "Recynov"
            },
            new BusinessUnit()
            {
                Id = 2,
                Libelle = "Transport"
            },
            new BusinessUnit()
            {
                Id = 3,
                Libelle = "Recynov Services"
            },
            new BusinessUnit()
            {
                Id = 4,
                Libelle = "Recynergies"
            },
            new BusinessUnit()
            {
                Id = 5,
                Libelle = "Noreval"
            }
        );
        modelBuilder.Entity<EtablissementClientBusinessUnit>().HasKey("BusinessUnitId", "ClientId");
        modelBuilder.Entity<ClientEuropeBusinessUnit>().HasKey("BusinessUnitId", "ClientId");

        modelBuilder.Entity<Counter>().HasData(
            new Counter()
            {
                Id = 1,
                Name = "Client_gpi",
                Value = 260,
                Description = "Compteur de clients gpi"
            },
            new Counter()
            {
                Id = 2,
                Name = "Fourniseurs_gpi",
                Value = 1000,
                Description = "Compteur de fournisseurs gpi"
            }
        );
        
        modelBuilder.Entity<Societe>().HasData(
            new Societe()
            {
                Id = 1,
                Nom = "Recygroup",
                IdOdoo = "1"
            },
            new Societe()
            {
                Id = 2,
                Nom = "BP Trans",
                IdOdoo = "1346"
            },
            new Societe()
            {
                Id = 3,
                Nom = "Paul Pacquet",
                IdOdoo = "1345"
            },
            new Societe()
            {
                Id = 4,
                Nom = "Valotrans",
                IdOdoo = "3"
            },
            new Societe()
            {
                Id = 5,
                Nom = "Recynov",
                IdOdoo = "1344"
            },
            new Societe()
            {
                Id = 6,
                Nom = "Recynergies",
                IdOdoo = "1342"
            },
            new Societe()
            {
                Id = 7,
                Nom = "Recynov Services",
                IdOdoo = "1351"
            },
            new Societe()
            {
                Id = 8,
                Nom = "Noreval",
                IdOdoo = "1354"
            }
        );

        modelBuilder.Entity<Group>().HasData(
            new Group()
            {
                Id = 1,
                Name = "NA"
            }
        );

        modelBuilder.Entity<BalanceFrance>()
            .HasKey(bf => new { bf.ClientId, bf.SocieteId });

        modelBuilder.Entity<BalanceFrance>(entity => {
            entity.ToTable( "BalanceFrances" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("BalanceFrancesHistory");
                }));
        });

        modelBuilder.Entity<BalanceFrance>()
            .Property(b => b.Montant)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BalanceFrance>()
            .HasIndex(i => new { i.ClientId, i.SocieteId })
            .IsUnique();
        
        modelBuilder.Entity<BalanceEurope>()
            .HasKey(be => new { be.ClientId, be.SocieteId });

        modelBuilder.Entity<BalanceEurope>(entity => {
            entity.ToTable( "BalanceEuropes" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("BalanceEuropesHistory");
                }));
        });
        modelBuilder.Entity<BalanceEurope>()
            .Property(b => b.Montant)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BalanceEurope>()
            .HasIndex(i => new { i.ClientId, i.SocieteId })
            .IsUnique();

        modelBuilder.Entity<BalanceParticulier>()
            .HasKey(bf => new { bf.ClientId, bf.SocieteId });

        modelBuilder.Entity<BalanceParticulier>(entity => {
            entity.ToTable( "BalanceParticuliers" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("BalanceParticuliersHistory");
                }));
        });
        modelBuilder.Entity<BalanceParticulier>()
            .Property(b => b.Montant)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BalanceParticulier>()
            .HasIndex(i => new { i.ClientId, i.SocieteId })
            .IsUnique();

        modelBuilder.Entity<FactorClientFranceBu>()
            .HasKey(f => new { f.IdClient, f.IdBu });

        modelBuilder.Entity<FactorClientEuropeBu>()
            .HasKey(f => new { f.IdClient, f.IdBu });

        modelBuilder.Entity<FactorClientFranceBu>()
            .HasIndex(f => new { f.IdClient, f.IdBu })
            .IsUnique();

        modelBuilder.Entity<FactorClientEuropeBu>()
            .HasIndex(f => new { f.IdClient, f.IdBu })
            .IsUnique();

        modelBuilder.Entity<FactorClientFranceBu>()
            .ToTable("FactorClientFranceBus");

        modelBuilder.Entity<FactorClientEuropeBu>()
            .ToTable("FactorClientEuropeBus");

        modelBuilder.Entity<Group>()
            .HasIndex(g => g.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Commercial)
            .WithOne(c => c.User)
            .HasForeignKey<Commercial>(c => c.UserId)
            .IsRequired(false);

        modelBuilder.Entity<Notification>(entity => {
            entity.ToTable( "Notification" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("NotificationHistory");
                }));
        });

        modelBuilder.Entity<MessageMail>(entity => {
            entity.ToTable( "MessageMails" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("MessageMailsHistory");
                }));
        });

        modelBuilder.Entity<Parameter>(entity => {
            entity.ToTable( "Parameters" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("ParametersHistory");
                }));
        });

        modelBuilder.Entity<Commercial>(entity => {
            entity.ToTable( "Commercial" , tb => tb.IsTemporal(TemporalTableBuilder => {
                TemporalTableBuilder.HasPeriodStart("PeriodStart").HasColumnName("PeriodStart");
                TemporalTableBuilder.HasPeriodEnd("PeriodEnd").HasColumnName("PeriodEnd");
                TemporalTableBuilder.UseHistoryTable("CommercialHistory");
            }));
        });
    }
}