/** INotificationService.cs - Interface pour le service des notifications
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 04/02/2025
 * Fichier Modifié le : 04/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface INotificationService
{
    Task<GridData<NotificationDto>> GetDataForGrid(NotificationFilter filter, bool includeDeleted = false);
    Task<NotificationDto> GetById(int id, bool includeDeleted = false);
    Task<bool> Delete(int id);
    Task<NotificationDto> Edit(NotificationDto dto);
    Task<NotificationDto> Create(NotificationDto dto);
    Task<IEnumerable<NotificationDto>> GetMyNotifications(bool? read = null, bool includeDeleted = false);
    Task<bool> MarkAsRead(int id);
    Task<bool> MarkAllAsRead();
    Task<int> GetUnreadCount();
}
