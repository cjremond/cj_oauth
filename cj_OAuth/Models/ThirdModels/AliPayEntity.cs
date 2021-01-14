using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class AliPayEntity
    {
    }

    public class AlipayParamEntity
    {
        /// <summary>
        /// 开发者应用的 APPID；
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// 接口权限值，获取用户信息场景暂支持 auth_user 和 auth_base 两个值
        /// </summary>
        public string scope { get; set; } = "auth_user";
        /// <summary>
        /// 回调页面，是 经过转义 的url链接（url 必须以 http 或者 https 开头），比如： http%3A%2F%2Fexample.com   
        /// 在请求之前，开发者需要先到开发者中心对应应用内，配置授权回调地址。
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 商户自定义参数，用户授权后，重定向到 redirect_uri 时会原样回传给商户。 
        /// 为防止 CSRF 攻击，建议开发者请求授权时传入 state 参数，该参数要做到既不可预测，又可以证明客户端和当前第三方网站的登录认证状态存在关联，并且不能有中文。
        /// </summary>
        public string state { get; set; } = "init";
    }

    public class AlipayAccessTokenParamEntity
    {
        /// <summary>
        /// 发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string method { get; set; } = "alipay.system.oauth.token";
        /// <summary>
        /// 支付宝分配给开发者的应用ID
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>
        public string sign_type { get; set; } = "RSA2";
        /// <summary>
        /// 商户请求参数的签名串，详见https://alipay.open.taobao.com/docs/doc.htm?treeId=291&articleId=105974&docType=1
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 调用的接口版本，固定为：1.0
        /// </summary>
        public string version { get; set; } = "1.0";
        /// <summary>
        /// 请求使用的编码格式，如utf-8,gbk,gb2312等
        /// </summary>
        public string charset { get; set; } = "utf-8";
        /// <summary>
        /// 授权方式。支持：
        /// 1.authorization_code，表示换取使用用户授权码code换取授权令牌access_token。
        /// 2.refresh_token，表示使用refresh_token刷新获取新授权令牌。
        /// </summary>
        public string grant_type { get; set; } = "authorization_code";
        /// <summary>
        /// 授权码，用户对应用授权后得到。本参数在 grant_type 为 authorization_code 时必填；为 refresh_token 时不填。
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 刷新令牌，上次换取访问令牌时得到。
        /// 本参数在 grant_type 为 authorization_code 时不填；
        /// 为 refresh_token 时必填，且该值来源于此接口的返回值 app_refresh_token（即至少需要通过 grant_type=authorization_code 调用此接口一次才能获取）。
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 仅支持JSON
        /// </summary>
        public string format { get; set; } = "JSON";
        /// <summary>
        /// 针对用户授权接口，获取用户相关数据时，用于标识用户授权关系。详见用户信息授权
        /// </summary>
        public string auth_token { get; set; }
    }

    public class AlipayAccessTokenEntity
    {
        public TokenResponse alipay_system_oauth_token_response { get; set; }
        public string sign { get; set; }
    }

    public class TokenResponse
    {
        /// <summary>
        /// 支付宝用户的唯一userId
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 访问令牌。通过该令牌调用需要授权类接口
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 访问令牌的有效时间，单位是秒。
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// 刷新令牌。通过该令牌可以刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 刷新令牌的有效时间，单位是秒。
        /// </summary>
        public string re_expires_in { get; set; }
        /// <summary>
        /// 网关返回码,详见文档
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 业务错误码
        /// </summary>
        public string sub_code { get; set; }
    }

    public class AlipayUserinfoEntity
    {
        public UserinfoResponse alipay_user_info_share_response { get; set; }
        public string sign { get; set; }
    }

    public class UserinfoResponse
    {
        /// <summary>
        /// 支付宝用户的userId
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 市名称。
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户昵称。
        /// </summary>
        public string nick_name { get; set; }
        /// <summary>
        /// 只有is_certified为T的时候才有意义，否则不保证准确性.
        /// 性别（F：女性；M：男性）。
        /// </summary>
        public string gender { get; set; }
    }
}
