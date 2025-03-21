//  ImportFcli.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using RecyOs.Controllers;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service.pappers;

namespace RecyOs.Commands;

public class ImportFcli : ICommandImportFcli
{
    private readonly ILogger<ImportFcli> _logger;
    private readonly IFCliService _fCliService;
    private readonly IMapper _mapper;
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IPappersUtilitiesService _pappersUtilitiesService;
    
    public ImportFcli(IFCliService fCliService, IMapper mapper, IEtablissementClientService etablissementClientService,
        IPappersUtilitiesService pappersUtilitiesService, ILogger<ImportFcli> logger)
    {
        _fCliService = fCliService;
        _mapper = mapper;
        _etablissementClientService = etablissementClientService;
        _pappersUtilitiesService = pappersUtilitiesService;
        _logger = logger;
    }
    
    /// <summary>
    /// Importe et normalise un établissement par son code si l'établissement est valide depuis le service _fCliService.
    /// </summary>
    /// <returns>
    /// Retourne true une fois que l'établissement client a été traité et importé.
    /// </returns>
    /// <remarks>
    /// Le processus d'importation suit les étapes suivantes :
    /// 1. Obtention de l'établissement client valide depuis le service _fCliService.
    /// 2. Si le SIRET n'est pas vide ou null, l'établissement est normalisé.
    /// 3. Si le SIRET normalisé n'est pas vide ou null, l'établissement normalisé est mappé en EtablissementClientExDto.
    /// 4. Le nouvel établissement mappé est créé dans le service _etablissementClientService.
    ///  Les actions et les résultats sont enregistrés à l'aide d'un système de logging.
    ///  Note : La méthode est asynchrone pour gérer efficacement les opérations IO et éviter le blocage du thread principal. 
    /// </remarks>
    public async Task<bool> Import(string code)
    {
        EtablissementMkgtDto res = await _fCliService.GetClient(code);
        EtablissementMkgtDto normed = null;
        if (res == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(res.siret))
        {
            normed = normalize(res);
        }else{
            _logger.LogInformation("Etablissement invalide");
            return false;
        }
        
        if (!string.IsNullOrEmpty(normed.siret))
        {
            var etablissementClient = _mapper.Map<EtablissementClientDto>(normed);
            _logger.LogInformation(etablissementClient.ToString());
            await _pappersUtilitiesService.CreateEntrepriseBySiret(etablissementClient.Siret);
            await _etablissementClientService.Create(etablissementClient);
            
        }else{
            _logger.LogInformation("Etablissement invalide");
            return false;
        }
        
        return true;
    }
    

    /// <summary>
    /// Importe et normalise une liste d'établissements clients valides depuis le service _fCliService.
    /// </summary>
    /// <returns>
    /// Retourne true une fois que tous les établissements clients ont été traités et importés.
    /// </returns>
    /// <remarks>
    /// Le processus d'importation suit les étapes suivantes :
    /// 1. Obtention de la liste des établissements clients valides depuis le service _fCliService.
    /// 2. Pour chaque établissement client :
    ///    a. Si le SIRET n'est pas vide ou null, l'établissement est normalisé.
    ///    b. Si le SIRET normalisé n'est pas vide ou null, l'établissement normalisé est mappé en EtablissementClientExDto.
    ///    c. Le nouvel établissement mappé est créé dans le service _etablissementClientService.
    /// Les actions et les résultats sont enregistrés à l'aide d'un système de logging.
    /// Note : La méthode est asynchrone pour gérer efficacement les opérations IO et éviter le blocage du thread principal.
    /// </remarks>
    public async Task<bool> Import()
    {
        IList<EtablissementMkgtDto> clients = await _fCliService.GetValidsClients();
    
        foreach (EtablissementMkgtDto etablissementMkgtDto in clients)
        {
            EtablissementMkgtDto normed = null;
            
            if (!string.IsNullOrEmpty(etablissementMkgtDto.siret))
            {
                normed = normalize(etablissementMkgtDto);
            }
            if (!string.IsNullOrEmpty(normed.siret))
            {
                await _pappersUtilitiesService.CreateEntrepriseBySiret(normed.siret);
                await HandleNormedEtablissement(normed);
            }
        }
        return true;
    }
    
    /// <summary>
    /// Normalise les informations contenues dans un objet EtablissementMkgtDto. 
    /// </summary>
    /// <param name="fcli">L'objet EtablissementMkgtDto contenant les informations à normaliser.</param>
    /// <returns>
    /// Retourne l'objet EtablissementMkgtDto après avoir normalisé ses champs (SIRET, téléphones et e-mails).
    /// </returns>
    /// <remarks>
    /// Le processus de normalisation suit les étapes suivantes :
    /// 1. Vérification et normalisation du numéro SIRET.
    /// 2. Normalisation des numéros de téléphone (t2, t3, ptb2, ptb3).
    /// 3. Vérification des adresses e-mail (email1, email2).
    /// Après la normalisation, l'objet est renvoyé avec les valeurs normalisées.
    /// </remarks>
    private EtablissementMkgtDto normalize(EtablissementMkgtDto fcli)
    {
        fcli.siret = checkSiret(fcli.siret);
        fcli.t2 = NormalizeTel(fcli.t2);
        fcli.t3 = NormalizeTel(fcli.t3);
        fcli.ptb2 = NormalizeTel(fcli.ptb2);
        fcli.ptb3 = NormalizeTel(fcli.ptb3);
        fcli.email1 = checkEmail(fcli.email1);
        fcli.email2 = checkEmail(fcli.email2);
        return fcli;
    }

    /// <summary>
    /// Vérifie la validité d'un numéro SIRET en utilisant l'algorithme de Luhn.
    /// </summary>
    /// <param name="numberToVerify">Le numéro SIRET à vérifier.</param>
    /// <returns>
    /// Retourne le numéro SIRET s'il est valide après avoir été préparé (nettoyé des caractères non-alphanumériques).
    /// Si le numéro est invalide ou si la chaîne en entrée est vide ou null, retourne null.
    /// </returns>
    /// <remarks>
    /// Le processus de vérification suit les étapes suivantes :
    /// 1. Préparation du numéro en supprimant les caractères non-alphanumériques.
    /// 2. Extraction des chiffres du numéro préparé.
    /// 3. Multiplication de chaque deuxième chiffre par deux.
    /// 4. Addition de tous les chiffres (si un chiffre est à deux chiffres comme 10 ou plus, ces chiffres sont additionnés individuellement).
    /// 5. Vérification que le total de l'addition est un multiple de 10 (algorithme de Luhn).
    /// Les actions et les résultats sont enregistrés à l'aide d'un système de logging.
    /// </remarks>
    private string checkSiret(string numberToVerify)
    {
        _logger.LogInformation("checkSiret({}) : ", numberToVerify);

        if (string.IsNullOrEmpty(numberToVerify))
        {
            _logger.LogTrace("checkSiret({}) => null or empty", numberToVerify);
            return null;
        }

        string formatInput = PrepareInput(numberToVerify);
        var arrDigits = GetDigits(formatInput);
        var arrMult = GetMultipliedDigits(arrDigits);
        var ttlAddition = GetTotalAddition(arrMult);

        if (arrDigits == null || arrMult == null || ttlAddition == null)
        {
            _logger.LogTrace("checkSiret({}) => null", numberToVerify);
            return null;
        }

        if (ttlAddition % 10 == 0)
        {
            _logger.LogTrace("checkSiret({}) => valid", numberToVerify);
            return formatInput;
        }
        else
        {
            _logger.LogTrace("checkSiret({}) => invalid", numberToVerify);
            return null;
        }
    }

    /// <summary>
    /// Prépare et nettoie une chaîne de caractères en supprimant tout ce qui n'est pas un caractère alphanumérique ou un underscore (_).
    /// </summary>
    /// <param name="input">La chaîne de caractères à préparer.</param>
    /// <returns>
    /// Retourne une chaîne de caractères ne contenant que des lettres (majuscules ou minuscules), des chiffres et des underscores (_).
    /// </returns>
    /// <remarks>
    /// Par exemple, pour la chaîne "Ab#c_123!", la méthode retourne "Abc_123".
    /// </remarks>
    private string PrepareInput(string input)
    {
        return Regex.Replace(input, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
    }

    /// <summary>
    /// Extrait tous les chiffres d'une chaîne de caractères, les renverse et les retourne dans un tableau.
    /// </summary>
    /// <param name="input">La chaîne de caractères contenant les chiffres à extraire.</param>
    /// <returns>
    /// Retourne un tableau d'entiers où chaque élément est un chiffre de la chaîne de caractères en entrée, en ordre inversé.
    /// </returns>
    /// <remarks>
    /// Par exemple, pour la chaîne "1234", la méthode retourne [4, 3, 2, 1].
    /// Si la chaîne de caractères contient des caractères non numériques, ils seront ignorés.
    /// </remarks>
    private int[] GetDigits(string input)
    {
        try
        {
            string reverseInput = new string(input.ToCharArray().Reverse().ToArray());
            return Array.ConvertAll<string, int>(
                Regex.Split(reverseInput, @"(?!^)(?!$)"),
                str => int.Parse(str)
            );
        }
        catch (FormatException)
        {
            _logger.LogWarning("GetDigits({0}) : FormatException", input);
            return null;
        }
    }

    /// <summary>
    /// Multiplie chaque deuxième chiffre d'un tableau par deux. Les autres chiffres restent inchangés.
    /// </summary>
    /// <param name="digits">Tableau de chiffres à traiter.</param>
    /// <returns>
    /// Retourne un nouveau tableau où chaque deuxième chiffre est multiplié par deux et les autres chiffres restent inchangés.
    /// </returns>
    /// <remarks>
    /// Par exemple, pour le tableau [1, 2, 3, 4], la méthode retourne [1, 4, 3, 8].
    /// La position des chiffres est basée sur une indexation à partir de zéro.
    /// </remarks>
    private int[] GetMultipliedDigits(int[] digits)
    {
        if (digits == null)
        {
            return null;
        }
        int[] multipliedDigits = new int[digits.Length];
        for (int i = 0; i < digits.Length; i++)
        {
            if ((i+1) % 2 == 0)
                multipliedDigits[i] = 2 * digits[i];
            else
                multipliedDigits[i] = digits[i];
        }
        return multipliedDigits;
    }

    /// <summary>
    /// Calcule le total cumulé des chiffres d'un tableau. Si un élément du tableau a plusieurs chiffres 
    /// (par exemple, 10 ou plus), chaque chiffre est traité individuellement dans le total.
    /// </summary>
    /// <param name="digits">Tableau de chiffres dont le total doit être calculé.</param>
    /// <returns>
    /// Retourne la somme de tous les chiffres du tableau. Si un élément du tableau a plusieurs chiffres, 
    /// chacun d'eux est ajouté individuellement au total.
    /// </returns>
    /// <remarks>
    /// Par exemple, pour le tableau [2, 12, 3], la méthode retourne 2 + 1 + 2 + 3 = 8.
    /// </remarks>
    private int? GetTotalAddition(int[] digits)
    {
        if (digits== null)
        {
            return null;
        }

        int total = 0;
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i] >= 10)
            {
                var subDigits = GetDigits(digits[i].ToString());
                total += subDigits.Sum();
            }
            else
            {
                total += digits[i];
            }
        }
        return total;
    }

    /// <summary>
    /// Normalise un numéro de téléphone en extrayant le code du pays et en formatant le numéro selon une structure définie.
    /// </summary>
    /// <param name="tel">Le numéro de téléphone à normaliser.</param>
    /// <returns>
    /// Retourne le numéro de téléphone normalisé au format "+[CodePays] X XX XX XX XX" ou null en cas d'erreur.
    /// Si le numéro de téléphone est null ou vide, null est également renvoyé.
    /// </returns>
    /// <remarks>
    /// Cette méthode suit la logique suivante :
    /// 1. Si le numéro commence par "+", le code pays est extrait.
    /// 2. Si le numéro commence par "00", le code pays est également extrait.
    /// 3. Si le numéro ne correspond à aucun des cas précédents, le code pays par défaut est considéré comme "33" (France).
    /// 4. Le numéro est ensuite nettoyé pour ne conserver que les chiffres.
    /// 5. Enfin, le numéro est formaté selon sa longueur.
    /// Des logs sont utilisés tout au long de la méthode pour enregistrer les actions et les résultats.
    /// </remarks>
    private string NormalizeTel(string tel)
    {
        _logger.LogInformation("normalize_tel({0})", tel);

        string countryCode = "";
        // si tel est null ou vide, on retourne null
        if (string.IsNullOrEmpty(tel))
        {
            _logger.LogTrace("normalize_tel({0}) => null or empty", tel);
            return null;
        }
        // si tel commence par + on extrait le code pays
        else if (tel.StartsWith('+'))
        {
            countryCode = tel.Substring(1, 2);
            tel = tel.Substring(3);
        }
        // si tel commence par 00 on extrait le code pays
        else if (tel.StartsWith("00"))
        {
            countryCode = tel.Substring(2, 2);
            tel = tel.Substring(4);
        }
        // sinon on considère que le code pays est 33
        else
        {
            countryCode = "33";
            tel = tel.Substring(1);
        }

        // on supprime tout ce qui n'est pas chiffre
        tel = Regex.Replace(tel, @"[^0-9+]", "");

        // formatage du numéro de téléphone
        string formattedPhoneNumber;
        if (tel.Length == 9)
        {
            formattedPhoneNumber = $"+{countryCode} {tel[0]} {tel.Substring(1, 2)} {tel.Substring(3, 2)} {tel.Substring(5, 2)} {tel.Substring(7, 2)}";
        }
        else if (tel.Length >= 10)
        {
            formattedPhoneNumber = $"+{countryCode} {tel[0]} {tel.Substring(1, 2)} {tel.Substring(3, 2)} {tel.Substring(5, 2)} {tel.Substring(7, 2)}";
        }
        else
        {
            _logger.LogTrace("normalize_tel({0}) : tel invalide", tel);
            return null;
        }

        _logger.LogTrace("normalize_tel({0}) : {1}", tel, formattedPhoneNumber);
        return formattedPhoneNumber;
    }
    
    /// <summary>
    /// Vérifie la validité d'une adresse e-mail en utilisant une expression régulière.
    /// </summary>
    /// <param name="email">L'adresse e-mail à vérifier.</param>
    /// <returns>
    /// Retourne l'adresse e-mail si elle est valide, sinon retourne null.
    /// Si l'adresse e-mail est null ou vide, null est également renvoyé.
    /// </returns>
    /// <remarks>
    /// Cette méthode utilise également un système de logging pour enregistrer les actions et les résultats.
    /// </remarks>
    private string checkEmail(string email)
    {
        _logger.LogInformation("checkEmail({0})", email);
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogTrace("checkEmail({0}) => null or empty", email);
            return null;
        }
        else
        {
            // Utilise une expression régulière pour vérifier la validité de l'adresse e-mail
            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*(\+\w+)*@([\w-]+\.)+[\w-]{2,4}$");
            if (regex.IsMatch(email))
            {
                _logger.LogTrace("checkEmail({0}) => valid", email);
                return email;
            }
            else
            {
                _logger.LogTrace("checkEmail({0}) => invalid", email);
                return null;
            }
        }
    }
    
    private async Task HandleNormedEtablissement(EtablissementMkgtDto normed)
    {
        var etablissementClient = _mapper.Map<EtablissementClientDto>(normed);
        _logger.LogInformation(etablissementClient.ToString());
        var result = await _etablissementClientService.Create(etablissementClient);
        if (result != null)
        {
            await CheckAndHandleRadiation(etablissementClient);
        }
    }

    private async Task CheckAndHandleRadiation(EtablissementClientDto etablissementClient)
    {
        var pappersData = await _pappersUtilitiesService.CreateEntrepriseBySiret(etablissementClient.Siret);
        if (pappersData != null && pappersData.EtablissementCesse)
        {
            var client = await _etablissementClientService.GetBySiret(pappersData.Siret);
            client.Radie = true;
            await _etablissementClientService.Edit(client);
        }
    }



}