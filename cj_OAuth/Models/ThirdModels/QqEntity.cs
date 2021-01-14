using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class QqEntity
    {
    }

    public class QqParamEntity
    {
        /// <summary>
        /// 授权类型，此值固定为“token”
        /// </summary>
        public string response_type { get; set; } = "code";
        /// <summary>
        /// 申请QQ登录成功后，分配给应用的appid
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 申请QQ登录成功后，分配给应用的appsecret
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// 成功授权后的回调地址，必须是注册appid时填写的主域名下的地址，建议设置为网站首页或网站的用户中心。
        /// 注意需要将url进行URLEncode。
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 请求用户授权时向用户显示的可进行授权的列表。
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 仅PC网站接入时使用。
        /// 用于展示的样式。不传则默认展示为为PC下的样式。
        /// 如果传入“mobile”，则展示为mobile端下的样式。
        /// </summary>
        public string display { get; set; }
        /// <summary>
        /// 仅WAP网站接入时使用。
        /// QQ登录页面版本（1：wml版本； 2：xhtml版本），默认值为1。
        /// </summary>
        public string g_ut { get; set; } = "1";

        /// <summary>
        /// 是否获取用户的unionid
        /// </summary>
        public bool withUnionid { get; set; } = false;
    }

    public class qqAccessTokenEntity
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string refresh_token { get; set; }

        public string name { get; set; }
    }

    public class qqOpenIDEntity
    {
        public string client_id { get; set; }
        public string openid { get; set; }
        public string unionid { get; set; }
    }

    public class QqUserinfoEntity
    {
        /// <summary>
        /// 返回码.  0: 正确返回
        /// </summary>
        public int ret { get; set; }
        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 用户在QQ空间的昵称。
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 大小为30×30像素的QQ空间头像URL。
        /// </summary>
        public string figureurl { get; set; }
        /// <summary>
        /// 大小为50×50像素的QQ空间头像URL。
        /// </summary>
        public string figureurl_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ空间头像URL。
        /// </summary>
        public string figureurl_2 { get; set; }
        /// <summary>
        /// 大小为40×40像素的QQ头像URL。
        /// </summary>
        public string figureurl_qq_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ头像URL。
        /// 需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有。
        /// </summary>
        public string figureurl_qq_2 { get; set; }
        /// <summary>
        /// 性别。 如果获取不到则默认返回"男"。
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 标识用户是否为黄钻用户（0：不是；1：是）。
        /// </summary>
        public string is_yellow_vip { get; set; }
        /// <summary>
        /// 标识用户是否为黄钻用户（0：不是；1：是）。
        /// </summary>
        public string vip { get; set; }
        /// <summary>
        /// 黄钻等级。
        /// </summary>
        public string yellow_vip_level { get; set; }
        /// <summary>
        /// 黄钻等级。
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 标识是否为年费黄钻用户（0：不是； 1：是）。
        /// </summary>
        public string is_yellow_year_vip { get; set; }
    }
}
