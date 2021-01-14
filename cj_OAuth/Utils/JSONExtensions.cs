using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;
using System.Web;

namespace cj_OAuth.Utils
{
    public static class JSONExtensions
    {
        private static readonly JsonSerializerSettings CamelCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string JSONify(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 小写JSON序列化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>JSON</returns>
        public static string JSONifyCamelCase(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, CamelCaseSettings);
        }

    }
}
