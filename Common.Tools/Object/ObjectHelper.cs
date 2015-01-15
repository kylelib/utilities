using System;
using System.Collections.Generic;
using System.Text;
using Common.Tools.Xml;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Common.Tools.Object
{
    public static class ObjectHelper
    {

        public static To CopyObjectToSameObject<To>(To BaseObject)
        {
            string strSerializedObjectToCopy = XmlHelper.SerializeObject<To>(BaseObject);

            To objNewObject = XmlHelper.DeserializeObject<To>(strSerializedObjectToCopy);

            return objNewObject;
        }

        public static To CopyObjectToDifferentObject<From, To>(From BaseObject) where To : new()
        {
            //Define new object and serialize so we can get a "default" Xml
            To objNewObject = new To();
            string strNewObjectXml = XmlHelper.SerializeObject<To>(objNewObject);
            XmlDocument xdNewObject = new XmlDocument();
            xdNewObject.LoadXml(strNewObjectXml);

            //Serialize Object to Copy (so we can move the inner Xml into the new object)
            string strBaseObjectXml = XmlHelper.SerializeObject<From>(BaseObject);
            XmlDocument xdBaseObject = new XmlDocument();
            xdBaseObject.LoadXml(strBaseObjectXml);

            //Now copy insides of Xml to new object Xml
            if (xdBaseObject.FirstChild != null && xdNewObject.FirstChild != null)
            {
                xdNewObject.FirstChild.InnerXml = xdBaseObject.FirstChild.InnerXml;
                objNewObject = XmlHelper.DeserializeObject<To>(xdNewObject.OuterXml);
            }

            return objNewObject;
        }

        public static string ConvertToJsonString<T>(T content)
        {
            string strJsonSerialized = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, content);

                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                strJsonSerialized = sr.ReadToEnd();
            }

            return strJsonSerialized;
        }
    }
}
