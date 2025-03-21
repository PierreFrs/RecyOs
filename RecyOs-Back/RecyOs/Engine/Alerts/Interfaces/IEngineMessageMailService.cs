/// <summary>
/// Interface pour le service des messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Alerts.Entities;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOs.Engine.Alerts.Interfaces;

public interface IEngineMessageMailService
{
    Task<IEnumerable<MessageMailDto>> GetPendingMessagesAsync();
    Task<IEnumerable<MessageMailDto>> GetByStatusAsync(TaskStatus status);
    Task<IEnumerable<MessageMailDto>> GetFailedMessagesAsync();
    Task UpdateStatusAsync(int id, TaskStatus newStatus);
    Task<MessageMailDto> CreateAsync(MessageMailDto messageDto);
    Task<MessageMailDto> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<IEnumerable<MessageMailDto>> GetUnsentEmails();
    Task<MessageMailDto> UpdateAsync(MessageMailDto messageDto);
}
