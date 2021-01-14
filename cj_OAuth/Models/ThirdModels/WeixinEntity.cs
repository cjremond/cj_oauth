using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class WeixinEntity
    {
    }
    public class WxParamEntity
    {
        public string appid { get; set; }
        public string appsecret { get; set; }
        public string redirect_uri { get; set; }
        public string response_type { get; set; } = "code";
        public string scope { get; set; } = "snsapi_login";
        public string state { get; set; }
    }
    public class WxAccessTokenEntity
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 当且仅当该网站应用已获得该用户的userinfo授权时，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }
    }

    public class WxUserinfoEntity
    {
        /// <summary>
        /// 普通用户的标识，对当前开发者帐号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 普通用户昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息，json数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public List<string> privilege { get; set; }
        /// <summary>
        /// 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        public string unionid { get; set; }
    }
}
