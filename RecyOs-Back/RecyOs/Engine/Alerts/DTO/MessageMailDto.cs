/// <summary>
/// DTO pour les messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using System;
using RecyOs.Engine.Alerts.Entities;

namespace RecyOs.Engine.Alerts.DTO;

public class MessageMailDto
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public TaskStatus Status { get; set; }
    public int Priority { get; set; }
    public DateTime DateCreated { get; set; }
#nullable enable
    public string? Body { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string? Error { get; set; }
    public DateTime? DateSent { get; set; }
#nullable disable
}