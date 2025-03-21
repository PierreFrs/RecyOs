import {ChangeDetectionStrategy, Component} from "@angular/core";

@Component({
    selector       : 'changelog',
    templateUrl    : './changelog.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChangelogComponent {
    changelog: any[] = [
        {
            version    : 'v0.3',
            releaseDate: '15 mars 2025',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['fonctionnalité pour afficher des notifications',
                        'fonctionnalité pour la création de nouveaux utilisateurs',
                        'fonctionnalité pour la suppression d\'un utilisateur',
                        'Interface pour la consultation des balances clients',
                        'Gestion des groupements de clients',
                        'Gestion des entitées du groupe'
                    ]
                },
                {
                    type: 'Corrections',
                    list: ['Correction d\'un problème de création des clients néerlandais',
                        'Correction d\'un problème de création des clients particuliers dans Odoo (Recynov Services)'
                    ]
                },
                {
                    type: 'Modifications',
                    list: ['Ajout de droits individuels pour la création des fiches dans les différents ERP',
                        'Ajout d\'un droit super_admin pour la gestion des utilisateurs',
                        'Ajout d\'un droit compta pour la visualisation de toutes les balances',
                        'Ajout d\'un droit responsable_bu pour la visualisation des balances par BU',
                        'Ajout d\'un lien entre les utilisateurs et les commerciaux'
                    ],
                },
                {
                    type: 'Moteur',
                    list: ['Synchronisation quotidienne des balances clients avec Odoo',
                           'Ajout d\'une fonctionnalité d\'envoi d\'emails',
                        'Ajout de tables temporelles pour historiser les modifications sur les fiches clients, balances',
                    'Attachement des utilisateurs à une société du groupe']
                }
            ]
        },
        {
            version    : 'v0.2.7',
            releaseDate: '18 décembre 2024',
            changes    : [
                {
                    type: 'Modifications',
                    list: ['Migration vers des cookies HTTPs sécurisés',
                        'Les fiches clients ne sont créées dans Hubspot que si elles sont créées dans MKGT',
                    ],
                },
                {
                    type: 'Moteur',
                    list: ['Mise à jour des librairies utilisés par le back-end']
                }
            ]
        },
        {
            version    : 'v0.2.6',
            releaseDate: '9 décembre 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Gestion des codes shippers pour la liaison Dashdoc Mkgt',
                        'Détection et alerte des doublons de code MKGT'
                    ]
                },
            ]
        },
        // 'v0.2.5'
        {
            version    : 'v0.2.5',
            releaseDate: '13 novembre 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Gestion des clients particuliers'
                    ]
                },
                {
                    type: 'Modifications',
                    list: ['Synchronisation du commercial, APE, SIRET, TVA sur les fiches chantiers de MKGT',
                        'Prise en charge des nouveaux formats de fichiers Allianz Trade']
                },
                {
                    type: 'Corrections',
                    list: ['Correction problèmes de synchronisation du pays des clients européens avec Dashdoc',]
                },
                {
                    type: 'Moteur',
                    list: ['Synchronisation des clients particuliers avec MKGT',
                        'Synchronisation des clients particuliers avec Odoo',]
                }
            ]
        },
        // 'v0.2.4'
        {
            version    : 'v0.2.4',
            releaseDate: '8 octobre 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Gestion des fiches clients dans DashDoc',
                        'Recherche par identifiant Dashdoc'
                    ]
                },
                {
                    type: 'Modifications',
                    list: []
                },
                {
                    type: 'Corrections',
                    list: ['Le code MKGT pouvait être en minuscules dans certains cas',
                        'Force le navigateur à recharger les fichiers CSS et JS lors de la mise à jour de RecyOs',]
                },
                {
                    type: 'Moteur',
                    list: ['Stockage du paramétrage en base de données',
                        'Synchronisation des clients français avec Dashdoc',
                        'Synchronisation des clients européens avec Dashdoc',]
                }
            ]
        },
        // 'v0.2.3'
        {
            version    : 'v0.2.3',
            releaseDate: '1 septembre 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Possibilité de mettre à jour le SIRET d\'une entreprise',
                        'Gestion de l\'affacturage des clients par BU',
                        'Export des fichiers de clients au format THEMIS pour les clients affacturés',
                        'Gestion de l\'affacturage dans MKGT, Odoo, GPI'
                    ]
                },
                {
                    type: 'Modifications',
                    list: ['Création de fiche vide si l\'entreprise s\'est opposée à la diffusion de ses données',]
                },
                {
                    type: 'Corrections',
                    list: ['Bugs sur le dashboard des clients',
                        'Correction de problèmes d\'affichage sur les listes clients',
                        'Affichage d\'un message d\'erreur lors de la création d\'un client déjà existant dans la base',
                        'Correction d\'avertissements dans le code source du back-end']
                },
                {
                    type: 'Moteur',
                    list: ['Portage du back-end sur .NET Core 8.0',
                        'Mise à jour des librairies utilisées par le back-end',
                        'Conteneurisation du back-end',
                        'Conteneurisation du front-end',
                        'Mise en place d\'un pipeline de déploiement continu']
                }
            ]
        },
        // 'v0.2.2'
        {
            version    : 'v0.2.2',
            releaseDate: '9 juin 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['État des entreprises sans commerciaux',]
                },
                {
                    type: 'Modifications',
                    list: ['Création systématique de la fiche client dans Odoo lors de la création de la fiche client dans MKGT',]
                },
                {
                    type: 'Corrections',
                    list: ['Correction d\'un bug empêchant la synchronisation du commercial avec MKGT',]
                },
                {
                    type: 'Moteur',
                    list: []
                }
            ]
        },
        // 'v0.2.1'
        {
            version    : 'v0.2.1',
            releaseDate: '25 mars 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Référentiel commerciaux',
                        'Possibilité d\'affecter un commercial à un client français',
                        'Possibilité d\'affecter un commercial à un client européen',
                        ]
                },
                {
                    type: 'Modifications',
                    list: []
                },
                {
                    type: 'Corrections',
                    list: []
                },
                {
                    type: 'Moteur',
                    list: ['Synchronisation des clients français avec Hubspot',
                        'Synchronisation des clients européens avec Hubspot',]
                }
            ]
        },
        // 'v0.2'
        {
            version    : 'v0.2',
            releaseDate: '31 mars 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Référentiel fournisseur européen',
                        'Référentiel fournisseur français',
                        'Balance comptable des clients par société',
                        'Gestion des multiples adresses e-mail pour chaque client (uniquement sur GPI)',]
                },
                {
                    type: 'Modifications',
                    list: ['Réécriture du front-end en modules réutilisables pour la partie fournisseurs',
                        'Suppression des informations IBAN et BIC des clients',
                           'Ajout d\'un droit spécifique pour pouvoir modifier l\'IBAN et le BIC',]
                },
                {
                    type: 'Corrections',
                    list: []
                },
                {
                    type: 'Moteur',
                    list: ['Synchronisation des données fournisseurs avec Odoo',
                        'Synchronisation des données fournisseurs avec MKGT',
                        'Synchronisation des données fournisseurs avec Gpi',]
                }
            ]
        },
        // 'v0.1.7'
        {
            version    : 'v0.1.7',
            releaseDate: '14 février 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Bandeau d\'alerte sur la version test de RecyOs',
                        'Génération des fichiers de demande de couverture non dénommée au format Allianz Trade',
                        'Génération des fichiers pour la synchronisation avec Dartagnan',
                        'Gestion des business units',
                        'Création de fiches clients d\'Artagnan depuis RecyOs']
                },
                {
                    type: 'Modifications',
                    list: ['Remplacement des boutons de création des clients dans les différents logiciels']
                },
                {
                    type: 'Corrections',
                    list: ['Bug dans le module de synchronisation avec Odoo empêchant la création d\'une entreprise hollandaise',
                        'Correction d\'un problème de droits concernant l\'import des couvertures Allianz']
                },
                {
                    type: 'Moteur',
                    list: ['Réécriture des dépôts de données du moteur pour s\'affranchir des procédures stockées. Permet une meilleure maintenabilité du code.',
                        'Désormais, le paramétrage des modules de synchronisation ne se fait plus via les fichiers de configuration mais en base de données',
                        'Augmentation de la couverture des tests unitaires (51%)',
                        'Conteneurisation de l\'application serveur afin d\'automatiser le processus de déploiement des futures versions']
                }
            ]
        },
        // 'v0.1.6'
        {
            version    : 'v0.1.6',
            releaseDate: '8 janvier 2024',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Ajout des informations concernant la couverture ND Cover']
                },
                {
                    type: 'Corrections',
                    list: ['Correction d\'un problème de stockage des fichiers']
                },
                {
                    type: 'Moteur',
                    list: ['Augmentation de la couverture des tests unitaires (47%)']
                }
            ]
        },
        // 'v0.1.5'
        {
            version    : 'v0.1.5',
            releaseDate: '27 décembre 2023',
            changes    : [
                {
                    type: 'Moteur',
                    list: ['Augmentation de la couverture des tests unitaires (28%)',
                        'Préparation pour l\'ajout de ND Cover']
                }
            ]
        },
        // 'v0.1.4'
        {
            version    : 'v0.1.4',
            releaseDate: '26 décembre 2023',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Gestion des codes clients de GPI sur les clients français',
                        'Ajout d\'un mode administrateur sur les fiches clients français',
                        'Gestion des codes clients de GPI sur les clients européens',
                        'Ajout d\'un mode administrateur sur les fiches clients européens']
                },
                {
                    type: 'Modifications',
                    list: []
                },
                {
                    type: 'Corrections',
                    list: []
                },
                {
                    type: 'Moteur',
                    list: ['Mise à niveau des librairies']
                }
            ]
        },
        // 'v0.1.3'
        {
            version    : 'v0.1.3',
            releaseDate: '8 décembre 2023',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Verrouillage des champs dans Odoo pour les clients qui ont été créés depuis RecyOs']
                },
                {
                    type: 'Modifications',
                    list: ['Mise à niveau de la structure des fiches garantie Allianz-Trade pour prendre en compte les nouvelles informations',
                        'Marquage des fiches Odoo comme étant liées à RecyOs']
                },
                {
                    type: 'Corrections',
                    list: ['Affectation de la catégorie N/A pour les fiches créées depuis MKGT',
                        'Correction d\'un bug empêchant la création d\'entreprise non diffusible dans MKGT et Odoo',
                        'Correction d\'un bug empêchant la création de client dans Oo dans certains cas']
                },
                {
                    type: 'Moteur',
                    list: []
                }
            ]
        },
        // 'v0.1.2'
        {
            version    : 'v0.1.2',
            releaseDate: '1 décembre 2023',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Gestion du référentiel client européen',
                        'Possibilité de joindre des fichiers aux fiches clients',
                        'Possibilité aux utilisateurs ayant le droit opérateur d\'importer des clients depuis MKGT si la fiche est valide',
                        'Possibilité aux utilisateurs ayant le droit opérateur de relancer un import en masse MKGT pour importer uniquement les clients qui n\'ont pas pu être importés lors de l\'import initial',
                        'Possibilité aux utilisateurs ayant le droit opérateur d\'importer le fichier Allianz TRADE pour actualiser les données',
                        'Ajout des informations : activité détaillée sur les fiches clients',
                        'Ajout d\'un champ catégorie client obligatoire sur les fiches clients']
                },
                {
                    type: 'Modifications',
                    list: ['Le compte comptable de vente est également mis à jour sur les fiches Odoo',
                        'Simplification de la création de client sur Odoo']
                },
                {
                    type: 'Corrections',
                    list: ['La valeur -1 pouvait être envoyée dans le champ "Code client de MKGT" lors de la création d\'un client dans Odoo',
                        'Lors de l\'importation en masse depuis MKGT, les anciens comptes comptables étaient encore utilisés',
                        'Problèmes d\'affichage divers']
                },
                {
                    type: 'Moteur',
                    list: ['Ajout de la possibilité de stocker des fichiers dans RecyOS.']
                }
            ]
        },
        // 'v0.1.1'
        {
            version    : 'v0.1.1',
            releaseDate: '1 septembre 2023',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Possibilité de changer de mot de passe',
                        'Affichage des informations concernant la couverture du client',
                        'Envoi des conditions de règlement client dans Odoo',
                        'Journal des modifications entre les versions de RecyOs',
                        'Tableau de bord clients',
                        'Liste des clients filtrés par statut',
                        'Liste des demandes de couverture clients filtrés par statut',
                        'Possibilité de créer des clients non diffusibles']
                },
                {
                    type: 'Modifications',
                    list: ['Affichage de la version actuelle de RecyOs dans la barre du navigateur',
                        'La fiche n\'est plus créée automatiquement dans le logiciel tiers lors du clic sur "Créer la fiche dans ...". Dorénavant, il faut cliquer sur le bouton Mettre à jour pour confirmer',
                        'Sélection automatique du compte comptable lors du changement du taux de TVA']
                },
                {
                    type: 'Corrections',
                    list: ['Détection des doublons sur les codes MKGT et demande de choix manuel du code quand il y a doublon',
                        'Correction d\'un problème d\'affichage sur les écrans ayant une résolution inférieure à 1920x1080',
                        'Remplacement des valeurs Null par une chaîne vide dans les champs adresses de MKGT (bug édition de facture)']
                },
                {
                    type: 'Moteur',
                    list: ['Prise en charge des badges sur les éléments du menu',
                        'Mise à niveau des librairies']
                }
            ]
        },
        // 'v0.1'
        {
            version    : 'v0.1',
            releaseDate: '20 juin 2023',
            changes    : [
                {
                    type: 'Ajouts',
                    list: ['Ajout de droits d\'accès aux fonctionnalités',
                        'Ajout d\'un module de gestion des utilisateurs et des droits',
                        'Synchronisation des clients avec Odoo']
                },
                {
                    type: 'Modifications',
                    list: ['La création de client requiert un droit d\'accès',
                        'La modification de client requiert un droit d\'accès']
                },
                {
                    type: 'Corrections',
                    list: ['Problèmes de synchronisation remontés lors des tests']
                }
            ]
        },
        // 'Beta 1'
        {
            version    : 'Beta 1',
            releaseDate: '28 mai 2023',
            changes    : [
                {
                    type: 'Modifications',
                    list: ['Revue des automatismes sur le formulaire client',
                        'La définition de l\'encours client se fait par palier',
                        'Ajout d\'une section pour saisir l\'IBAN du client',
                        'Possibilité de saisir un contact alternatif',
                        'Possibilité de saisir le nom du contact']
                },
                {
                    type: 'Corrections',
                    list: ['Problèmes identifiés lors du premier déploiement']
                }
            ]
        },
        // 'Alpha 1'
        {
            version    : 'Alpha 1',
            releaseDate: '28 avril 2023',
            changes    : [
                {
                    type: 'Première présentation',
                    list: ['Identification de l\'utilisateur avant d\'accéder à l\'outil',
                        'Module clients professionnel',
                        'Synchronisation avec le fichier client MKGT']
                }
            ]
        }
    ];
}
