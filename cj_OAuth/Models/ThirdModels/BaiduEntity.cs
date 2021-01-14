using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class BaiduEntity
    {
    }

    public class BaiduParamEntity
    {
        /// <summary>
        /// 注册应用时获得的API Key。
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 注册应用时获得的API Secret。
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// 回调页面，是 经过转义 的url链接（url 必须以 http 或者 https 开头），比如： http%3A%2F%2Fexample.com   
        /// 在请求之前，开发者需要先到开发者中心对应应用内，配置授权回调地址。
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 必须参数，此值固定为“code”
        /// </summary>
        public string response_type { get; set; } = "code";
        /// <summary>
        /// 非必须参数，以空格分隔的权限列表，若不传递此参数，代表请求用户的默认权限。
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 非必须参数，用于保持请求和回调的状态，授权服务器在回调时
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 非必须参数，登录和授权页面的展现样式，默认为“page”, 具体参数定义请参考 http://developer.baidu.com/wiki/index.php?title=docs/oauth/set
        /// </summary>
        public string display { get; set; }
        /// <summary>
        /// 非必须参数，如传递“force_login=1”，则加载登录页时强制用户输入用户名和口令，不会从cookie中读取百度用户的登陆状态。
        /// </summary>
        public int force_login { get; set; } = 1;
        /// <summary>
        /// 隐藏属性，1则是二维码扫码登录的
        /// </summary>
        public int qrcode { get; set; } = 1;
    }

    public class BaiduAccessTokenParamEntity
    {
        /// <summary>
        /// 应用的API Key；
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 必须参数，'应用的Secret Key；
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// 必须参数，此值固定为“authorization_code”；
        /// </summary>
        public string grant_type { get; set; } = "authorization_code";
        /// <summary>
        /// 必须参数，通过上面第一步所获得的Authorization Code；
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 该值必须与获取Authorization Code时传递的“redirect_uri”保持一致。
        /// </summary>
        public string redirect_uri { get; set; }
    }

    public class BaiduAccessTokenEntity
    {
        /// <summary>
        /// 要获取的Access Token；
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// Access Token的有效期，以秒为单位；
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// 用于刷新Access Token 的 Refresh Token,所有应用都会返回该参数；（10年的有效期）
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// Access Token最终的访问范围，即用户实际授予的权限列表（用户在授权页面时，有可能会取消掉某些请求的权限）
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 基于http调用Open API时所需要的Session Key，其有效期与Access Token一致；
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 基于http调用Open API时计算参数签名用的签名密钥。
        /// </summary>
        public string session_secret { get; set; }
    }

    public class BaiduUserinfoEntity
    {
        public string openid { get; set; }
        /// <summary>
        /// uint
        /// 当前登录用户的数字ID
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// string
        /// 当前登录用户的用户名，值可能为空。
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户真实姓名，可能为空
        /// </summary>
        public string realname { get; set; }
        /// <summary>
        /// 当前登录用户的头像
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// 自我简介，可能为空。
        /// </summary>
        public string userdetail { get; set; }
        /// <summary>
        /// 生日，以yyyy-mm-dd格式显示。
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string marriage { get; set; }
        /// <summary>
        /// 性别。"1"表示男，"0"表示女。
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 血型
        /// </summary>
        public string blood { get; set; }
        /// <summary>
        /// 体型
        /// </summary>
        public string figure { get; set; }
        /// <summary>
        /// 星座
        /// </summary>
        public string constellation { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string education { get; set; }
        /// <summary>
        /// 当前职业
        /// </summary>
        public string trade { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string job { get; set; }
    }
}
