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

        [TestCase("MT548-22-Indicator.html")]
        [TestCase("MT548-25D-Status.html")]
        [TestCase("MT548-24B-Reason.html")]
        public void ParseHtml(string file)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilesFolder, file);

            string html;
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                html = reader.ReadToEnd();
            }

            var parser = new Parser();
            var codes = parser.ParseHtmlToCodeDescriptors(html);
        }

        [TestCase("MT548-22-Indicator.html", "Swift-Codes-MT548-22-Indicator.sql")]
        [TestCase("MT548-25D-Status.html", "Swift-Codes-MT548-25D-Status.sql")]
        [TestCase("MT548-24B-Reason.html", "Swift-Codes-MT548-24B-Reason.sql")]
        public void ParseAndWriteFile(string htmlFile, string sqlFile)
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
            writer.WriteSqlToFile(parser.ParseHtmlToCodeDescriptors(html), sqlPath);

        }
    }
}
