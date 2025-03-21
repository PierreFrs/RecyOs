// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfEuropeServiceFixture.cs
// Created : 2023/12/26 - 12:19
// Updated : 2023/12/26 - 12:19

using RecyOsTests.TestsHelpers;

namespace RecyOsTests.TestFixtures;

public class DocumentPdfEuropeServiceFixture
{
    public DocumentPdfTestsHelpers DocumentPdfTestsHelpers { get; }

    public DocumentPdfEuropeServiceFixture()
    {
        DocumentPdfTestsHelpers = new DocumentPdfTestsHelpers();
    }
}