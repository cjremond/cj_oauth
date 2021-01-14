using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth
{
    public class ConfigEntity
    {
        public string app_id { get; set; } = "";
        public string app_secret { get; set; } = "";
        public string callback { get; set; } = "";
        public string response_type { get; set; } = "code";
        public string grant_type { get; set; } = "authorization_code";
        public string proxy { get; set; } = "";
        public string state { get; set; } = "";
        public string scope { get; set; } = "";
        public string display { get; set; } = "default";
        /// <summary>
        /// 是否获取unionid。QQ获取用户信息的时候专用
        /// </summary>
        public bool withUnionid { get; set; } = false;
        /// <summary>
        /// 钉钉应用APPkey
        /// </summary>
        public string dingcorp_appkey { get; set; }
        /// <summary>
        /// 钉钉应用AppSecret
        /// </summary>
        public string dingcorp_appsecret { get; set; }
    }
}
