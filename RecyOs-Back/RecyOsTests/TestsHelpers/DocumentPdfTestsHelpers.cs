// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfTestsHelpers.cs
// Created : 2023/12/26 - 09:45
// Updated : 2023/12/26 - 09:45

namespace RecyOsTests.TestsHelpers;

public class DocumentPdfTestsHelpers
{
    // Helper method to create a unique test file path
    public string CreateTestFilePath(string directory, string fileName)
    {
        string testFileGuid = Guid.NewGuid().ToString();
        string testFilePath = Path.Combine("TestFiles", $"{testFileGuid}_{fileName}");
        return Path.Combine(directory, testFilePath);
    }
}