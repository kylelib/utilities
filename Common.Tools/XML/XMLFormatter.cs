using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Common.Tools.Xml
{
    public class XmlFormatter
    {

        public static string GetEmptyString()
        {
            string strString = string.Empty;

            return strString;
        }


        public static string FormatXmlString(string strDefaultXml)
        {
            XmlDocument objXml = new XmlDocument();
            XmlTextWriter objTextWriter;
            StringWriter objStringWriter;

            if (!string.IsNullOrEmpty(strDefaultXml))
            {
                objXml.LoadXml(strDefaultXml);

                objStringWriter = new StringWriter();
                objTextWriter = new XmlTextWriter(objStringWriter);
                objTextWriter.Formatting = Formatting.Indented;
                objTextWriter.Indentation = 5;

                objXml.WriteTo(objTextWriter);

                return objStringWriter.ToString();
            }

            return string.Empty;

        }
    }
}
