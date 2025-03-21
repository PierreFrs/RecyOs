// <copyright file="FactorClientEuropeBuDto.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;

namespace RecyOs.ORM.DTO.hub;

public class FactorClientEuropeBuDto : FactorClientBuDto
{
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return this.IdClient == ((FactorClientEuropeBuDto)obj).IdClient 
               && this.IdBu == ((FactorClientEuropeBuDto)obj).IdBu
               && this.IsDeleted == ((FactorClientEuropeBuDto)obj).IsDeleted;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(IdClient, IdBu, IsDeleted);
        
    }
}