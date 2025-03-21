/// <summary>
/// Interface pour le repository des messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.Engine.Alerts.Entities;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;


namespace RecyOs.Engine.Alerts.Repositories;

public interface IEngineMessageMailRepository
{
    Task<IEnumerable<MessageMail>> GetPendingMessagesAsync();
    Task<IEnumerable<MessageMail>> GetByStatusAsync(TaskStatus status);
    Task<IEnumerable<MessageMail>> GetFailedMessagesAsync();
    Task UpdateStatusAsync(int id, TaskStatus newStatus);
    Task<MessageMail> CreateAsync(MessageMail entity);
    Task<MessageMail> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<MessageMail> UpdateAsync(MessageMail entity);
}