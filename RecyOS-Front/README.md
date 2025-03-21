# Fuse - Admin template and Starter project for Angular

This project was generated with [Angular CLI](https://github.com/angular/angular-cli)

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

### Default development environment :
Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. <br>

### Build in a production environment :
* Add `--configuration=<yourConf>` to build in 'stage' or 'production' mode. <br>
* Or simply `ng -c stage` for pre-prod and `ng build --prod` for a proper production environment.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice.  To use this command, you need to first add a package that implements end-to-end testing capabilities.
By default, the tests will run at each push from the command line. To skip the tests add 
```bash
--no-verify
``` 
to the push command.

## Environments

=> Files in ./src/app/environments <br>
=> Config in angular.json under:
`"configurations": {
    production: { ... },
    stage: { ... },
    development: { ... }
}`

## Adding new form fields to the entities (Clients and Suppliers)
1. Add the business logic in the service (business.service, europe.service, etc.)
2. Add the relative methods in the strategy files (IEntityServiceStrategy, etablissement-client-service-strategy, etc.)
3. Add the field in the Dto to Form and Form to Dto converters in the service (business.service, europe.service, etc.)
4. Add the field in the form-initializer service

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.


