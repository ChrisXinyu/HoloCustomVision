using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomVisionClient.Utils
{
    class Utils
    {
        public static string JsonSerializer<T>(T t)
        {
            try
            {
                string JsonStr = JsonConvert.SerializeObject(t);
                return JsonStr;
            }
            catch
            {
                return null;
            }
        }

        public static T JsonDeserialize<T>(string JsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(JsonStr);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
