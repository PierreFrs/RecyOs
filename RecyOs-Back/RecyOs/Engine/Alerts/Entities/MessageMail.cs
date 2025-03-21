/// <summary>
/// Entité pour les messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Alerts.Entities;

public enum TaskStatus
{
    Pending,      // The task is pending
    Queued,       // The task is queued
    InProgress,   // The task is in progress
    Completed,    // The task is completed
    Failed        // The task has failed
}

public class MessageMail : DeletableEntity
{
    [Column("subject")]
    [MaxLength(255)]
    [Required]
    public string Subject { get; set; }

    [Column("from")]
    [MaxLength(255)]
    [Required]
    public string From { get; set; }

    [Column("to")]
    [MaxLength(255)]
    [Required]
    public string To { get; set; }

    // Use TaskStatus enum for Status with EF Core value converter
    [Column("status")]
    [Required]
    public TaskStatus Status { get; set; }

    [Column("priority")]
    [Required]
    public int Priority { get; set; }

    [Column("date_created")]
    [Required]
    public DateTime DateCreated { get; set; }

#nullable enable
    [Column("body")]
    public string? Body { get; set; }

    [Column("cc")]
    [MaxLength(255)]
    public string? Cc { get; set; }

    [Column("bcc")]
    [MaxLength(255)]
    public string? Bcc { get; set; }

    [Column("error")]
    public string? Error { get; set; }

    [Column("date_sent")]
    public DateTime? DateSent { get; set; }
#nullable disable
    
    // Implémentation de l'opérateur d'affectation
    public void CopyFrom(MessageMail source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        
        Subject = source.Subject;
        From = source.From;
        To = source.To;
        Status = source.Status;
        Priority = source.Priority;
        Body = source.Body;
        Cc = source.Cc;
        Bcc = source.Bcc;
        Error = source.Error;
        DateSent = source.DateSent;
    }
}