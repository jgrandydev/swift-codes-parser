using System;
using System.IO;
using System.Text;

using NUnit.Framework;
using SwiftCodesParser.Parse;

namespace SwiftCodesParser.Tests
{
    [TestFixture]
    public class ParserUnitTests
    {
        private const string TestFilesFolder = "test-files";

        [TestCase("MT548-22-Indicator.html")]
        public void TestParseFile(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestFilesFolder, fileName);

            string html;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                html = reader.ReadToEnd();
            }

            var parser = new Parser();
            var codes = parser.ParseHtmlToCodeDescriptors(html);
        }
    }
}
