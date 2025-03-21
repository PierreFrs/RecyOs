//  PappersApiService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using NLog;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service.pappers;

public class PappersApiService : IPappersApiService
{
    private readonly HttpClient _httpClient;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly string apiToken;

    public PappersApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        apiToken = configuration.GetValue<string>("Pappers:ApiKey");
    }

    public async Task<Boolean> ObtenirDonneesAsync(string endPoint, string numeroSirenOuSiret)
    {
        var response = await _httpClient.GetAsync($"{endPoint}?api_token={_httpClient.DefaultRequestHeaders.GetValues("api_token").First()}&siren={numeroSirenOuSiret}");
        response.EnsureSuccessStatusCode();

        var contenu = await response.Content.ReadAsStringAsync();
        var resultat = JsonSerializer.Deserialize<Boolean>(contenu);

        return resultat;
    }
    
    /// <summary>
    /// Cette méthode appelle l'API de Pappers pour récupérer des informations sur une entreprise à partir de son numéro SIRET.
    /// Elle utilise également un mécanisme de mise en cache pour stocker les résultats des appels précédents dans des fichiers JSON
    /// et éviter de répéter les requêtes inutiles.
    /// </summary>
    /// <param name="siret">Le numéro SIRET de l'entreprise à rechercher.</param>
    /// <returns>
    /// Une chaîne de caractères contenant les informations sur l'entreprise au format JSON, ou null si une erreur s'est produite.
    /// </returns>
    public async Task<string> GetEtablissement(string siret)
    {
        string cacheDirectory = "cache";
        string cacheFile = Path.Combine(cacheDirectory, $"{siret}.json");

        // Vérifier si le fichier existe déjà
        if (File.Exists(cacheFile))
        {
            // Si le fichier existe déjà, lire le contenu du fichier et le renvoyer
            return await File.ReadAllTextAsync(cacheFile);
        }

        // Construire l'URL de l'API avec le numéro SIRET et la clé d'API
        string url = $"https://api.pappers.fr/v2/entreprise?siret={siret}&api_token={apiToken}";

        // Envoyer une requête GET à l'API de Pappers
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new HttpRequestException($"Resource not found", null, HttpStatusCode.NotFound);
        }

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();

            // Créer le répertoire de cache s'il n'existe pas
            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }

            // Enregistrer le résultat dans un fichier pour une consultation ultérieure
            await File.WriteAllTextAsync(cacheFile, content);

            return content;
        }
        else
        {
            // Si la requête a échoué, afficher un message d'erreur et renvoyer null
            Logger.Error($"Erreur lors de l'appel de l'API de Pappers : {response.StatusCode} - {response.ReasonPhrase}");
            return null;
        }
    }
}