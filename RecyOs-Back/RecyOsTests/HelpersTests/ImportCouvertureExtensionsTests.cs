using System.Globalization;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Moq;
using RecyOs.Commands;
using RecyOs.Helpers;
using Xunit.Sdk;

namespace RecyOsTests.HelpersTests;

[Collection("CouvertureTests")]
public class ImportCouvertureExtensionsTests
{
    ///////////////////////////////////////////////
    /// Tests for import parsing helper methods ///
    ///////////////////////////////////////////////
    
    [Fact]
    public void GetNullableDateTime_ShouldHandleParsedDateAndNull()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Date1";
        worksheet.Cell(1, 2).Value = "Date2";
        worksheet.Cell(1, 3).Value = "Date3";
        worksheet.Cell(1, 4).Value = "Date4";
        worksheet.Cell(1, 5).Value = "Date5";
        worksheet.Cell(1, 6).Value = "Date6";
        worksheet.Cell(1, 7).Value = "Date7";
        worksheet.Cell(1, 8).Value = "Date8";
        
        worksheet.Cell(2, 1).Value = "05/22/2023";
        worksheet.Cell(2, 2).Value = "2023/05/22";
        worksheet.Cell(2, 3).Value = "05-22-2023";
        worksheet.Cell(2, 4).Value = "2023-05-22";
        worksheet.Cell(2, 5).Value = "22/05/2023";
        worksheet.Cell(2, 6).Value = 22/05/2023;
        worksheet.Cell(2, 7).Value = "Data3";
        worksheet.Cell(2, 8).Value = "";
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var date1 = reader.GetNullableDateTime(columns, "Date1");
            var date2 = reader.GetNullableDateTime(columns, "Date2");
            var date3 = reader.GetNullableDateTime(columns, "Date3");
            var date4 = reader.GetNullableDateTime(columns, "Date4");
            var date5 = reader.GetNullableDateTime(columns, "Date5");
            var date6 = reader.GetNullableDateTime(columns, "Date6");
            var date7 = reader.GetNullableDateTime(columns, "Date7");
            var date8 = reader.GetNullableDateTime(columns, "Date8");
            
            Assert.NotNull(date1); // Expect a valid DateTime for "05/22/2023" (assuming mm/dd/yyyy format)
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date1.Value);
            Assert.NotNull(date2); // Expect a valid DateTime for "2023/05/22" (assuming yyyy/mm/dd format)
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date2.Value);
            Assert.NotNull(date3); // Expect a valid DateTime for "05-22-2023" (assuming mm-dd-yyyy format)
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date3.Value);
            Assert.NotNull(date4); // Expect a valid DateTime for "2023-05-22" (assuming yyyy-mm-dd format)
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date4.Value);
            Assert.Null(date5); // "22/05/2023" is invalid in mm/dd/yyyy format
            Assert.Null(date6); // Non-date string "Data3"
            Assert.Null(date7); // Empty string
            Assert.Null(date8); // No value provided
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetDateTime_ShouldHandleParsedDateAndThrowException()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Date1";
        worksheet.Cell(1, 2).Value = "Date2";
        worksheet.Cell(1, 3).Value = "Date3";
        worksheet.Cell(1, 4).Value = "Date4";
        worksheet.Cell(1, 5).Value = "Date5";
        worksheet.Cell(1, 6).Value = "Date6";
        worksheet.Cell(1, 7).Value = "Date7";
        worksheet.Cell(1, 8).Value = "Date8";
        
        worksheet.Cell(2, 1).Value = "05/22/2023";
        worksheet.Cell(2, 2).Value = "2023/05/22";
        worksheet.Cell(2, 3).Value = "05-22-2023";
        worksheet.Cell(2, 4).Value = "2023-05-22";
        worksheet.Cell(2, 5).Value = "22/05/2023";
        worksheet.Cell(2, 6).Value = 22/05/2023;
        worksheet.Cell(2, 7).Value = "Data3";
        worksheet.Cell(2, 8).Value = "";
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var date1 = reader.GetDateTime(columns, "Date1");
            var date2 = reader.GetDateTime(columns, "Date2");
            var date3 = reader.GetDateTime(columns, "Date3");
            var date4 = reader.GetDateTime(columns, "Date4");
            
            
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date1);
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date2);
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date3);
            Assert.Equal(new DateTime(2023, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), date4);
            Assert.Throws<FormatException>(() => reader.GetDateTime(columns, "Date5")); // "22/05/2023" is invalid in mm/dd/yyyy format
            Assert.Throws<FormatException>(() => reader.GetDateTime(columns, "Date6")); // Non-date string
            Assert.Throws<FormatException>(() => reader.GetDateTime(columns, "Date7")); // Empty string
            Assert.Throws<NullReferenceException>(() => reader.GetDateTime(columns, "Date8")); // No value provided
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }

    [Fact]
    public void GetNullableInt_ShouldHandleParsedIntAndNull()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Int1";
        worksheet.Cell(1, 2).Value = "Int2";
        worksheet.Cell(1, 3).Value = "Int3";
        worksheet.Cell(1, 4).Value = "Int4";
        worksheet.Cell(1, 5).Value = "Int5";
        worksheet.Cell(1, 6).Value = "Int6";
        worksheet.Cell(1, 7).Value = "Int7";
        worksheet.Cell(1, 8).Value = "Int8";
        worksheet.Cell(1, 9).Value = "Int9";
        
        worksheet.Cell(2, 1).Value = 0;
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "1";
        worksheet.Cell(2, 4).Value = "";
        worksheet.Cell(2, 5).Value = "abc";
        worksheet.Cell(2, 6).Value = "1,5";
        worksheet.Cell(2, 7).Value = 1.5;
        worksheet.Cell(2, 8).Value = "-4";
        worksheet.Cell(2, 9).Value = -4;
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var int1 = reader.GetNullableInt(columns, "Int1");
            var int2 = reader.GetNullableInt(columns, "Int2");
            var int3 = reader.GetNullableInt(columns, "Int3");
            var int4 = reader.GetNullableInt(columns, "Int4");
            var int5 = reader.GetNullableInt(columns, "Int5");
            var int6 = reader.GetNullableInt(columns, "Int6");
            var int7 = reader.GetNullableInt(columns, "Int7");
            var int8 = reader.GetNullableInt(columns, "Int8");
            var int9 = reader.GetNullableInt(columns, "Int9");
            
            Assert.NotNull(int1);
            Assert.Equal(0, int1.Value);
            Assert.NotNull(int2);
            Assert.Equal(1, int2.Value);
            Assert.NotNull(int3);
            Assert.Equal(1, int3.Value);
            Assert.Null(int4);
            Assert.Null(int5);
            Assert.Null(int6);
            Assert.Null(int7);
            Assert.Null(int8);
            Assert.Null(int9);
            
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetInt_ShouldHandleParsedIntAndThrowException()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Int1";
        worksheet.Cell(1, 2).Value = "Int2";
        worksheet.Cell(1, 3).Value = "Int3";
        worksheet.Cell(1, 4).Value = "Int4";
        worksheet.Cell(1, 5).Value = "Int5";
        worksheet.Cell(1, 6).Value = "Int6";
        worksheet.Cell(1, 7).Value = "Int7";
        worksheet.Cell(1, 8).Value = "Int8";
        worksheet.Cell(1, 9).Value = "Int9";
        
        worksheet.Cell(2, 1).Value = 0;
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "1";
        worksheet.Cell(2, 4).Value = "";
        worksheet.Cell(2, 5).Value = "abc";
        worksheet.Cell(2, 6).Value = "1,5";
        worksheet.Cell(2, 7).Value = 1.5;
        worksheet.Cell(2, 8).Value = "-4";
        worksheet.Cell(2, 9).Value = -4;
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            Assert.Equal(0, reader.GetInt(columns, "Int1"));
            Assert.Equal(1, reader.GetInt(columns, "Int2"));
            Assert.Equal(1, reader.GetInt(columns, "Int3"));
            Assert.Throws<NullReferenceException>(() => reader.GetInt(columns, "Int4"));
            Assert.Throws<FormatException>(() => reader.GetInt(columns, "Int5"));
            Assert.Throws<FormatException>(() => reader.GetInt(columns, "Int6"));
            Assert.Throws<FormatException>(() => reader.GetInt(columns, "Int7"));
            Assert.Throws<FormatException>(() => reader.GetInt(columns, "Int8"));
            Assert.Throws<FormatException>(() => reader.GetInt(columns, "Int9"));
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetNullableDecimal_ShouldHandleParsedDecimalAndNull()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Dec1";
        worksheet.Cell(1, 2).Value = "Dec2";
        worksheet.Cell(1, 3).Value = "Dec3";
        worksheet.Cell(1, 4).Value = "Dec4";
        worksheet.Cell(1, 5).Value = "Dec5";
        worksheet.Cell(1, 6).Value = "Dec6";
        worksheet.Cell(1, 7).Value = "Dec7";
        worksheet.Cell(1, 8).Value = "Dec8";
        worksheet.Cell(1, 9).Value = "Dec9";
        
        worksheet.Cell(2, 1).Value = 0;
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "1";
        worksheet.Cell(2, 4).Value = "1,5";
        worksheet.Cell(2, 5).Value = 1.5;
        worksheet.Cell(2, 6).Value = "";
        worksheet.Cell(2, 7).Value = "abc";
        worksheet.Cell(2, 8).Value = "-4";
        worksheet.Cell(2, 9).Value = -4;
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var dec1 = reader.GetNullableDecimal(columns, "Dec1");
            var dec2 = reader.GetNullableDecimal(columns, "Dec2");
            var dec3 = reader.GetNullableDecimal(columns, "Dec3");
            var dec4 = reader.GetNullableDecimal(columns, "Dec4");
            var dec5 = reader.GetNullableDecimal(columns, "Dec5");
            var dec6 = reader.GetNullableDecimal(columns, "Dec6");
            var dec7 = reader.GetNullableDecimal(columns, "Dec7");
            var dec8 = reader.GetNullableDecimal(columns, "Dec8");
            var dec9 = reader.GetNullableDecimal(columns, "Dec9");
            
            Assert.NotNull(dec1);
            Assert.Equal(0m, dec1.Value);
            Assert.NotNull(dec2);
            Assert.Equal(1m, dec2.Value);
            Assert.NotNull(dec3);
            Assert.Equal(1m, dec3.Value);
            Assert.NotNull(dec4);
            Assert.Equal(1.5m, dec4.Value);
            Assert.NotNull(dec5);
            Assert.Equal(1.5m, dec5.Value);
            Assert.Null(dec6);
            Assert.Null(dec7);
            Assert.Null(dec8);
            Assert.Null(dec9);
            
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetDecimal_ShouldHandleParsedDecimalAndThrowException()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Dec1";
        worksheet.Cell(1, 2).Value = "Dec2";
        worksheet.Cell(1, 3).Value = "Dec3";
        worksheet.Cell(1, 4).Value = "Dec4";
        worksheet.Cell(1, 5).Value = "Dec5";
        worksheet.Cell(1, 6).Value = "Dec6";
        worksheet.Cell(1, 7).Value = "Dec7";
        worksheet.Cell(1, 8).Value = "Dec8";
        worksheet.Cell(1, 9).Value = "Dec9";
        
        worksheet.Cell(2, 1).Value = 0;
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "1";
        worksheet.Cell(2, 4).Value = "1,5";
        worksheet.Cell(2, 5).Value = 1.5;
        worksheet.Cell(2, 6).Value = "";
        worksheet.Cell(2, 7).Value = "abc";
        worksheet.Cell(2, 8).Value = "-4";
        worksheet.Cell(2, 9).Value = -4;
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            Assert.Equal(0m, reader.GetDecimal(columns, "Dec1"));
            Assert.Equal(1m, reader.GetDecimal(columns, "Dec2"));
            Assert.Equal(1m, reader.GetDecimal(columns, "Dec3"));
            Assert.Equal(1.5m, reader.GetDecimal(columns, "Dec4"));
            Assert.Equal(1.5m, reader.GetDecimal(columns, "Dec5"));
            Assert.Throws<FormatException>(() => reader.GetDecimal(columns, "Dec6"));
            Assert.Throws<FormatException>(() => reader.GetDecimal(columns, "Dec7"));
            Assert.Throws<FormatException>(() => reader.GetDecimal(columns, "Dec8"));
            Assert.Throws<FormatException>(() => reader.GetDecimal(columns, "Dec9"));
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetNullableString_ShouldHandleParsedStringAndNull()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Str1";
        worksheet.Cell(1, 2).Value = "Str2";
        worksheet.Cell(1, 3).Value = "Str3";

        
        worksheet.Cell(2, 1).Value = "Test";
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "";

        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var str1 = reader.GetNullableString(columns, "Str1");
            var str2 = reader.GetNullableString(columns, "Str2");
            var str3 = reader.GetNullableString(columns, "Str3");
            
            Assert.NotNull(str1);
            Assert.Equal("Test", str1);
            Assert.Null(str2);
            Assert.Null(str3);
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public void GetString_ShouldHandleParsedStringAndThrowException()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "Str1";
        worksheet.Cell(1, 2).Value = "Str2";
        worksheet.Cell(1, 3).Value = "Str3";

        
        worksheet.Cell(2, 1).Value = "Test";
        worksheet.Cell(2, 2).Value = 1;
        worksheet.Cell(2, 3).Value = "";
        
        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        reader.Read();
        
        Dictionary<string, int> columns = new Dictionary<string, int>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string? columnName = reader.GetValue(i).ToString();
            columns[columnName] = i;
        }
        
        // Act and Assert
        while (reader.Read())
        {
            var str1 = reader.GetString(columns, "Str1");
            var str2 = reader.GetString(columns, "Str2");
            
            Assert.NotNull(str1);
            Assert.Equal("Test", str1);
            Assert.Equal("1", str2);
            Assert.Throws<NullReferenceException>(() => reader.GetString(columns, "Str3"));
        }
        
        // Cleanup
        fileStream.Close();
        File.Delete(tempFileName);
    }
        
    //////////////////////////////////////////////
    /// Tests for check parsing helper methods ///
    //////////////////////////////////////////////

    [Fact]
    public void IsNullOrWhiteSpaceOrInvalidDate_ShouldHandleInvalidDates()
    {
        // Arrange
        var validDate = "2023-05-22";
        var invalidDate = "22/05/2023";
        var emptyDate = "";
        var nullDate = (string?)null;
        var whitespaceDate = " ";

        // Act and Assert
        Assert.False(ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(validDate));
        Assert.True(ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(invalidDate));
        Assert.False(ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(emptyDate));
        Assert.False(ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(nullDate));
        Assert.False(ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(whitespaceDate));
    }

    [Fact]
    public void ValidateDateTime_ShouldValidateDateFormats()
    {
        // Arrange
        var validValue = "2023-05-22";
        var invalidValue = "22/05/2023";
        string[] dateFormats = {"yyyy-MM-dd", "yyyy/MM/dd"};
        
        // Act and Assert
        Assert.True(ImportCouvertureExtensions.ValidateDateTime(validValue, dateFormats));
        Assert.False(ImportCouvertureExtensions.ValidateDateTime(invalidValue, dateFormats));
    }

    [Fact]
    public void ValidateInt_ShouldValidateIntegersPassedAsStrings()
    {
        // Arrange
        var validString = "1";
        var invalidNumber = "1,5";
        var invalidString = "abc";
        var emptyString = "";
        
        // Act and Assert
        Assert.True(ImportCouvertureExtensions.ValidateInt(validString));
        Assert.False(ImportCouvertureExtensions.ValidateInt(invalidNumber));
        Assert.False(ImportCouvertureExtensions.ValidateInt(invalidString));
        Assert.False(ImportCouvertureExtensions.ValidateInt(emptyString));
    }
    
    [Fact]
    public void ValidateDecimal_ShouldValidateDecimalsPassedAsStrings()
    {
        // Arrange
        var validString = "1,5";
        var invalidString = "abc";
        var emptyString = "";
        
        // Act and Assert
        Assert.True(ImportCouvertureExtensions.ValidateDecimal(validString));
        Assert.False(ImportCouvertureExtensions.ValidateDecimal(invalidString));
        Assert.False(ImportCouvertureExtensions.ValidateDecimal(emptyString));
    }
    
    [Fact]
    public void ValidateTimeSpan_ShouldValidateTimeSpan()
    {
        // Arrange
        var format = "hh\\:mm\\:ss";
        var validString = "18:54:03";
        var invalidString = "abc";
        var emptyString = "";
        
        // Act and Assert
        Assert.True(ImportCouvertureExtensions.ValidateTimeSpan(validString, format));
        Assert.False(ImportCouvertureExtensions.ValidateTimeSpan(invalidString, format));
        Assert.False(ImportCouvertureExtensions.ValidateTimeSpan(emptyString, format));
    }
}