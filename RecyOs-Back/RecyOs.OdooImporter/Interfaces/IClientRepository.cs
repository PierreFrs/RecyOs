using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Interfaces;

public interface IClientRepository
{
    /// <summary>
    /// Retourne tous les idodoo des clients de tous types
    /// </summary>
    Task<IList<long>> GetAllAsync();

    /// <summary>
    /// Retourne tous les idodoo des clients français
    /// </summary>
    Task<IList<EtablissementClient>> GetAllFrenchAsync();

    /// <summary>
    /// Retourne tous les idodoo des clients européens
    /// </summary>
    Task<IList<ClientEurope>> GetAllEuropeanAsync();

    /// <summary>
    ///     Retourne tous les idodoo des clients particuliers
    /// </summary>
    Task<IList<ClientParticulier>> GetAllIndividualAsync();
}
