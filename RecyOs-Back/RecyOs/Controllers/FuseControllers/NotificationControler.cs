/** NotificationController.cs - Contrôleur pour les notifications
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 04/02/2025
 * Fichier Modifié le : 04/02/2025
 * Code développé pour le projet : RecyOs
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("notifications")]
public class NotificationController : BaseApiController
{
    protected readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Récupère une notification par son ID
    /// </summary>
    /// <param name="id">ID de la notification</param>
    [SwaggerResponse(200, "Notification trouvée", typeof(NotificationDto))]
    [SwaggerResponse(404, "Notification non trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> Get(int id)
    {
        var notification = await _notificationService.GetById(id);
        if (notification == null) return NotFound();
        return Ok(notification);
    }

    /// <summary>
    /// Récupère la liste des notifications avec pagination
    /// </summary>
    [SwaggerResponse(200, "Liste des notifications", typeof(GridData<NotificationDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] NotificationFilter filter)
    {
        filter ??= new NotificationFilter();
        var notifications = await _notificationService.GetDataForGrid(filter);
        return Ok(notifications);
    }

    /// <summary>
    /// Récupère les notifications de l'utilisateur connecté
    /// </summary>
    [SwaggerResponse(200, "Notifications de l'utilisateur", typeof(IEnumerable<NotificationDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("my-notifications")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> GetMyNotifications([FromQuery] bool? read = null)
    {
        var notifications = await _notificationService.GetMyNotifications(read);
        return Ok(notifications);
    }

    /// <summary>
    /// Crée une nouvelle notification
    /// </summary>
    [SwaggerResponse(200, "Notification créée", typeof(NotificationDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] NotificationDto notificationDto)
    {
        var notification = await _notificationService.Create(notificationDto);
        return Ok(notification);
    }

    /// <summary>
    /// Marque une notification comme lue
    /// </summary>
    [SwaggerResponse(200, "Notification marquée comme lue")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Route("{id:int}/mark-as-read")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _notificationService.MarkAsRead(id);
        if (!result) return NotFound();
        return Ok();
    }

    /// <summary>
    /// Marque toutes les notifications comme lues
    /// </summary>
    [SwaggerResponse(200, "Toutes les notifications marquées comme lues")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Route("mark-all-as-read")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsRead();
        return Ok();
    }

    /// <summary>
    /// Récupère le nombre de notifications non lues
    /// </summary>
    [SwaggerResponse(200, "Nombre de notifications non lues", typeof(int))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("unread-count")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count = await _notificationService.GetUnreadCount();
        return Ok(count);
    }

    /// <summary>
    /// Supprime une notification
    /// </summary>
    [SwaggerResponse(200, "Notification supprimée")]
    [SwaggerResponse(404, "Notification non trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _notificationService.GetById(id);
        if (notification == null) return NotFound();
        await _notificationService.Delete(id);
        return Ok();
    }

    /// <summary>
    /// Met à jour une notification
    /// </summary>
    [SwaggerResponse(200, "Notification mise à jour")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> Update([FromBody] NotificationDto notificationDto)
    {
        var notification = await _notificationService.Edit(notificationDto);
        return Ok(notification);
    }
}
