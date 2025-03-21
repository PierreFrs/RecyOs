// /** IrPropertyRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 15/07/2023
//  * Fichier Modifié le : 20/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.Engine.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Repository;

public class IrPropertyRepository : BaseOdooRepository, IIrPropertyRepository<IrPropertyOdooModel>
{
    private readonly OdooRepository<IrPropertyOdooModel> _odooRepository;
    
    /// <summary>
    /// Constructeur de la classe IrPropertyRepository.
    /// </summary>
    /// <param name="configuration">L'objet IConfiguration pour lire les paramètres de configuration.</param>
    /// <remarks>
    /// Ce constructeur initialise une nouvelle instance de la classe IrPropertyRepository avec la configuration fournie.
    /// Il hérite de la classe BaseOdooRepository et implémente l'interface IIrPropertyRepository.
    /// Une nouvelle instance de OdooRepository est créée avec la configuration Odoo et est assignée à la propriété privée _odooRepository.
    /// </remarks>
    public IrPropertyRepository(IConfiguration configuration) : base(configuration)
    {
        _odooRepository = new OdooRepository<IrPropertyOdooModel>(_odooConfig);
    }

    /// <summary>
    /// Récupère une propriété spécifique par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la propriété à récupérer.</param>
    /// <returns>Retourne l'objet IrPropertyOdooModel correspondant à l'identifiant fourni, ou null si aucun n'a été trouvé.</returns>
    /// <remarks>
    /// Cette méthode crée un filtre Odoo pour rechercher la propriété avec l'identifiant donné.
    /// Elle utilise ensuite le répertoire Odoo pour interroger le système et récupérer la propriété correspondante.
    /// Le résultat est ensuite renvoyé.
    /// </remarks>
    public async Task<IrPropertyOdooModel> GetPropertyAsync(int id)
    {
        var filter = OdooFilter<IrPropertyOdooModel>.Create()
            .Or()
            .EqualTo(x => x.Id, id);
        
        var res =await _odooRepository.Query()
            .Where(filter)
            .FirstOrDefaultAsync();
        
        return res.Value;
    }

    /// <summary>
    /// Met à jour une propriété existante dans le système.
    /// </summary>
    /// <param name="prmProperty">L'objet IrPropertyOdooModel qui contient les données de la propriété à mettre à jour.</param>
    /// <returns>Retourne l'objet IrPropertyOdooModel mis à jour, ou null si la mise à jour a échoué.</returns>
    /// <remarks>
    /// Cette méthode crée un nouveau modèle IrPropertyOdooModel à partir des données fournies dans prmProperty.
    /// Elle utilise ensuite le répertoire Odoo pour mettre à jour la propriété correspondante dans le système.
    /// Si la mise à jour est réussie, elle retourne l'objet prmProperty.
    /// Si la mise à jour échoue, elle retourne null.
    /// </remarks>
    public async Task<IrPropertyOdooModel> UpdateProperty(IrPropertyOdooModel prmProperty)
    {
        var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = prmProperty.Name,
            ValueReference = prmProperty.ValueReference,
            CompanyId = prmProperty.CompanyId,
            FieldsId = prmProperty.FieldsId,
            ResId = prmProperty.ResId,
            Type = prmProperty.Type,
        });
        
        var retour = await UpdateAsync(model, (int)prmProperty.Id);
        if (retour)
        {
            return prmProperty;
        }
        else return null;
    }

    /// <summary>
    /// Crée une nouvelle propriété dans le système.
    /// </summary>
    /// <param name="prmProperty">L'objet IrPropertyOdooModel qui contient les données de la nouvelle propriété.</param>
    /// <returns>Retourne l'objet IrPropertyOdooModel nouvellement créé avec l'identifiant assigné.</returns>
    /// <remarks>
    /// Cette méthode crée un nouveau modèle IrPropertyOdooModel à partir des données fournies dans prmProperty.
    /// Elle utilise ensuite le répertoire Odoo pour créer la nouvelle propriété dans le système.
    /// L'identifiant assigné à la nouvelle propriété est ensuite assigné à l'objet prmProperty, qui est renvoyé.
    /// </remarks>
    public async Task<IrPropertyOdooModel> CreateProperty(IrPropertyOdooModel prmProperty)
    {
        var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = prmProperty.Name,
            ValueReference = prmProperty.ValueReference,
            CompanyId = prmProperty.CompanyId,
            FieldsId = prmProperty.FieldsId,
            ResId = prmProperty.ResId,
            Type = prmProperty.Type,
        });
        int id = await CreateAsync(model);
        prmProperty.Id = id;
        return prmProperty;
    }

    /// <summary>
    /// Effectue une recherche de propriétés basée sur un filtre fourni.
    /// </summary>
    /// <param name="filter">L'objet OdooFilter qui définit les critères de la recherche.</param>
    /// <returns>Retourne un tableau d'objets IrPropertyOdooModel correspondant aux résultats de la recherche.</returns>
    /// <remarks>
    /// Cette méthode utilise le répertoire Odoo pour effectuer une recherche de propriétés dans le système.
    /// Les critères de la recherche sont définis par l'objet OdooFilter fourni.
    /// Les résultats de la recherche sont retournés sous forme d'un tableau d'objets IrPropertyOdooModel.
    /// </remarks>
    public async Task<IrPropertyOdooModel[]> SearchProperty(OdooFilter filter)
    {
        var res = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();
        
        return res.Value;
    }
}