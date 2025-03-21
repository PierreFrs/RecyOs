// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IImportNDCover.cs
// Created : 2023/12/19 - 11:09
// Updated : 2023/12/19 - 11:09

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RecyOs.Controllers;

public interface ICommandImportNDCover
{
    Task<bool> Import(IFormFile file);
    Task<bool> CheckFormat(IFormFile file);
    Task ImportNdCoverErrorAsync(IFormFile file);
}