using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WarZoneGuns
{
    public static class GunsClient
    {
        static string url = @"http://localhost:60065/Guns";

        public static Gun Post(Gun gun)
        {
            return ExecuteRequest<Gun, Gun>("POST", gun);
        }

        public static bool Put(Gun gun)
        {
            return ExecuteRequest<bool, Gun>("PUT", gun);
        }

        public static bool Delete(Gun gun)
        {
            return ExecuteRequest<bool, Gun>("DELETE", gun);
        }

        public static IEnumerable<Gun> GetAllGuns()
        {
            return ExecuteRequest<List<Gun>, string>("GET", null);
        }

        static T ExecuteRequest<T, V>(string type, V v)
        {
            var request = WebRequest.Create(url);
            DataContractJsonSerializer serializerT = new DataContractJsonSerializer(typeof(T));
            request.Method = type;
            request.ContentType = "application/json";
            if (v != null)
            {
                DataContractJsonSerializer serializerV = new DataContractJsonSerializer(typeof(V));
                using (var stream = request.GetRequestStream())
                    serializerV.WriteObject(stream, v);
            }
            T result = (T)Activator.CreateInstance(typeof(T));
            try
            {
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                    result = (T)serializerT.ReadObject(stream);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            return result;
        }
    }
}
