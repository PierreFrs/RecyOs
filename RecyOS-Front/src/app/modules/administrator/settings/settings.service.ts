import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BackendApiService } from "../../../backend.api.service";
import { ParameterDto, CreateParameterDto, UpdateParameterDto } from "./settings.types";

@Injectable({
    providedIn: 'root'
})
export class SettingsService {
    private readonly _parameter: BehaviorSubject<ParameterDto | null> = new BehaviorSubject<ParameterDto | null>(null);
    private readonly _parameters: BehaviorSubject<ParameterDto[] | null> = new BehaviorSubject<ParameterDto[] | null>(null);
    private readonly _modules: BehaviorSubject<string[] | null> = new BehaviorSubject<string[] | null>(null);
    private readonly _baseUrl = this.apiService.getBaseUrl();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService
    ) {}

    /**
     * Getters pour les observables
     */
    get parameter$(): Observable<ParameterDto | null> {
        return this._parameter.asObservable();
    }

    get parameters$(): Observable<ParameterDto[] | null> {
        return this._parameters.asObservable();
    }

    get modules$(): Observable<string[] | null> {
        return this._modules.asObservable();
    }

    /**
     * Récupère un paramètre par son identifiant
     */
    getParameterById(id: number): Observable<ParameterDto> {
        return this._httpClient
            .get<ParameterDto>(`${this._baseUrl}/parameter/${id}`)
            .pipe(
                tap((parameter: ParameterDto) => {
                    this._parameter.next(parameter);
                })
            );
    }

    /**
     * Supprime un paramètre par son identifiant
     */
    deleteParameter(id: number): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/parameter/${id}`)
            .pipe(
                tap(() => {
                    const parameters = this._parameters.getValue();
                    if (parameters) {
                        const updatedParameters = parameters.filter(param => param.id !== id);
                        this._parameters.next(updatedParameters);
                    }
                })
            );
    }

    /**
     * Récupère tous les paramètres
     */
    getAllParameters(): Observable<ParameterDto[]> {
        return this._httpClient
            .get<ParameterDto[]>(`${this._baseUrl}/parameter`)
            .pipe(
                tap((parameters: ParameterDto[]) => {
                    this._parameters.next(parameters);
                })
            );
    }

    /**
     * Crée un nouveau paramètre
     */
    createParameter(parameter: CreateParameterDto): Observable<ParameterDto> {
        return this._httpClient.post<ParameterDto>(`${this._baseUrl}/parameter`, parameter)
            .pipe(
                tap((newParameter: ParameterDto) => {
                    const parameters = this._parameters.getValue();
                    if (parameters) {
                        this._parameters.next([...parameters, newParameter]);
                    }
                })
            );
    }

    /**
     * Met à jour un paramètre existant
     */
    updateParameter(id: number, parameter: UpdateParameterDto): Observable<ParameterDto> {
        return this._httpClient.put<ParameterDto>(`${this._baseUrl}/parameter`, { ...parameter, id })
            .pipe(
                tap((updatedParameter: ParameterDto) => {
                    const parameters = this._parameters.getValue();
                    if (parameters) {
                        const index = parameters.findIndex(p => p.id === id);
                        if (index !== -1) {
                            parameters[index] = updatedParameter;
                            this._parameters.next([...parameters]);
                        }
                    }
                    
                    // Mise à jour du paramètre courant si nécessaire
                    if (this._parameter.getValue()?.id === id) {
                        this._parameter.next(updatedParameter);
                    }
                })
            );
    }

    /**
     * Récupère tous les modules disponibles
     */
    getAllModules(): Observable<string[]> {
        return this._httpClient
            .get<string[]>(`${this._baseUrl}/parameter/modules`)
            .pipe(
                tap((modules: string[]) => {
                    this._modules.next(modules);
                })
            );
    }

    /**
     * Récupère tous les paramètres d'un module
     */
    getParametersByModule(module: string): Observable<ParameterDto[]> {
        return this._httpClient
            .get<ParameterDto[]>(`${this._baseUrl}/parameter/module/${module}`)
            .pipe(
                tap((parameters: ParameterDto[]) => {
                    this._parameters.next(parameters);
                })
            );
    }

    /**
     * Récupère un paramètre par son module et son nom
     */
    getParameterByModuleAndName(module: string, name: string): Observable<ParameterDto> {
        return this._httpClient
            .get<ParameterDto>(`${this._baseUrl}/parameter/module/${module}/nom/${name}`)
            .pipe(
                tap((parameter: ParameterDto) => {
                    this._parameter.next(parameter);
                })
            );
    }
}
