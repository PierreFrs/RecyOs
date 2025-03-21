using System;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces.IParameters;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Startup;

public class ShipperDashdocSettings : IShipperDashdocSettings
{
    private readonly string _uri;
    private readonly string _token;
    
    public ShipperDashdocSettings(IEngineParametterService parameterService)
    {
        var token = parameterService.GetByNomAsync("Dashdoc", "ShipperToken").Result;
        if (token != null)
        {
            _token = token.Valeur;
        }
        else
        {
            ParameterDto parameter = new()
            {
                Module = "Dashdoc",
                Nom = "ShipperToken",
                Valeur = "Token",
                Type = "String",
                ControlType = "text",
                Placeholder = "Token expéditeur",
                Label = "Token expéditeur",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _token = "Token";
        }
        var uri = parameterService.GetByNomAsync("Dashdoc", "ShipperEndPoint").Result;
        if (uri != null)
        {
            _uri = uri.Valeur;
        }
        else
        {
            ParameterDto parameter = new()
            {
                Module = "Dashdoc",
                Nom = "ShipperEndPoint",
                Valeur = "EndPoint",
                Type = "String",
                ControlType = "url",
                Placeholder = "Url de l'API",
                Label = "Url API expéditeur",
                CreateDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "Engine",
                UpdatedBy = "Engine",
                IsDeleted = false
            };
            var tsk = parameterService.CreateAsync(parameter);
            tsk.GetAwaiter().GetResult();
            _uri = "EndPoint";
        }
    }
    
    public string GetUri()
    {
        return _uri;
    }

    public string GetToken()
    {
        return _token;
    }
}