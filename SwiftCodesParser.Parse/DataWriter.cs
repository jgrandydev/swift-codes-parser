using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftCodesParser.Parse;

namespace SwiftCodesParser.Persist
{
    public class DataWriter
    {
        public void WriteSqlToFile(IEnumerable<SwiftCodeDescriptor> codes, string path)
        {
            WriteToFile(CreateSql(codes), path);
        }

        public string CreateSql(IEnumerable<SwiftCodeDescriptor> codes)
        {
            const string TableName = "T_SWIFT_CODES_DEFINITIONS";

            string sql = null;

            foreach(var code in codes)
            {
                if (code.IsQualifier)
                {
                    sql += $"INSERT INTO {TableName} ";
                    sql += $"(MESSAGE_TYPE,QUALIFIER,QUALIFIER_DEFINITION,QUALIFIER_DESCRIPTION) ";
                    sql += $"VALUES ({Quoted(code.MessageType)},{Quoted(code.Code)},{Quoted(code.Definition)},{Quoted(code.Description)})";
                    sql += Environment.NewLine;
                }
                else
                {
                    sql += $"INSERT INTO {TableName} ";
                    sql += $"(MESSAGE_TYPE,QUALIFIER,CODE,CODE_DEFINITION,CODE_DESCRIPTION) ";
                    sql += $"VALUES ({Quoted(code.MessageType)},{Quoted(code.Qualifier)},{Quoted(code.Code)},{Quoted(code.Definition)},{Quoted(code.Description)})";
                    sql += Environment.NewLine;
                }
            }

            return sql;
        }

        public void WriteToFile(string contents, string path)
        {
            System.IO.File.WriteAllText(path,contents);
        }

        public string Quoted(string contents)
        {
            return "'" + contents + "'";
        }
    }
}
