/// <summary>
/// Service pour les messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Engine.Alerts.Interfaces;
using RecyOs.Engine.Alerts.Mappers;
using RecyOs.Engine.Alerts.Repositories;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOs.Engine.Alerts.Services;

public class EngineMessageMailService : IEngineMessageMailService
{
    private readonly IEngineMessageMailRepository _messageMailRepository;
    private readonly IMapper _mapper;

    public EngineMessageMailService(IEngineMessageMailRepository messageMailRepository, IMapper mapper)
    {
        _messageMailRepository = messageMailRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Récupère les messages de mail en attente.
    /// </summary>
    /// <returns>Liste des messages de mail en attente.</returns>
    public async Task<IEnumerable<MessageMailDto>> GetPendingMessagesAsync()
    {
        var messages = await _messageMailRepository.GetPendingMessagesAsync();
        return _mapper.Map<IEnumerable<MessageMailDto>>(messages);
    }

    /// <summary>
    /// Récupère les messages de mail par statut.
    /// </summary>
    /// <param name="status">Statut des messages de mail.</param>
    /// <returns>Liste des messages de mail par statut.</returns>
    public async Task<IEnumerable<MessageMailDto>> GetByStatusAsync(TaskStatus status)
    {
        var messages = await _messageMailRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<MessageMailDto>>(messages);
    }

    /// <summary>
    /// Récupère les messages de mail en échec.
    /// </summary>
    /// <returns>Liste des messages de mail en échec.</returns>
    public async Task<IEnumerable<MessageMailDto>> GetFailedMessagesAsync()
    {
        var messages = await _messageMailRepository.GetFailedMessagesAsync();
        return _mapper.Map<IEnumerable<MessageMailDto>>(messages);
    }

    /// <summary>
    /// Met à jour le statut d'un message de mail.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    /// <param name="newStatus">Nouveau statut du message de mail.</param>
    public async Task UpdateStatusAsync(int id, TaskStatus newStatus)
    {
        await _messageMailRepository.UpdateStatusAsync(id, newStatus);
    }

    /// <summary>
    /// Crée un message de mail.
    /// </summary>
    /// <param name="messageDto">Message de mail à créer.</param>
    /// <returns>Message de mail créé.</returns>
    public async Task<MessageMailDto> CreateAsync(MessageMailDto messageDto)
    {
        var entity = _mapper.Map<MessageMail>(messageDto);
        var createdEntity = await _messageMailRepository.CreateAsync(entity);
        return _mapper.Map<MessageMailDto>(createdEntity);
    }

    /// <summary>
    /// Récupère un message de mail par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    /// <returns>Message de mail trouvé.</returns>
    public async Task<MessageMailDto> GetByIdAsync(int id)
    {
        var message = await _messageMailRepository.GetByIdAsync(id);
        return message != null ? _mapper.Map<MessageMailDto>(message) : null;
    }

    /// <summary>
    /// Supprime un message de mail.
    /// </summary>
    /// <param name="id">Identifiant du message de mail.</param>
    public async Task DeleteAsync(int id)
    {
        await _messageMailRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Récupère les messages de mail non envoyés.
    /// </summary>
    /// <returns>Liste des messages de mail non envoyés.</returns>
    public async Task<IEnumerable<MessageMailDto>> GetUnsentEmails()
    {
        var messages = await _messageMailRepository.GetByStatusAsync(TaskStatus.Pending);
        return _mapper.Map<IEnumerable<MessageMailDto>>(messages);
    }

    /// <summary>
    /// Met à jour un message de mail.
    /// </summary>
    /// <param name="messageDto">Message de mail à mettre à jour.</param>
    /// <returns>Message de mail mis à jour.</returns>
    public async Task<MessageMailDto> UpdateAsync(MessageMailDto messageDto)
    {
        var entity = _mapper.Map<MessageMail>(messageDto);
        var updatedEntity = await _messageMailRepository.UpdateAsync(entity);
        return _mapper.Map<MessageMailDto>(updatedEntity);
    }
}