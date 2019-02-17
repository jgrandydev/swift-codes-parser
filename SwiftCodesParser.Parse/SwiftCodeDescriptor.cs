using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCodesParser.Parse
{
    public class SwiftCodeDescriptor
    {
        public string MessageType { get; set; }
        public bool IsQualifier { get; set; }
        public string Qualifier { get; set; }
        public string Code { get; set; }
        public string Definition { get; set; }
        public string Description { get; set; }
    }
}
