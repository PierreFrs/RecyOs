using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.Controllers;

public class BaseOdooControler: BaseApiController
{
    private readonly IBaseOdooService _baseOdooService;
    
    public BaseOdooControler(IBaseOdooService baseOdooService)
    {
        _baseOdooService = baseOdooService;
    }
    
    [HttpGet("model/{modelName}")]
    public async Task<String> GetModelAsync(string modelName)
    {
        return await _baseOdooService.GetModelAsync(modelName);
    }
}