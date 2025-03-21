// /** IrPropertyService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 16/07/2023
//  * Fichier Modifié le : 20/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Services;

public class IrPropertyService<TIrProperty> : IIrPropertyService where TIrProperty : IrPropertyOdooModel, new()
{
    protected readonly IIrPropertyRepository<TIrProperty> _irPropertyRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructeur de la classe IrPropertyService.
    /// </summary>
    /// <param name="irPropertyRepository">Le référentiel pour interagir avec les entités IrProperty dans le système.</param>
    /// <param name="mapper">Un objet IMapper pour effectuer les opérations de mappage.</param>
    /// <remarks>
    /// Ce constructeur initialise l'instance d'IrPropertyService avec le référentiel IrProperty et l'objet IMapper fournis.
    /// </remarks>
    public IrPropertyService(IIrPropertyRepository<TIrProperty> irPropertyRepository,
     IMapper mapper)
    {
        _irPropertyRepository = irPropertyRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Crée une nouvelle propriété dans le système à partir du DTO fourni.
    /// </summary>
    /// <param name="prmProperty">Le DTO de propriété contenant les informations à utiliser pour créer la propriété.</param>
    /// <returns>Retourne un DTO de la propriété nouvellement créée.</returns>
    /// <remarks>
    /// Cette méthode utilise l'objet IMapper pour convertir le DTO de propriété en une entité IrProperty.
    /// L'entité est ensuite passée au référentiel pour créer la propriété.
    /// La propriété créée est mappée à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<IrPropertyDto> CreateProperty(IrPropertyDto prmProperty)
    {
        var irProperty = _mapper.Map<TIrProperty>(prmProperty);
        var irPropertyResult = await _irPropertyRepository.CreateProperty(irProperty);
        return _mapper.Map<IrPropertyDto>(irPropertyResult);
    }
    
    /// <summary>
    /// Récupère une propriété spécifique du système en fonction de son ID.
    /// </summary>
    /// <param name="id">L'ID de la propriété à récupérer.</param>
    /// <returns>Retourne un DTO de la propriété correspondante, ou null si aucune propriété n'est trouvée.</returns>
    /// <remarks>
    /// Cette méthode utilise le référentiel pour récupérer la propriété avec l'ID spécifié.
    /// Si une propriété correspondante est trouvée, elle est mappée à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<IrPropertyDto> GetPropertyAsync(int id)
    {
        var irPropertyResult = await _irPropertyRepository.GetPropertyAsync(id);
        return _mapper.Map<IrPropertyDto>(irPropertyResult);
    }
    
    /// <summary>
    /// Récupère une propriété spécifique d'un partenaire dans le système en fonction de son ID de partenaire et du nom de la propriété.
    /// </summary>
    /// <param name="resPartnerId">L'ID du partenaire dont la propriété doit être récupérée.</param>
    /// <param name="propertyName">Le nom de la propriété à récupérer.</param>
    /// <returns>Retourne un DTO de la propriété correspondante, ou null si aucune propriété n'est trouvée.</returns>
    /// <remarks>
    /// Cette méthode utilise un filtre Odoo pour rechercher une propriété avec l'ID de partenaire et le nom de propriété spécifiés.
    /// Si une propriété correspondante est trouvée, elle est mappée à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<IrPropertyDto> GetResPartnerPropertyAsync(int resPartnerId, string propertyName)
    {
        OdooFilter filter = OdooFilter<IrPropertyOdooModel>.Create()
            .And()
            .EqualTo(x => x.ResId, resPartnerId)
            .And()
            .EqualTo(x => x.Name, propertyName);
        
       
        var result =_irPropertyRepository.SearchProperty(filter);
        var irPropertyResult = (await result).FirstOrDefault();
        return _mapper.Map<IrPropertyDto>(irPropertyResult);
    }
    
    /// <summary>
    /// Récupère toutes les propriétés d'un partenaire spécifique dans le système en fonction de son ID de partenaire.
    /// </summary>
    /// <param name="resPartnerId">L'ID du partenaire dont les propriétés doivent être récupérées.</param>
    /// <returns>Retourne un tableau de DTOs correspondant aux propriétés du partenaire, ou un tableau vide si aucune propriété n'est trouvée.</returns>
    /// <remarks>
    /// Cette méthode utilise un filtre Odoo pour rechercher toutes les propriétés avec l'ID de partenaire spécifié.
    /// Les propriétés correspondantes sont mappées à un tableau de DTOs qui est ensuite retourné.
    /// </remarks>
    public async Task<IrPropertyDto[]> GetResPartnerPropertiesAsync(int resPartnerId)
    {
        OdooFilter filter = OdooFilter<IrPropertyOdooModel>.Create()
            .And()
            .EqualTo(x => x.ResId, resPartnerId);
        
        var result = await _irPropertyRepository.SearchProperty(filter);
        return _mapper.Map<IrPropertyDto[]>(result);
    }
    
    /// <summary>
    /// Met à jour une propriété existante dans le système en utilisant les informations fournies dans le DTO.
    /// </summary>
    /// <param name="prmProperty">Le DTO de la propriété contenant les informations mises à jour à utiliser.</param>
    /// <returns>Retourne un DTO de la propriété mise à jour, ou null si la mise à jour a échoué.</returns>
    /// <remarks>
    /// Cette méthode utilise l'objet IMapper pour convertir le DTO de propriété en une entité IrProperty.
    /// L'entité est ensuite passée au référentiel pour mettre à jour la propriété.
    /// La propriété mise à jour est mappée à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<IrPropertyDto> UpdateProperty(IrPropertyDto prmProperty)
    {
        var irProperty = _mapper.Map<TIrProperty>(prmProperty);
        var irPropertyResult = await _irPropertyRepository.UpdateProperty(irProperty);
        return _mapper.Map<IrPropertyDto>(irPropertyResult);
    }
}