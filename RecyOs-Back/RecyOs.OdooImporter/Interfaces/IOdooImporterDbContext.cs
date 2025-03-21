using RecyOs.Helpers;

namespace RecyOs.OdooImporter.Interfaces;

public interface IOdooImporterDbContext
{
    DataContext GetContext(); // Ajoutez des méthodes spécifiques à votre contexte si nécessaire
}
