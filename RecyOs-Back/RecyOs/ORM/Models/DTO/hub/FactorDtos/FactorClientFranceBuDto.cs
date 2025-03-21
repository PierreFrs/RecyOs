// <copyright file="FactorClientFranceBuDto.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;

namespace RecyOs.ORM.DTO.hub;

public class FactorClientFranceBuDto : FactorClientBuDto
{
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return this.IdClient == ((FactorClientFranceBuDto)obj).IdClient 
               && this.IdBu == ((FactorClientFranceBuDto)obj).IdBu
               && this.IsDeleted == ((FactorClientFranceBuDto)obj).IsDeleted;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(IdClient, IdBu, IsDeleted);
        
    }
}