using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace Common.Tools.Xml
{
    public static class XmlHelper
    {
        private const string _TracerCategory = "Common.Tools.Xml";

        /// <summary>
        /// Serialize a DataContract object into an Xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objToSerialize"></param>
        /// <returns></returns>
        public static string SerializeDataContractObject<T>(T objToSerialize)
        {
            MemoryStream objMemoryStream = null;
            StreamReader sr = null;

            try
            {
                DataContractSerializer objDataContractSerializer = new DataContractSerializer(typeof(T));
                objMemoryStream = new MemoryStream();
                objDataContractSerializer.WriteObject(objMemoryStream, objToSerialize);

                // Need to set the MemoryStream Position to 0 (the start) since the write leaves the position at the end.
                objMemoryStream.Position = 0;

                sr = new StreamReader(objMemoryStream);
                return sr.ReadToEnd();
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }

                if (objMemoryStream != null)
                {
                    objMemoryStream.Close();
                }
            }
        }

        /// <summary>
        /// Serialize an object into an Xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T objToSerialize)
        {
            string XmlString = null;
            MemoryStream objMemoryStream = new MemoryStream();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            StringWriter sw = new StringWriter(); ;

            try
            {
                ns.Add("", "");
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlWriter writer = new XmlTextWriterFormattedNoDeclaration(sw);

                xs.Serialize(writer, objToSerialize, ns);

                XmlString = sw.ToString();
                return XmlString;
            }
            finally
            {
                objMemoryStream.Close();
                sw.Close();
            }
        }

        /// <summary>
        /// Reconstruct an object from an Xml string
        /// </summary>
        /// <param name="Xml"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string Xml)
        {
            object objDeserialized;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream objMemoryStream = new MemoryStream(StringToUTF8ByteArray(Xml));

            try
            {
                XmlTextWriter XmlTextWriter = new XmlTextWriter(objMemoryStream, Encoding.UTF8);
                objDeserialized = (T)xs.Deserialize(objMemoryStream);
                return (T)objDeserialized;
            }
            finally
            {
                objMemoryStream.Close();
            }
        }

        public class XmlTextWriterFormattedNoDeclaration : XmlTextWriter
        {
            public XmlTextWriterFormattedNoDeclaration(TextWriter w)
                : base(w)
            {
                Formatting = System.Xml.Formatting.Indented;
                Indentation = 5;
            }

            public override void WriteStartDocument() { } // suppress
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
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
