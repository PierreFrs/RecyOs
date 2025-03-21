// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IExportSoumissionNDCover.cs
// Created : 2024/01/23 - 11:26
// Updated : 2024/01/23 - 11:26


using System.Net;
using System.Threading.Tasks;
using ClosedXML.Excel;
using RecyOs.ORM.Entities;

namespace RecyOs.Controllers;

public interface ICommandExportSoumissionNDCoverService
{
    Task<XLWorkbook> ExportSoumissionNDCoverFranceAsync();
}