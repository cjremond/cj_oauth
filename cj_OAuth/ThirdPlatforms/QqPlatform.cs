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
    /// 第三方登录：QQ扫码登录
    /// https://wiki.open.qq.com/wiki/website/%E5%BC%80%E5%8F%91%E6%94%BB%E7%95%A5_Server-side#Step3.EF.BC.9A.E9.80.9A.E8.BF.87Authorization_Code.E8.8E.B7.E5.8F.96Access_Token
    /// </summary>
    public class QqPlatform : BasePlatform
    {
        private string Authorize_Url = "https://graph.qq.com/oauth2.0/authorize";
        private string AccessToken_Url = "https://graph.qq.com/oauth2.0/token";
        private string UserInfo_Url = "https://graph.qq.com/user/get_user_info";
        private string OpenID_Url = "https://graph.qq.com/oauth2.0/me";

        private qqAccessTokenEntity _accessToken;
        private string _unionid;
        private string _openid;

        private QqParamEntity param = new QqParamEntity();

        public QqPlatform(ConfigEntity config) : base(config)
        {
            param.response_type = config.response_type;
            param.client_id = config.app_id;
            param.client_secret = config.app_secret;
            param.redirect_uri = config.callback;
            param.scope = config.scope;
            param.state = config.state;
            param.display = config.display;
            param.withUnionid = config.withUnionid;
        }

        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}";
        }

        public override string GetOpenId()
        {
            _accessToken = GetAccessToken();

            var url = $"{OpenID_Url}?access_token={_accessToken.access_token}";

            // 如果要获取unionid，先去申请https://wiki.connect.qq.com/unionid%E4%BB%8B%E7%BB%8D
            if (param.withUnionid == true)
            {
                url += "&unionid=1";
            }

            var result = HttpUtil.Get(url);
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID","unionid":"YOUR_UNIONID"} );
            result = result.Replace("callback(", "").Replace(")", "").Trim(';');

            var openid = JsonConvert.DeserializeObject<qqOpenIDEntity>(result);
            if (openid == null)
            {
                throw new Exception("获取qq OPENID 出错");
            }
            _unionid = openid.unionid;
            return openid.openid;
        }

        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取qq用户信息失败");
            }

            QqUserinfoEntity qq_Userinfo = JsonConvert.DeserializeObject<QqUserinfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                OpenId = _openid,
                UnionId = _unionid,
                Channel = "qq",
                NickName = qq_Userinfo.nickname,
                Gender = qq_Userinfo.gender == "男" ? "m" : "f",
                Avatar = string.IsNullOrWhiteSpace(qq_Userinfo.figureurl_qq_2) ? qq_Userinfo.figureurl_qq_1 : qq_Userinfo.figureurl_qq_2
            };
        }

        public override string GetRawUserInfo()
        {
            _openid = GetOpenId();

            var url = $"{UserInfo_Url}?access_token={_accessToken?.access_token}&oauth_consumer_key={param.client_id}&openid={_openid}&format=json";
            var result = HttpUtil.Get(url);

            QqUserinfoEntity qq_Userinfo = JsonConvert.DeserializeObject<QqUserinfoEntity>(result);
            if (qq_Userinfo == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(qq_Userinfo);
            }
        }

        private qqAccessTokenEntity GetAccessToken()
        {
            var code = System.Web.HttpContext.Current.Request.QueryString["code"];
            var url = $"{AccessToken_Url}?client_id={param.client_id}&client_secret={param.client_secret}&code={code}&redirect_uri={param.redirect_uri}&grant_type=authorization_code";
            var result = HttpUtil.Get(url);

            var arr = result.Split('&');
            var resultDic = new Dictionary<string, string>();
            foreach (var ar in arr)
            {
                if (ar.Contains("="))
                {
                    resultDic.Add(ar.Split('=')[0], ar.Split('=')[1]);
                }
            }
            qqAccessTokenEntity qq_AccessToken = new qqAccessTokenEntity
            {
                access_token = resultDic.ContainsKey("access_token") ? resultDic["access_token"] : "",
                expires_in = resultDic.ContainsKey("expires_in") ? resultDic["expires_in"] : "",
            };

            if (qq_AccessToken == null)
            {
                throw new Exception("获取qq ACCESS_TOKEN 出错");
            }
            return qq_AccessToken;
        }
    }
}
