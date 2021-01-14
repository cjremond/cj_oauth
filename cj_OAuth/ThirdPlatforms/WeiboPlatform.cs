using cj_OAuth.Base;
using cj_OAuth.Models;
using cj_OAuth.Utils;
using Newtonsoft.Json;
using System;

namespace cj_OAuth.ThirdPlatforms
{
    /// <summary>
    /// 第三方登录：微博扫码登录
    /// 参考网址：https://open.weibo.com/wiki/%E5%BE%AE%E5%8D%9AAPI
    /// </summary>
    public class WeiboPlatform : BasePlatform
    {
        private string Authorize_Url = "https://api.weibo.com/oauth2/authorize";
        private string AccessToken_Url = "https://api.weibo.com/oauth2/access_token";
        private string UserInfo_Url = "https://api.weibo.com/2/users/show.json";

        private string _unionid;
        private WbParamEntity param = new WbParamEntity();

        public WeiboPlatform(ConfigEntity config) : base(config)
        {
            param.client_id = config.app_id;
            param.client_secret = config.app_secret;
            param.redirect_uri = config.callback;
            param.scope = config.scope;
            param.state = config.state;
            param.display = config.display;
        }
        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}&response_type=code";
        }

        public override string GetOpenId()
        {
            var accessToken = GetAccessToken();

            if (accessToken == null)
            {
                throw new Exception("没有获取到微博用户ID！");
            }
            return accessToken.uid;
        }

        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取微博用户信息失败");
            }

            WbUserinfoEntity wb_Userinfo = JsonConvert.DeserializeObject<WbUserinfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                UnionId = _unionid,
                Channel = "weibo",
                NickName = wb_Userinfo.screen_name,
                Gender = wb_Userinfo.gender,
                Avatar = wb_Userinfo.avatar_hd
            };
        }

        public override string GetRawUserInfo()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null)
            {
                throw new Exception("获取微博 ACCESS_TOKEN 出错");
            }

            var url = $"{UserInfo_Url}?access_token={accessToken.access_token}&uid={accessToken.uid}";
            var result = HttpUtil.Get(url);

            WbUserinfoEntity wb_Userinfo = JsonConvert.DeserializeObject<WbUserinfoEntity>(result);
            if (wb_Userinfo == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(wb_Userinfo);
            }
        }

        private WbAccessTokenEntity GetAccessToken()
        {
            var code = System.Web.HttpContext.Current.Request.QueryString["code"];
            var url = $"{AccessToken_Url}?client_id={param.client_id}&client_secret={param.client_secret}&code={code}&redirect_uri={param.redirect_uri}&grant_type=authorization_code";
            var data = "";
            var result = HttpUtil.Post(data, url);
            WbAccessTokenEntity wb_AccessToken = JsonConvert.DeserializeObject<WbAccessTokenEntity>(result);
            _unionid = wb_AccessToken?.uid;
            return wb_AccessToken;
        }
    }
}
