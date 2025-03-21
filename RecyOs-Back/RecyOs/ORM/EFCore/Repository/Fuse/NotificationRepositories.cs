/** MessageRepositories.cs - Repository pour les notifications
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 03/02/2025
 * Fichier Modifié le : 03/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.Extensions;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class NotificationRepository : BaseDeletableRepository<Notification, DataContext>, INotificationRepository<Notification>
{
    public NotificationRepository(DataContext context) : base(context)
    {
    }

    public async Task<Notification> GetAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetListAsync(ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .OrderByDescending(n => n.Time)
            .ToListAsync();
    }

    public async Task<Notification> CreateAsync(Notification notification, ContextSession session, bool includeDeleted = false)
    {
        var context = GetContext(session);
        await context.AddAsync(notification);
        await context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notification> UpdateAsync(Notification notification, ContextSession session)
    {
        var objectExists = await Exists(notification, session);
        var context = GetContext(session);
        context.Entry(notification).State = objectExists ? EntityState.Modified : EntityState.Added;

        await context.SaveChangesAsync();
        return notification;
    }

    public async Task DeleteAsync(int id, ContextSession session)
    {
        var notification = await GetAsync(id, session);
        if (notification != null)
        {
            var context = GetContext(session);
            notification.IsDeleted = true;
            context.Entry(notification).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Notification>> GetFilteredNotificationsWithCountAsync(
        NotificationFilter filter, 
        ContextSession session, 
        bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted)
            .AsNoTracking()
            .ApplyFilter(filter)
            .OrderByDescending(n => n.Time);

        return await query
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetMyNotificationsAsync(
        ContextSession session, 
        bool? read, 
        bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted)
            .Include(n => n.User)
            .Where(n => n.UserId == session.UserId);

        if (read.HasValue)
        {
            query = query.Where(n => n.Read == read.Value);
        }

        return await query
            .OrderByDescending(n => n.Time)
            .ToListAsync();
    }
}
