/// <summary>
/// Service de fond pour l'envoi des emails.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NLog;
using RecyOs.Engine.Alerts.Interfaces;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Tools.Interfaces;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOs.Engine.Alerts;

public class EmailBackgroundService : BackgroundService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IEmailSender _emailSender;
        private readonly IEngineMessageMailService _messageMailService;
        private bool _isActive;
        private int _intervalSeconds;
        private readonly IMigrationStatusService _migrationStatusService;
        private readonly IEngineParametterService _parameterService;
        /// <summary>
        /// Constructeur de la classe EmailBackgroundService.
        /// </summary>
        /// <param name="emailSender">Service pour l'envoi des emails.</param>
        /// <param name="parameterService">Service pour la gestion des paramètres.</param>
        /// <param name="messageMailService">Service pour la gestion des messages de mail.</param>
        /// <param name="migrationStatusService">Service pour la gestion des migrations.</param>
        public EmailBackgroundService(IEmailSender emailSender, IEngineParametterService parameterService, IEngineMessageMailService messageMailService
                , IMigrationStatusService migrationStatusService)
        {
            _emailSender = emailSender;
            _messageMailService = messageMailService;
            _migrationStatusService = migrationStatusService;
            _parameterService = parameterService;
        }

        /// <summary>
        /// Exécute le service de fond pour l'envoi des emails.
        /// </summary>
        /// <param name="stoppingToken">Token de cancellation.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Attendre que les migrations soient terminées
            while (!_migrationStatusService.IsMigrationCompleted)
            {
                _logger.Info("Waiting for migrations to complete...");
                await Task.Delay(5000, stoppingToken); // Attendre 5 secondes avant de réessayer
            }
            
            Initialize();
            if (!_isActive)
            {
                _logger.Info("Email service is not active. Stopping execution.");
                return;
            }

            _logger.Info("Email service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Trace("Checking for unsent emails...");
                try
                {
                    // Get all emails that are not sent
                    var emails = await _messageMailService.GetUnsentEmails();
                    _logger.Info($"Found {emails.Count()} emails to send.");
                    // Send them
                    foreach (var email in emails)
                    {
                        bool res = await _emailSender.SendEmailAsync(email);
                        if(res)
                        {
                            email.Status = RecyOs.Engine.Alerts.Entities.TaskStatus.Completed;
                            email.Error = $"Message Sent with Message-ID: {email.Error}";
                            email.DateSent = DateTime.Now;
                            await _messageMailService.UpdateAsync(email);
                        }
                        else
                        {
                            email.Error = _emailSender.GetLastError();
                            email.Status = RecyOs.Engine.Alerts.Entities.TaskStatus.Failed;
                            await _messageMailService.UpdateAsync(email);
                            _logger.Error($"Email {email.Id} failed to send.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error while sending emails.");
                }
                try{
                    await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.Info("Task was canceled. Exiting loop.");
                    break;
                }
            }
            _logger.Info("Email service stopped.");
        }
    
        /// <summary>
        /// Méthode pour initialiser les paramètres du service.
        /// </summary>
        private void Initialize()
        {
            var result = _parameterService.GetByNomAsync("EmailBackgroundService", "IsActive").Result;
            if (result != null)
            {
                if(result.Type == "boolean")
                {
                    _isActive = bool.Parse(result.Valeur);
                }else{
                    _isActive = false;
                }
            } else {
                ParameterDto parameter = new()
                {
                    Module = "EmailBackgroundService",
                    Nom = "IsActive",
                    Valeur = "false",
                    Type = "boolean",
                    ControlType = "switch",
                    Label = "Activer le service pour l'envoi des emails",
                    CreateDate = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "Engine",
                    UpdatedBy = "Engine",
                    IsDeleted = false
                };
                var tsk = _parameterService.CreateAsync(parameter);
                tsk.GetAwaiter().GetResult();
                _isActive = false;
            }

            var interval = _parameterService.GetByNomAsync("EmailBackgroundService", "Interval").Result;
            if (interval != null)
            {
                if(interval.Type == "int")
                {
                    _intervalSeconds = int.Parse(interval.Valeur);
                }else{
                    
                    _intervalSeconds = 3600;
                }
                
            } else {
                ParameterDto parameter = new()
                    {
                        Module = "EmailBackgroundService",
                        Nom = "Interval",
                        Valeur = "3600",
                        Type = "int",
                        ControlType = "number",
                        Placeholder = "Intervale entre chaque vérification des emails non envoyés (en secondes)",
                        Label = "Intervale entre chaque vérification des emails non envoyés",
                        CreateDate = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "Engine",
                        UpdatedBy = "Engine",
                        IsDeleted = false,
                        PrefixIcon = "fa-solid fa-clock",
                    };
                var tsk = _parameterService.CreateAsync(parameter);
                tsk.GetAwaiter().GetResult();
                _intervalSeconds = 3600;
            }
        }    
}