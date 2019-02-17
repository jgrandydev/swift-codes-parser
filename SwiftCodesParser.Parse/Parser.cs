
using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace SwiftCodesParser.Parse
{
    public class Parser
    {

        public IEnumerable<SwiftCodeDescriptor> ParseHtmlToCodeDescriptors(string html, string messageType)
        {
            IEnumerable<SwiftCodeDescriptor> codes = null;

            var parser = new HtmlParser();
            var doc = parser.ParseDocument(html);
            var divs = doc.All.Where(m => m.LocalName == "div");
            string qualifier = string.Empty;
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
                        codes = ExtractCodes(doc, messageType, null);
                        var list = codes.ToList();
                        if (list.Count > 0 && list[0].IsQualifier)
                        {
                            qualifier = list[0].Code;
                        }
                        break;

                    case "fldctu":
                        // get qualifier
                        doc = parser.ParseDocument(div.InnerHtml);
                        var p = doc.All.FirstOrDefault(m => m.LocalName == "p");
                        var i = p.InnerHtml.ToLower().IndexOf("if qualifier is ");
                        if (i >= 0)
                        {
                            qualifier = p.InnerHtml.Substring(i + 16, 4);
                        }
                        codes = codes.Concat(ExtractCodes( doc, messageType, qualifier)).ToList();
                        break;
                }
            }

            return codes;
        }

        public IEnumerable<SwiftCodeDescriptor> ExtractCodes(AngleSharp.Html.Dom.IHtmlDocument doc, string messageType, string qualifier)
        {
            var trs = doc.All.Where(m => m.LocalName == "tr" && m.HasAttribute("valign") && m.HasChildNodes && m.ChildElementCount == 3);

            foreach (var tr in trs)
            {
                yield return new SwiftCodeDescriptor
                {
                    MessageType =  messageType,
                    Code = tr.Children[0].TextContent.Trim(),
                    Definition = tr.Children[1].TextContent.Trim(),
                    Description = tr.Children[2].TextContent.Trim(),
                    IsQualifier = qualifier == null,
                    Qualifier = qualifier
                };
            }
        }

    }
}
