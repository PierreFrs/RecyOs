/// <summary>
/// Repository pour les messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOs.Engine.Alerts.Repositories;

public class EngineMessageMailRepository : IEngineMessageMailRepository
{
    private readonly DataContext _context;
    public EngineMessageMailRepository(IDataContextEngine ctx)
    {
        _context = ctx.GetContext();
    }

    /// <summary>
    /// Récupère les messages de mail en attente.
    /// </summary>
    /// <returns>Liste des messages de mail en attente.</returns>
    public async Task<IEnumerable<MessageMail>> GetPendingMessagesAsync()
    {
        var res = await _context.MessageMails.AsNoTracking()
            .Where(m => m.Status == TaskStatus.Pending)
            .OrderBy(m => m.Priority)
            .ThenBy(m => m.DateCreated)
            .ToListAsync();
        return res;
    }

    /// <summary>
    /// Récupère les messages de mail par statut.
    /// </summary>
    /// <param name="status">Statut des messages de mail.</param>
    /// <returns>Liste des messages de mail par statut.</returns>
    public async Task<IEnumerable<MessageMail>> GetByStatusAsync(TaskStatus status)
    {
        var res = await _context.MessageMails
            .Where(m => m.Status == status)
            .OrderByDescending(m => m.DateCreated)
            .AsNoTracking()
            .ToListAsync();
        return res;
    }

    /// <summary>
    /// Récupère les messages de mail en échec.
    /// </summary>
    /// <returns>Liste des messages de mail en échec.</returns>
    public async Task<IEnumerable<MessageMail>> GetFailedMessagesAsync()
    {
        var res = await _context.MessageMails.AsNoTracking()
            .Where(m => m.Status == TaskStatus.Failed)
            .OrderByDescending(m => m.DateCreated)
            .ToListAsync();
        return res;
    }

    /// <summary>
    /// Met à jour le statut d'un message de mail.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    /// <param name="newStatus">Nouveau statut du message de mail.</param>
    public async Task UpdateStatusAsync(int id, TaskStatus newStatus)
    {
        var message = await _context.MessageMails.FindAsync(id);
        if (message == null)
            throw new KeyNotFoundException($"MessageMail with ID {id} not found");

        message.Status = newStatus;
        await _context.SaveChangesAsync();
        // stop the tracking of the entity
        _context.Entry(message).State = EntityState.Detached;
    }

    /// <summary>
    /// Crée un message de mail.
    /// </summary>
    /// <param name="entity">Message de mail à créer.</param>
    /// <returns>Message de mail créé.</returns>
    public async Task<MessageMail> CreateAsync(MessageMail entity)
    {
        var result = await _context.MessageMails.AddAsync(entity);
        await _context.SaveChangesAsync();
        // stop the tracking of the entity
        _context.Entry(result.Entity).State = EntityState.Detached;
        return result.Entity;
    }

    /// <summary>
    /// Récupère un message de mail par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    /// <returns>Message de mail trouvé.</returns>
    public async Task<MessageMail> GetByIdAsync(int id)
    {
        return await _context.MessageMails.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Supprime un message de mail.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.MessageMails.Remove(entity);
            await _context.SaveChangesAsync();
            // stop the tracking of the entity
            _context.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Met à jour un message de mail.
    /// </summary>
    /// <param name="entity">Message de mail à mettre à jour.</param>
    /// <returns>Message de mail mis à jour.</returns>
    public async Task<MessageMail> UpdateAsync(MessageMail entity)
    {
        _context.MessageMails.Update(entity);
        await _context.SaveChangesAsync();
        // stop the tracking of the entity
        _context.Entry(entity).State = EntityState.Detached;
        return entity;
    }
} 