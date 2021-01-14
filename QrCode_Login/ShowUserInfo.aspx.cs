using cj_OAuth;
using cj_OAuth.Base;
using cj_OAuth.ThirdPlatforms;
using cj_OAuth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrCode_Login
{
    public partial class ShowUserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigEntity config = new ConfigEntity()
                {
                    app_id = "dingoatb*********",
                    app_secret = "sM6Jcqes***************************",
                    callback = "http://cjremond.goho.co/ShowUserInfo.aspx",
                    scope = "snsapi_login",
                    state = "Dingding_login",
                    dingcorp_appkey = "dingxlk******************",
                    dingcorp_appsecret = "zYz8oMOhQ3vm89***************************",
                };

                BasePlatform platform = new DingdingPlatform(config);

                //支付宝返回的是auth_code、其他的都是code
                var code = HttpContext.Current.Request.QueryString["code"];
                if (string.IsNullOrEmpty(code))
                {
                    var redirectUrl = platform.GetRedirectUrl();
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    FormatedUserInfo userInfo = platform.GetUserInfo();
                    ltUserInfo.Text = userInfo.JSONify();
                }
            }
        }
    }
}