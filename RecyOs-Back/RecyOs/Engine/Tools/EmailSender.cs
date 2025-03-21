using System.Threading.Tasks;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Tools.Interfaces;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO;
using System.Net.Mail;
using NLog;
using System;

namespace RecyOs.Engine.Tools;

public class EmailSender : IEmailSender
    {
        private  string _smtpClient;
        private  int _smtpPort;
        private  string _smtpUsername;
        private  string _smtpPassword;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private  bool _enableSsl;
        private string lastError = "";
        private readonly IEngineParametterService _parameterService;
        
        public EmailSender(IEngineParametterService parameterService)
        {
            _parameterService = parameterService;
           
        }

        public async Task<bool> SendEmailAsync(MessageMailDto email)
        {
            Initialize();
            _logger.Trace($"Attempting to send email to {email.To}");
            try
            {
                using (var smtp = new SmtpClient(_smtpClient, _smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword);
                    smtp.EnableSsl = _enableSsl;
                    var mailMessage = new MailMessage(email.From, email.To)
                    {
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = true
                    };

                    if (!string.IsNullOrEmpty(email.Cc))
                        mailMessage.CC.Add(email.Cc);
                    if (!string.IsNullOrEmpty(email.Bcc))
                        mailMessage.Bcc.Add(email.Bcc);

                    // Ajouter un Message-ID unique
                    string messageId = $"<{Guid.NewGuid()}@recygroup.fr>";
                    mailMessage.Headers.Add("Message-ID", messageId);
                    email.Error = messageId; // Stocker temporairement l'ID pour le retour

                    mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;

                    smtp.SendCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                        {
                            _logger.Error(e.Error, $"SMTP error while sending email to {email.To}");
                            email.Error = e.Error.Message;
                            lastError = e.Error.Message;
                        }
                        else
                        {
                            _logger.Trace($"SMTP server successfully processed email to {email.To}");
                        }
                    };

                    await smtp.SendMailAsync(mailMessage);
                    _logger.Trace($"Email sent successfully to {email.To}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to send email to {email.To}");
                email.Error = ex.Message;
                lastError = ex.Message;
                return false;
            }
        }

        public string GetLastError()
        {
            return lastError;
        }

        private void Initialize()
        {
       var smtp = _parameterService.GetByNomAsync("EmailSender", "SMTPServer").Result;
           if (smtp != null)
           {
                _smtpClient = smtp.Valeur;
           } else {
            ParameterDto parameter = new()
            {
                Module = "EmailSender",
                Nom = "SMTPServer",
                Valeur = "localhost",
                Type = "string",
                ControlType = "text",
                Placeholder = "Adresse du serveur SMTP",
                Label = "Adresse du serveur SMTP",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = _parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _smtpClient = "localhost";
           }

           var smtpPort = _parameterService.GetByNomAsync("EmailSender", "SMTPPort").Result;
           if (smtpPort != null)
           {
                _smtpPort = int.Parse(smtpPort.Valeur);
           } else {
            ParameterDto parameter = new()
            {
                Module = "EmailSender",
                Nom = "SMTPPort",
                Valeur = "25",
                Type = "int",
                ControlType = "number",
                Placeholder = "Port du serveur SMTP",
                Label = "Port du serveur SMTP",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = _parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _smtpPort = 25;
           }

           var smtpUsername = _parameterService.GetByNomAsync("EmailSender", "SMTPUsername").Result;
           if (smtpUsername != null)
           {
                _smtpUsername = smtpUsername.Valeur;
           }else {
            ParameterDto parameter = new()
            {
                Module = "EmailSender",
                Nom = "SMTPUsername",
                Valeur = "user",
                Type = "string",
                ControlType = "text",
                Placeholder = "Nom d'utilisateur du serveur SMTP",
                Label = "Nom d'utilisateur du serveur SMTP",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = _parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _smtpUsername = "user";
           }

           var smtpPassword = _parameterService.GetByNomAsync("EmailSender", "SMTPPassword").Result;
           if (smtpPassword != null)
           {
                _smtpPassword = smtpPassword.Valeur;
           }else {
            ParameterDto parameter = new()
            {
                Module = "EmailSender",
                Nom = "SMTPPassword",
                Valeur = "password",
                Type = "string",
                ControlType = "password",
                Placeholder = "Mot de passe du serveur SMTP",
                Label = "Mot de passe du serveur SMTP",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = _parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _smtpPassword = "password";
           }

           var enableSsl = _parameterService.GetByNomAsync("EmailSender", "EnableSsl").Result;
           if (enableSsl != null)
           {
                _enableSsl = bool.Parse(enableSsl.Valeur);
           }else {
            ParameterDto parameter = new()
            {
                Module = "EmailSender",
                Nom = "EnableSsl",
                Valeur = "true",
                Type = "boolean",
                ControlType = "switch",
                Placeholder = "Activer le SSL",
                Label = "Activer le SSL",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = _parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _enableSsl = true;
           }
           _logger.Trace($"EmailSender initialized with SMTP server: {_smtpClient}, port: {_smtpPort}, username: {_smtpUsername}, password: ***************");
        }
    }
