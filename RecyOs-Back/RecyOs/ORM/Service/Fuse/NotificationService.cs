/** NotificationService.cs - Service pour les notifications
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 04/02/2025
 * Fichier Modifié le : 04/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class NotificationService<TNotification> : BaseService, INotificationService
    where TNotification : Notification, new()
{
    protected readonly INotificationRepository<TNotification> _repository;
    private readonly IMapper _mapper;

    public NotificationService(
        ICurrentContextProvider contextProvider, 
        INotificationRepository<TNotification> repository,
        IMapper mapper) : base(contextProvider)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GridData<NotificationDto>> GetDataForGrid(NotificationFilter filter, bool includeDeleted = false)
    {
        var notifications = await _repository.GetFilteredNotificationsWithCountAsync(filter, Session, includeDeleted);
        var count = notifications.Count();
        var ratio = count / (double)filter.PageSize;
        var begin = filter.PageNumber * filter.PageSize;

        return new GridData<NotificationDto>
        {
            Items = _mapper.Map<IEnumerable<NotificationDto>>(notifications),
            Paginator = new Pagination()
            {
                length = count,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio), 1.0),
                startIndex = begin,
            }
        };
    }

    public async Task<NotificationDto> GetById(int id, bool includeDeleted = false)
    {
        var notification = await _repository.GetAsync(id, Session, includeDeleted);
        return _mapper.Map<NotificationDto>(notification);
    }

    public async Task<bool> Delete(int id)
    {
        await _repository.DeleteAsync(id, Session);
        return true;
    }

    public async Task<NotificationDto> Edit(NotificationDto dto)
    {
        var notification = _mapper.Map<TNotification>(dto);
        notification = await _repository.UpdateAsync(notification, Session);
        return _mapper.Map<NotificationDto>(notification);
    }

    public async Task<NotificationDto> Create(NotificationDto dto)
    {
        var notification = _mapper.Map<TNotification>(dto);
        notification.Time = DateTime.UtcNow;
        notification.Read = false;
        notification = await _repository.CreateAsync(notification, Session, false);
        return _mapper.Map<NotificationDto>(notification);
    }

    public async Task<IEnumerable<NotificationDto>> GetMyNotifications(bool? read = null, bool includeDeleted = false)
    {
        var notifications = await _repository.GetMyNotificationsAsync(Session, read, includeDeleted);
        return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<bool> MarkAsRead(int id)
    {
        var notification = await this.GetById(id);
        var notificationEntity = _mapper.Map<TNotification>(notification);
        if (notification != null && notification.UserId == Session.UserId)
        {
            notification.Read = true;
            await _repository.UpdateAsync(notificationEntity, Session);
            return true;
        }
        return false;
    }

    public async Task<bool> MarkAllAsRead()
    {
        var notifications = await _repository.GetMyNotificationsAsync(Session, false);
        foreach (var notification in notifications)
        {
            notification.Read = true;
            await _repository.UpdateAsync(notification, Session);
        }
        return true;
    }

    public async Task<int> GetUnreadCount()
    {
        var notifications = await _repository.GetMyNotificationsAsync(Session, false);
        return notifications.Count();
    }
}
