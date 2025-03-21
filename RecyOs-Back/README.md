# Projet Recyos Serveur Backend
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)
![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/github%20actions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white)

![Coverage](https://img.shields.io/badge/Coverage-56%25-yellow)
[![wakatime](https://wakatime.com/badge/github/rollinbe/RecyOs.svg)](https://wakatime.com/badge/github/rollinbe/RecyOs)

## Table des matières

1. [Introduction](#introduction)
2. [Prérequis](#prérequis)
3. [Installation](#installation)
4. [Utilisation](#utilisation)
5. [Documentation](#documentation)
6. [Contribuer](#contribuer)
7. [Licence](#licence)
8. [Contact](#contact)

## Introduction

Recyos est un projet basé sur le framework ASP.NET Core, visant à fournir une solution de référentiel unique,
Il gére également la sychronisation des données du référentiel avec les logiciels des différentes sociétés du groupe

## Prérequis

Avant de commencer, assurez-vous de disposer des éléments suivants :

- .NET SDK (version recommandée 8.0 ou supérieure)
- Une base de donnée (tésté uniquement sur SQL server)
- Un éditeur de texte ou un environnement de développement intégré (IDE) compatible, tel que Visual Studio, Visual Studio Code ou Rider.

## Installation

Pour installer le projet Recyos, suivez les étapes ci-dessous :

### 1. Clonez le dépôt Git :

```
git clone https://github.com/rollinbe/RecyOs.git
```

### 2. Naviguez dans le dossier du projet :

```
cd recyos
```

### 3. Restaurez les packages NuGet :

```
dotnet restore
```

### 4. Perssonalisez le fichier de configuation (appsettings.json) :

- Base de donnée Master Data
- Base de donnée MKGT (Si vous activez les modules)
- Clé API pappers
- Clé API Odoo
- Niveaux de log
- Certificats service WEB

### **5. Construisez le projet :**
```
dotnet build
```

## **Paramettrage de l'application**
Les paramètres de l'application se définissent dans le fichier appsettings.json. Vous trouverez un exemple de fichier appsettings.json.exemple

## Création DB et fixtures

pour créer la base de données vous pouvez le faire grace à Entity Framework. Pour ce faire vous deverz créer une migration :
```
dotnet ef migrations add InitialCreate --startup-project RecyOs
```
Puis pour appliquer cette migration sur le SGBD vous pouvez le faire via la commande :
```
dotnet ef database update --startup-project RecyOs
```

## Utilisation

Pour exécuter le projet Recyos, utilisez la commande suivante :
```
dotnet run
```

Le site web sera disponible à l'adresse http://localhost:5000 ou https://localhost:5001.

## Documentation

La documentation complète du projet Recyos est disponible sur [notre site web](https://it.recygroup.fr).

## Contribuer

Les contributions au projet Recyos sont les bienvenues. Pour contribuer, veuillez suivre ces étapes :

1. Forkez le dépôt sur GitHub
2. Clonez le dépôt forké sur votre machine
3. Créez une nouvelle branche pour votre fonctionnalité
4. Faites vos modifications et assurez-vous de les tester
5. Faites un commit avec un message descriptif
6. Envoyez vos modifications sur votre fork
7. Créez une pull request sur le dépôt principal

## Licence

Ce projet est sous License privée repordution interdite sans autorisation explicite

## Contact

Si vous avez des questions, des problèmes ou des suggestions, n'hésitez pas à nous contacter :

Nous nous réjouissons de vos commentaires et de votre soutien.
