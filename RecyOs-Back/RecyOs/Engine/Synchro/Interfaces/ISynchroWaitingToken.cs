//  IEngineWaitingToken.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/05/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading;

namespace RecyOs.Engine.Interfaces;

public interface ISynchroWaitingToken
{
    public void StopWaiting();
    public CancellationToken GetCancellationToken();
    public void ResetWaitingToken();
}