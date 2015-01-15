using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Common.Tools.Json
{
    public static class JsonHelper
    {

        public static string SerializeObject<T>(T inputObject)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate
            };

            string jsonObject = JsonConvert.SerializeObject(inputObject, jsonSerializerSettings);

            return jsonObject;
        }

        public static T DeserializeObject<T>(string jsonObject)
        {
            T netObject = JsonConvert.DeserializeObject<T>(jsonObject);

            return netObject;
        }

    }

}
