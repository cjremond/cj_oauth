using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace cj_OAuth.Utils
{
    /// <summary>
    /// 参数组合
    /// </summary>
    public class ParamUtil
    {
        /// <summary>
        /// 组合参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="urlencode">是否编码</param>
        /// <param name="exceptParam">过滤参数</param>
        /// <returns></returns>
        public static string BuildParams<T>(T param, bool urlencode = false, string exceptParam = "sign")
        {
            return GetParams(param, false, urlencode, exceptParam);
        }

        /// <summary>
        /// 默认排序的组合参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string BuildSortedParams<T>(T param, bool urlencode = false, string exceptParam = "sign")
        {
            return GetParams(param, true, urlencode, exceptParam);
        }

        /// <summary>
        /// 组合参数成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="paramsSort"></param>
        /// <param name="urlencode"></param>
        /// <param name="exceptParam"></param>
        /// <returns></returns>
        private static string GetParams<T>(T param, bool paramsSort, bool urlencode, string exceptParam)
        {
            List<string> result = new List<string>();
            Type t = param.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                if (!string.IsNullOrWhiteSpace(exceptParam) && name.Equals(exceptParam))
                {
                    continue;
                }
                object value = item.GetValue(param, null);
                if (!string.IsNullOrEmpty(name) && value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    result.Add($"{name}={(urlencode == true ? HttpUtility.UrlEncode(value.ToString()) : value)}");
                }
            }
            if (paramsSort == true)
            {
                result.Sort();
            }
            return string.Join("&", result);
        }
    }
}
