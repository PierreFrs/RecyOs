// ServiceException.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/08/2024
// Fichier Modifié le : 16/08/2024
// Code développé pour le projet : RecyOs

using System;

namespace RecyOs.Exceptions;

public class ServiceException : Exception
{
    public ServiceException()
    {
    }

    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}