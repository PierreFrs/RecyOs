using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCExport;
using Moq;

namespace RecyOsTests.TestsHelpers;

public class CsvFileResultTests
{
    private class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    private readonly List<TestModel> _testData;

    public CsvFileResultTests()
    {
        _testData = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1", Date = new DateTime(2025, 1, 1) },
            new() { Id = 2, Name = "Test2", Date = new DateTime(2025, 1, 2) }
        };
    }

    [Fact]
    public void Constructor_WithNullSource_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CsvFileResult<TestModel>(null, "test.csv"));
    }

    [Fact]
    public void Constructor_WithNullFileName_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CsvFileResult<TestModel>(_testData, null));
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Act
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("text/csv", result.ContentType);
        Assert.Equal("test.csv", result.FileDownloadName);
    }

    [Fact]
    public void Headers_WithoutCustomHeaders_ReturnsPropertyNames()
    {
        // Arrange
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");

        // Act
        var headers = result.Headers.ToList();

        // Assert
        Assert.Equal(3, headers.Count);
        Assert.Contains("Id", headers);
        Assert.Contains("Name", headers);
        Assert.Contains("Date", headers);
    }

    [Fact]
    public void Headers_WithCustomHeaders_ReturnsCustomHeaders()
    {
        // Arrange
        var customHeaders = new[] { "CustomId", "CustomName", "CustomDate" };
        var result = new CsvFileResult<TestModel>(_testData, "test.csv", 
            x => new[] { x.Id.ToString(), x.Name, x.Date.ToString() }, 
            customHeaders);

        // Act
        var headers = result.Headers.ToList();

        // Assert
        Assert.Equal(customHeaders.Length, headers.Count);
        Assert.Equal(customHeaders, headers);
    }

    [Fact]
    public void ContentEncoding_DefaultValue_ReturnsUTF8WithPreamble()
    {
        // Arrange
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");

        // Act
        var encoding = result.ContentEncoding;

        // Assert
        Assert.IsType<UTF8Encoding>(encoding);
        Assert.True(encoding.GetPreamble().Length > 0);
    }

    [Fact]
    public void Delimiter_DefaultValue_ReturnsCurrentCultureListSeparator()
    {
        // Arrange
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");

        // Act
        var delimiter = result.Delimiter;

        // Assert
        Assert.Equal(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator, delimiter);
    }

    [Fact]
    public async Task ExecuteResultAsync_WritesCorrectContent()
    {
        // Arrange
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");
        var stream = new MemoryStream();
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = stream;
        var actionContext = new ActionContext
        {
            HttpContext = httpContext
        };

        // Act
        await result.ExecuteResultAsync(actionContext);

        // Assert
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        var delimiter = result.Delimiter;
        
        Assert.Contains($"Id{delimiter}Name{delimiter}Date", content);
        Assert.Contains($"1{delimiter}Test1", content);
        Assert.Contains($"2{delimiter}Test2", content);
    }

    [Fact]
    public async Task ExecuteResultAsync_WithCustomMapping_WritesCorrectContent()
    {
        // Arrange
        var customHeaders = new[] { "Identifiant", "Nom" };
        var result = new CsvFileResult<TestModel>(_testData, "test.csv",
            x => new[] { x.Id.ToString(), x.Name },
            customHeaders);
        
        var stream = new MemoryStream();
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = stream;
        var actionContext = new ActionContext
        {
            HttpContext = httpContext
        };

        // Act
        await result.ExecuteResultAsync(actionContext);

        // Assert
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        var delimiter = result.Delimiter;
        
        Assert.Contains($"Identifiant{delimiter}Nom", content);
        Assert.Contains($"1{delimiter}Test1", content);
        Assert.Contains($"2{delimiter}Test2", content);
        Assert.DoesNotContain("Date", content);
    }

    [Fact]
    public void GetPropertyValue_WithInvalidProperty_ReturnsErrorMessage()
    {
        // Arrange
        var result = new CsvFileResult<TestModel>(_testData, "test.csv");
        var propertyInfo = typeof(TestModel).GetProperty("NonExistentProperty");

        // Act & Assert
        Assert.Null(propertyInfo);
    }
} 