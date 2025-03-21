// /** BaseOdooRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 10/05/2023
//  * Fichier Modifié le : 19/07/2023
//  * Code développé pour le projet : RecyOs
//  */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.Engine.Services
{
    public class BaseOdooRepository : IBaseOdooRepository
    {
        protected readonly OdooConfig _odooConfig;
        private readonly OdooClient _odooClient;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initialise une nouvelle instance de la classe BaseOdooRepository.
        /// </summary>
        /// <param name="Configuration">Un objet IConfiguration qui contient les paramètres de configuration pour le répertoire.</param>
        /// <remarks>
        /// Ce constructeur initialise un nouvel objet OdooConfig avec les paramètres de configuration fournis, 
        /// crée un nouvel OdooClient en utilisant cet objet OdooConfig, 
        /// puis se connecte au service Odoo en utilisant le client.
        /// </remarks>
        public BaseOdooRepository(IConfiguration Configuration)
        {
            _odooConfig = new OdooConfig(
                apiUrl: Configuration["Odoo:Url"],
                dbName: Configuration["Odoo:Db"],
                userName: Configuration["Odoo:Username"],
                password: Configuration["Odoo:Password"]);

            _odooClient = new OdooClient(_odooConfig);
            Task.Run(() => _odooClient.LoginAsync()).Wait();
        }

        /// <summary>
        /// Met à jour un enregistrement existant dans la base de données Odoo.
        /// </summary>
        /// <param name="prmModel">Le modèle d'objet à mettre à jour.</param>
        /// <param name="prmId">L'identifiant de l'enregistrement à mettre à jour.</param>
        /// <returns>Retourne un booléen indiquant si la mise à jour a réussi ou non.</returns>
        /// <remarks>
        /// Cette méthode utilise le client Odoo pour mettre à jour un enregistrement spécifique dans la base de données Odoo. 
        /// Elle retourne true si la mise à jour a réussi, sinon elle retourne false.
        /// </remarks>
        public async Task<bool> UpdateAsync(OdooDictionaryModel prmModel, long prmId)
        {
            try
            {
                var modelUpdateResult = await _odooClient.UpdateAsync(prmModel, prmId);
                return modelUpdateResult.Succeed;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

        }

        /// <summary>
        /// Crée un nouvel enregistrement dans la base de données Odoo.
        /// </summary>
        /// <param name="prmModel">Le modèle d'objet à créer.</param>
        /// <returns>Retourne l'identifiant du nouvel enregistrement créé.</returns>
        /// <remarks>
        /// Cette méthode utilise le client Odoo pour créer un nouvel enregistrement dans la base de données Odoo. 
        /// Elle retourne l'identifiant du nouvel enregistrement créé.
        /// </remarks>
        public async Task<int> CreateAsync(OdooDictionaryModel prmModel)
        {
            try
            {
                var modelCreateResult = await _odooClient.CreateAsync(prmModel);
                return (int)modelCreateResult.Value;

            }
            catch (Exception e)
            {
                _logger.Error(e);
                return 0;
            }
            
        }
        
        
        public async Task<string> GetModelAsync(string modelName)
        {
            var modelResult = await _odooClient.GetModelAsync(modelName);
            var model = OdooModelMapper.GetDotNetModel(modelName, modelResult.Value);
            return model;
        }
    }
    
    public class WrappedOdooException : Exception
    {
        public object OriginalException { get; }

        public WrappedOdooException(object originalException)
            : base("Une erreur Odoo s'est produite.")
        {
            OriginalException = originalException;
        }
    }

}