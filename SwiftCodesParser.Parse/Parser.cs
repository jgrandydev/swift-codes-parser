
using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace SwiftCodesParser.Parse
{
    public class Parser
    {

        public IEnumerable<SwiftCodeDescriptor> ParseHtmlToCodeDescriptors(string html)
        {
            var codes = new List<SwiftCodeDescriptor>();

            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var doc = parser.ParseDocument(html);

            var divs = doc.All.Where(m => m.LocalName == "div");
            foreach (var div in divs)
            {
                switch (div.ClassName)
                {
                    case "main":
                    case "fldblk":
                    case "fldfmt":
                    case "fldpres":
                    case "fldqual":
                        break;

                    case "flddfn":
                        doc = parser.ParseDocument(div.InnerHtml);
                        codes = codes.Concat(ExtractCodes(doc, null)).ToList();
                        break;

                    case "fldctu":
                        // get qualifier
                        doc = parser.ParseDocument(div.InnerHtml);
                        var p = doc.All.FirstOrDefault(m => m.LocalName == "p");
                        var i = p.InnerHtml.IndexOf("if Qualifier is ");
                        var qualifier = p.InnerHtml.Substring(i + 16, 4);
                        codes = codes.Concat(ExtractCodes(doc, qualifier)).ToList();
                        break;
                }
            }

            return codes;
        }

        public IEnumerable<SwiftCodeDescriptor> ExtractCodes(AngleSharp.Html.Dom.IHtmlDocument doc, string qualifier)
        {
            var trs = doc.All.Where(m => m.LocalName == "tr" && m.HasAttribute("valign") && m.HasChildNodes && m.ChildElementCount == 3);

            foreach (var tr in trs)
            {
                yield return new SwiftCodeDescriptor
                {
                    Code = tr.Children[0].TextContent,
                    Definition = tr.Children[1].TextContent,
                    Description = tr.Children[2].TextContent,
                    IsQualifier = qualifier == null,
                    Qualifier = qualifier
                };
            }
        }

    }
}
