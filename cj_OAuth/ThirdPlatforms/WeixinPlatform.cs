using cj_OAuth.Base;
using cj_OAuth.Models;
using cj_OAuth.Utils;
using System;
using Newtonsoft.Json;
using System.Web;

namespace cj_OAuth.ThirdPlatforms
{
    /// <summary>
    /// 第三方登录：微信扫码登录（OAuth2.0）
    /// 参考网址： https://developers.weixin.qq.com/doc/oplatform/Website_App/WeChat_Login/Wechat_Login.html
    /// </summary>
    public class WeixinPlatform : BasePlatform
    {
        private string Authorize_Url = "https://open.weixin.qq.com/connect/qrconnect";
        private string AccessToken_Url = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private string UserInfo_Url = "https://api.weixin.qq.com/sns/userinfo";

        private WxParamEntity param = new WxParamEntity();

        public WeixinPlatform(ConfigEntity config) : base(config)
        {
            param.appid = config.app_id;
            param.appsecret = config.app_secret;
            param.redirect_uri = config.callback;
            param.state = config.state;
        }

        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}#wechat_redirect";
        }

        public override string GetOpenId()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null)
            {
                throw new Exception("没有获取到微信用户ID！");
            }
            return accessToken.openid;
        }

        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取微信用户信息");
            }

            WxUserinfoEntity wx_Userinfo = JsonConvert.DeserializeObject<WxUserinfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                OpenId = wx_Userinfo.openid,
                UnionId = wx_Userinfo.unionid,
                Channel = "weixin",
                NickName = wx_Userinfo.nickname,
                Gender = GetGender(wx_Userinfo.sex),
                Avatar = wx_Userinfo.headimgurl
            };
        }

        public override string GetRawUserInfo()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null)
            {
                throw new Exception("获取微信 ACCESS_TOKEN 出错");
            }

            var url = $"{UserInfo_Url}?access_token={accessToken.access_token}&openid={accessToken.openid}";
            var result = HttpUtil.Get(url);

            WxUserinfoEntity wx_Userinfo = JsonConvert.DeserializeObject<WxUserinfoEntity>(result);
            if (wx_Userinfo == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(wx_Userinfo);
            }
        }

        private WxAccessTokenEntity GetAccessToken()
        {
            var code = HttpContext.Current.Request.QueryString["code"];
            var url = $"{AccessToken_Url}?appid={param.appid}&secret={param.appsecret}&code={code}&grant_type=authorization_code";
            var result = HttpUtil.Get(url);
            WxAccessTokenEntity wx_AccessToken = JsonConvert.DeserializeObject<WxAccessTokenEntity>(result);
            return wx_AccessToken;
        }

        /// <summary>
        /// 设置性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        private string GetGender(int sex)
        {
            string result;
            switch (sex)
            {
                case 1:
                    result = "m";
                    break;
                case 2:
                    result = "f";
                    break;
                default:
                    result = "n";
                    break;
            }
            return result;
        }
    }
}
