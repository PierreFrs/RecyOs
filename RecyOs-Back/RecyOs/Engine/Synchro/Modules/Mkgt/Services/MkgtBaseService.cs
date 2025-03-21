//  MkgtBaseService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.Engine.Services;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Modules.Mkgt.Services;

public abstract class MkgtBaseService<TDestination> : BaseDataService<TDestination> where TDestination : class
{
    protected MkgtBaseService() : base()
    {
        
    }
}
