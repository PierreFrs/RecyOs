//  EngineWaitingToken.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/05/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading;
using RecyOs.Engine.Interfaces;

namespace RecyOs.Engine.Services;

public class SynchroWaitingToken : ISynchroWaitingToken
{
    private CancellationTokenSource cancellationTokenSource;

    public SynchroWaitingToken()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// Interrompt l'attente dans la tâche en arrière-plan en annulant le jeton d'annulation et en réinitialisant le jeton d'annulation pour la prochaine attente.
    /// </summary>
    public void StopWaiting()
    {
        // Annule le jeton d'annulation pour interrompre l'attente dans la tâche en arrière-plan.
        cancellationTokenSource.Cancel();
    }
    
    /// <summary>
    /// Récupère le jeton d'annulation.
    /// </summary>
    public CancellationToken GetCancellationToken()
    {
        return cancellationTokenSource.Token;
    }
    
    /// <summary>
    /// Réinitialise le jeton d'annulation pour la prochaine attente.
    /// </summary>
    public void ResetWaitingToken()
    {
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }
}