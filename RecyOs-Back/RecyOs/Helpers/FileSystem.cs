// Created by : Pierre FRAISSE
// RecyOs => RecyOs => FileSystem.cs
// Created : 2023/12/28 - 09:41
// Updated : 2023/12/28 - 09:41

using System.IO;
using RecyOs.ORM.Interfaces;

namespace RecyOs.Helpers;

public class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);
    public void CreateDirectory(string path) => Directory.CreateDirectory(path);
    public bool FileExists(string path) => File.Exists(path);
    public void DeleteFile(string path) => File.Delete(path);
    public Stream  CreateFileStream(string path, FileMode mode) => new FileStream(path, mode);
}