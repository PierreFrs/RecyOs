using System.Threading.Tasks;
using RecyOs.Engine.Alerts.DTO;

namespace RecyOs.Engine.Tools.Interfaces;
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(MessageMailDto email);
        string GetLastError();
    }
