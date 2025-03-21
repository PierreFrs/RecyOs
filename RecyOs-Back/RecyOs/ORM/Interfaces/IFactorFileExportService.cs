// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>IFactorFileExportService.cs
// Created : 2024/05/2121 - 09:05

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RecyOs.ORM.Interfaces;

public interface IFactorFileExportService
{
    Task<FileResult> ExportFactorFileAsync();
}