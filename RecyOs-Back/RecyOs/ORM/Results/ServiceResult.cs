// <copyright file="ServiceResults.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

namespace RecyOs.ORM.Results;

public class ServiceResult
{
    public bool Success { get; set; } = false;
    public int StatusCode { get; set; } = 400;
    public string Message { get; set; } = string.Empty;
    public object Data { get; set; } = new object();
}