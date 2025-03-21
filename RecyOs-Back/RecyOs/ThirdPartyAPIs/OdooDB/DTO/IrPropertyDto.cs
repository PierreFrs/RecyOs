// /** IrPropertyDto.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 16/07/2023
//  * Fichier Modifié le : 16/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.DTO;

public class IrPropertyDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ResId { get; set; }
    public long? CompanyId { get; set; }
    public long FieldsId { get; set; }
    public double? ValueFloat { get; set; }
    public int? ValueInteger { get; set; }
    public string ValueText { get; set; }
    public string ValueBinary { get; set; }
    public string ValueReference { get; set; }
    public DateTime? ValueDatetime { get; set; }
    public TypeIrPropertyOdoo Type { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string DisplayName { get; set; }
    public long? CreateUid { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? WriteUid { get; set; }
    public DateTime? WriteDate { get; set; }
}