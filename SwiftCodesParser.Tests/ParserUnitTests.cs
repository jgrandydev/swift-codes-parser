using System;
using System.IO;
using System.Text;

using NUnit.Framework;
using SwiftCodesParser.Parse;
using SwiftCodesParser.Persist;

namespace SwiftCodesParser.Tests
{
    [TestFixture]
    public class ParserUnitTests
    {
        private const string TestFilesFolder = "test-files";

        [TestCase("545", "MT545-22-Indicator.html")]
        [TestCase("545", "MT545-94B-Place.html")]
        [TestCase("545", "MT545-22F-LinkageTypeIndicator.html")]
        [TestCase("545", "MT545-98-DateTime.html")]
        [TestCase("545", "MT545-90-DealPrice.html")]
        [TestCase("548", "MT548-22-Indicator.html")]
        [TestCase("548", "MT548-25D-Status.html")]
        [TestCase("548", "MT548-24B-Reason.html")]
        [TestCase("548", "MT548-94-Place.html")]
        [TestCase("548", "MT548-95-Party.html")]
        public void ParseHtml(string messageType, string file)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilesFolder, file);

            string html;
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                html = reader.ReadToEnd();
            }

            var parser = new Parser();
            var codes = parser.ParseHtmlToCodeDescriptors(messageType, html);
        }

        [TestCase("545", "MT545-22-Indicator.html", "Swift-Codes-MT545-22-Indicator.sql")]
        [TestCase("545", "MT545-22F-LinkageTypeIndicator.html", "Swift-Codes-MT545-22F-LinkageTypeIndicator.sql")]
        [TestCase("545", "MT545-94B-Place.html", "Swift-Codes-MT545-94B-Place.sql")]
        [TestCase("545", "MT545-98-DateTime.html", "Swift-Codes-MT545-98-DateTime.sql")]
        [TestCase("545", "MT545-90-DealPrice.html", "Swift-Codes-MT545-90-DealPrice.sql")]
        [TestCase("548", "MT548-22-Indicator.html", "Swift-Codes-MT548-22-Indicator.sql")]
        [TestCase("548", "MT548-25D-Status.html", "Swift-Codes-MT548-25D-Status.sql")]
        [TestCase("548", "MT548-24B-Reason.html", "Swift-Codes-MT548-24B-Reason.sql")]
        [TestCase("548", "MT548-94-Place.html", "Swift-Codes-MT548-94-Place.sql")]
        [TestCase("548", "MT548-95-Party.html", "Swift-Codes-MT548-95-Party.sql")]
        public void ParseAndWriteFile(string messageType, string htmlFile, string sqlFile)
        {
            var htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilesFolder, htmlFile);
            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilesFolder, sqlFile);

            string html;
            using (var reader = new StreamReader(htmlPath, Encoding.UTF8))
            {
                html = reader.ReadToEnd();
            }

            var parser = new Parser();          
            var writer = new DataWriter();
            writer.WriteSqlToFile(parser.ParseHtmlToCodeDescriptors(html, messageType), sqlPath);

        }
    }
}
