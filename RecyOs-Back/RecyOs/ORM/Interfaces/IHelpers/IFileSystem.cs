// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IFileSystem.cs
// Created : 2023/12/28 - 09:41
// Updated : 2023/12/28 - 09:41

using System.IO;

namespace RecyOs.ORM.Interfaces;

public interface IFileSystem
{
    bool DirectoryExists(string path);
    void CreateDirectory(string path);
    bool FileExists(string path);
    void DeleteFile(string path);
    Stream CreateFileStream(string path, FileMode mode);
}