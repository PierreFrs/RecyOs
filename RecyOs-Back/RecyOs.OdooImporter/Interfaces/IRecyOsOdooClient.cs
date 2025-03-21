using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyOs.OdooImporter.Interfaces;

public interface IRecyOsOdooClient
{
    /// <summary>
    /// Effectue une recherche et lecture dans Odoo
    /// </summary>
    /// <param name="model">Le modèle Odoo à interroger</param>
    /// <param name="domain">Les conditions de filtrage</param>
    /// <param name="fields">Les champs à récupérer</param>
    /// <param name="limit">Limite optionnelle du nombre de résultats</param>
    /// <returns>Liste des enregistrements trouvés</returns>
    Task<List<Dictionary<string, object>>> SearchRead(string model, object[] domain, string[] fields, int limit = 0);
}
