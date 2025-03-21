// RepositoryException.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/08/2024
// Fichier Modifié le : 16/08/2024
// Code développé pour le projet : RecyOs

using System;

namespace RecyOs.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException()
    {
    }

    public RepositoryException(string message) : base(message)
    {
    }

    public RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}