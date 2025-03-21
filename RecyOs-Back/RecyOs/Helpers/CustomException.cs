// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CustomExceptions.cs
// Created : 2024/01/03 - 13:58
// Updated : 2024/01/03 - 13:58

using System;

namespace RecyOs.Helpers;

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}
public class EntityNotFoundException : CustomException
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
}