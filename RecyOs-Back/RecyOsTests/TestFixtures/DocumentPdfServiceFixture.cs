// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfServiceFixture.cs
// Created : 2023/12/26 - 10:12
// Updated : 2023/12/26 - 10:12

using RecyOsTests.TestsHelpers;

namespace RecyOsTests.TestFixtures;

public class DocumentPdfServiceFixture
{
    public DocumentPdfTestsHelpers DocumentPdfTestsHelpers { get; }

    public DocumentPdfServiceFixture()
    {
        DocumentPdfTestsHelpers = new DocumentPdfTestsHelpers();
    }
}