using cj_OAuth.Base;
using cj_OAuth.Models;
using cj_OAuth.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.ThirdPlatforms
{
    /// <summary>
    /// 第三方登录：百度扫码登录
    /// 参考网站： http://developer.baidu.com/wiki/index.php?title=docs/oauth
    /// </summary>
    public class BaiduPlatform : BasePlatform
    {
        private string Authorize_Url = "http://openapi.baidu.com/oauth/2.0/authorize";
        private string AccessToken_Url = "https://openapi.baidu.com/oauth/2.0/token";
        private string UserInfo_Url = "https://openapi.baidu.com/rest/2.0/passport/users/getInfo";

        private BaiduParamEntity param = new BaiduParamEntity();
        public BaiduPlatform(ConfigEntity config) : base(config)
        {
            param.response_type = config.response_type;
            param.client_id = config.app_id;
            param.client_secret = config.app_secret;
            param.redirect_uri = config.callback;
            param.state = config.state;
        }

        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}";
        }

        public override string GetOpenId()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取百度用户信息失败");
            }

            BaiduUserinfoEntity baidu_Userinfo = JsonConvert.DeserializeObject<BaiduUserinfoEntity>(userInfoRawJson);
            return baidu_Userinfo.openid;
        }

        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取百度用户信息失败");
            }

            BaiduUserinfoEntity baidu_Userinfo = JsonConvert.DeserializeObject<BaiduUserinfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                OpenId = baidu_Userinfo.openid,
                Channel = "baidu",
                NickName = string.IsNullOrWhiteSpace(baidu_Userinfo.username) ? baidu_Userinfo.realname : baidu_Userinfo.username,
                Gender = "1".Equals(baidu_Userinfo.sex) ? "m" : ("0".Equals(baidu_Userinfo.sex) ? "f" : "n"),
                Avatar = $"https://himg.bdimg.com/sys/portraitn/item/{baidu_Userinfo.portrait}",
            };
        }

        public override string GetRawUserInfo()
        {
            var accessToken = GetAccessToken();
            var url = $"{UserInfo_Url}?access_token={accessToken.access_token}";
            var result = HttpUtil.Get(url);

            BaiduUserinfoEntity baidu_Userinfo = JsonConvert.DeserializeObject<BaiduUserinfoEntity>(result);
            if (baidu_Userinfo == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(baidu_Userinfo);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private BaiduAccessTokenEntity GetAccessToken()
        {
            BaiduAccessTokenParamEntity baiduAccessToken = new BaiduAccessTokenParamEntity
            {
                grant_type = "authorization_code",
                code = System.Web.HttpContext.Current.Request.QueryString["code"],
                client_id = param.client_id,
                client_secret = param.client_secret,
                redirect_uri = param.redirect_uri,
            };
            var url = $"{AccessToken_Url}?{ParamUtil.BuildSortedParams(baiduAccessToken)}";
            var result = HttpUtil.Get(url);
            BaiduAccessTokenEntity baidu_AccessToken = JsonConvert.DeserializeObject<BaiduAccessTokenEntity>(result);
            if (baidu_AccessToken == null)
            {
                throw new Exception($"获取百度 ACCESS_TOKEN 出错:{result}");
            }
            return baidu_AccessToken;
        }
    }
}
