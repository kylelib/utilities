using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.IO;

namespace Common.Tools.Xml
{
    public static class XSLHelper
    {

        public static XmlDocument ConvertToXmlDocument(string XmlString, XslCompiledTransform XslTransformer)
        {
            XmlDocument objXmlDocumentToConvert = new XmlDocument();
            objXmlDocumentToConvert.LoadXml(XmlString);

            XmlDocument objConvertedXmlDocument = ConvertToXmlDocument(objXmlDocumentToConvert, XslTransformer);

            return objConvertedXmlDocument;
        }

        public static XmlDocument ConvertToXmlDocument(XmlDocument XmlDocument, XslCompiledTransform XslTransformer)
        {
            string strConvertedXml = ConvertToXmlString(XmlDocument, XslTransformer);

            XmlDocument objConvertedXmlDocument = new XmlDocument();
            objConvertedXmlDocument.LoadXml(strConvertedXml);

            return objConvertedXmlDocument;
        }

        public static string ConvertToXmlString(string XmlString, XslCompiledTransform XslTransformer)
        {
            XmlDocument objXmlDocumentToConvert = new XmlDocument();
            objXmlDocumentToConvert.LoadXml(XmlString);

            string strConvertedXml = ConvertToXmlString(objXmlDocumentToConvert, XslTransformer);

            return strConvertedXml;
        }

        public static string ConvertToXmlString(XmlDocument XmlDocument, XslCompiledTransform XslTransformer)
        {
            string strConvertedXml = string.Empty;

            StringWriter objStringWriter = new StringWriter();
            XmlNodeReader objXmlReader = new XmlNodeReader(XmlDocument);

            try
            {
                XslTransformer.Transform(objXmlReader, null, objStringWriter);
                strConvertedXml = objStringWriter.ToString();
            }
            finally
            {
                objXmlReader.Close();
                objStringWriter.Close();
            }

            return strConvertedXml;
        }

    }
}
