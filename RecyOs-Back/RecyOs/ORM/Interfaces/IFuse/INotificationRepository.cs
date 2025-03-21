/** INotificationRepository.cs - Interface pour le repository des notifications
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 03/02/2025
 * Fichier Modifié le : 03/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface INotificationRepository<TNotification> where TNotification : Notification
{
    Task<Notification> GetAsync(int id, ContextSession session, bool includeDeleted = false);
    Task<IEnumerable<TNotification>> GetListAsync(ContextSession session, bool includeDeleted = false);
    Task<TNotification> CreateAsync(TNotification notification, ContextSession session, bool includeDeleted = false);
    Task<TNotification> UpdateAsync(TNotification notification, ContextSession session);
    Task DeleteAsync(int id, ContextSession session);
    Task<IEnumerable<TNotification>> GetFilteredNotificationsWithCountAsync(NotificationFilter filter, ContextSession session, bool includeDeleted = false);
    Task<IEnumerable<TNotification>> GetMyNotificationsAsync(ContextSession session, bool? read, bool includeDeleted = false);
}
