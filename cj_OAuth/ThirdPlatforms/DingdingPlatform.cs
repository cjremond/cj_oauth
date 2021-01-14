using cj_OAuth.Base;
using cj_OAuth.Models;
using cj_OAuth.Utils;
using Newtonsoft.Json;
using System;
using System.Web;

namespace cj_OAuth.ThirdPlatforms
{
    /// <summary>
    /// 第三方登录：钉钉扫码登录
    /// https://ding-doc.dingtalk.com/document/app/orgapp-development-process
    /// </summary>
    public class DingdingPlatform : BasePlatform
    {
        private string Authorize_Url = "https://oapi.dingtalk.com/connect/qrconnect";
        private string AccessToken_Url = "https://oapi.dingtalk.com/sns/gettoken";
        private string PersistentCode_Url = "https://oapi.dingtalk.com/sns/get_persistent_code";

        private string Corp_AccessToken_Url = "https://oapi.dingtalk.com/gettoken";
        private string Userid_Url = "https://oapi.dingtalk.com/topapi/user/getbyunionid";
        private string UserInfo_Url = "https://oapi.dingtalk.com/topapi/v2/user/get";

        private DingdingPersistentCodeEntity _persistentCode;  //获取用户的unionid 和openid
        private DingdingParamEntity param = new DingdingParamEntity();
        public DingdingPlatform(ConfigEntity config) : base(config)
        {
            param.appid = config.app_id;
            param.appsecret = config.app_secret;
            param.redirect_uri = config.callback;
            param.scope = config.scope;
            param.state = config.state;
            param.DingCorpAppKey = config.dingcorp_appkey;
            param.DingCorpAppSecret = config.dingcorp_appsecret;
        }


        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}";
        }


        public override string GetOpenId()
        {
            var accessToken = GetAccessToken();
            var persistentCode = GetPersistentCode(accessToken.access_token);
            return persistentCode.openid;
        }


        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取钉钉用户信息失败");
            }

            DingdingUserInfoEntity dingding_Userinfo = JsonConvert.DeserializeObject<DingdingUserInfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                UnionId = dingding_Userinfo.result.unionid,
                Channel = "dingding",
                NickName = dingding_Userinfo.result.name,
                Gender = "n", //钉钉中没有性别
                Avatar = dingding_Userinfo.result.avatar
            };
        }


        public override string GetRawUserInfo()
        {            
            string openid = GetOpenId();
            var userinfo = GetUserInfoByCorp();
            return JsonConvert.SerializeObject(userinfo);
        }

        private DingdingAccessTokenEntity GetAccessToken()
        {
            var url = $"{AccessToken_Url}?appid={param.appid}&appsecret={param.appsecret}";
            var result = HttpUtil.Get(url);
            DingdingAccessTokenEntity dingding_AccessToken = JsonConvert.DeserializeObject<DingdingAccessTokenEntity>(result);
            if (dingding_AccessToken == null || dingding_AccessToken.errcode != "0")
            {
                throw new Exception("获取钉钉 ACCESS_TOKEN 出错");
            }
            return dingding_AccessToken;
        }

        private DingdingPersistentCodeEntity GetPersistentCode(string accessToken)
        {
            var url = $"{PersistentCode_Url}?access_token={accessToken}";
            string data = "{\"tmp_auth_code\": \"" + HttpContext.Current.Request.QueryString["code"] + "\"}";
            var result = HttpUtil.Post(data, url, contentType: "application/json");
            DingdingPersistentCodeEntity dingdingPersistentCodeEntity = JsonConvert.DeserializeObject<DingdingPersistentCodeEntity>(result);
            if (dingdingPersistentCodeEntity == null || dingdingPersistentCodeEntity.errcode != "0")
            {
                throw new Exception("获取永久授权码 出错");
            }
            _persistentCode = dingdingPersistentCodeEntity;
            return dingdingPersistentCodeEntity;
        }


        /// <summary>
        /// 如果需要用户的详细信息，则需要建立一个企业内部应用，并将AppKey和AppSecret传递到Config参数中DingCorpAppKey和DingCorpAppSecret
        /// 如果扫描的用户不是企业的员工，则能拿到unionID，但是无法获取userid
        /// </summary>
        /// <returns></returns>
        private DingdingUserInfoEntity GetUserInfoByCorp()
        {
            var url = $"{Corp_AccessToken_Url}?appkey={param.DingCorpAppKey}&appsecret={param.DingCorpAppSecret}";
            var result = HttpUtil.Get(url);
            DingdingAccessTokenEntity corp_accToken = JsonConvert.DeserializeObject<DingdingAccessTokenEntity>(result);
            if (corp_accToken == null || corp_accToken.errcode != "0")
            {
                throw new Exception("获取企业应用Token 出错");
            }

            url = $"{Userid_Url}?access_token={corp_accToken.access_token}";
            var data = "{\"unionid\": \"" + _persistentCode.unionid + "\"}";
            result = HttpUtil.Post(data, url, contentType: "application/json");
            DingdingUseridEntity dingdingPersistentCodeEntity = JsonConvert.DeserializeObject<DingdingUseridEntity>(result);
            if (dingdingPersistentCodeEntity == null || dingdingPersistentCodeEntity.errcode != "0")
            {
                throw new Exception("获取不到用户具体userid信息");
            }

            url = $"{UserInfo_Url}?access_token={corp_accToken.access_token}";
            data = "{\"userid\": \"" + dingdingPersistentCodeEntity.result.userid + "\"}";
            result = HttpUtil.Post(data, url, contentType: "application/json");

            DingdingUserInfoEntity dingding_Userinfo = JsonConvert.DeserializeObject<DingdingUserInfoEntity>(result);
            if (dingding_Userinfo == null)
            {
                throw new Exception("获取不到用户信息获取失败");
            }
            return dingding_Userinfo;
        }

    }
}
