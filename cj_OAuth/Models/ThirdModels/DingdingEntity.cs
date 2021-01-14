using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class DingdingEntity
    {
    }

    public class DingdingParamEntity
    {
        /// <summary>
        /// 应用的appId
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 应用的appsecret
        /// </summary>
        public string appsecret { get; set; }
        /// <summary>
        /// 接口权限值，获取用户信息场景暂支持 auth_user 和 auth_base 两个值
        /// </summary>
        public string scope { get; set; } = "snsapi_login";
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
        /// <summary>
        /// 固定为code。
        /// </summary>
        public string response_type { get; set; } = "code";
        /// <summary>
        /// 钉钉应用APPkey
        /// </summary>
        public string DingCorpAppKey { get; set; }
        /// <summary>
        /// 钉钉应用AppSecret
        /// </summary>
        public string DingCorpAppSecret { get; set; }
    }

    public class DingdingAccessTokenEntity
    {
        /// <summary>
        /// 生成的access_token。
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token的过期时间，单位秒。
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// 返回码描述
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string errcode { get; set; }
    }

    public class DingdingPersistentCodeEntity
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string persistent_code { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }

    public class DingdingUseridEntity
    {
        /// <summary>
        /// 请求ID。
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// 返回码，0代表成功。
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 调用失败时返回的错误信息。
        /// </summary>
        public string errmsg { get; set; }
        public UserGetByUnionIdResponse result { get; set; }
    }

    public class UserGetByUnionIdResponse
    {
        public int contact_type { get; set; }
        public string userid { get; set; }
    }

    public class DingdingUserInfoEntity
    {
        /// <summary>
        /// 请求ID。
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// 返回码，0代表成功。
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 调用失败时返回的错误信息。
        /// </summary>
        public string errmsg { get; set; }
        public UserGetRequest result { get; set; }
    }

    public class UserGetRequest
    {
        /// <summary>
        /// 员工的userid。
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 员工在当前开发者企业账号范围内的唯一标识。
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 员工名称。
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像。
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 国际电话区号。
        /// </summary>
        public string state_code { get; set; }
        /// <summary>
        /// 手机号码。
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 是否号码隐藏： true：隐藏; false：不隐藏
        /// </summary>
        public bool hide_mobile { get; set; }
        /// <summary>
        /// 分机号。
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 员工工号。
        /// </summary>
        public string job_number { get; set; }
        /// <summary>
        /// 职位。
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 员工邮箱。
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 办公地点。
        /// </summary>
        public string work_place { get; set; }
        /// <summary>
        /// 备注。
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 所属部门ID列表。
        /// </summary>
        public List<int> dept_id_list { get; set; }
        /// <summary>
        /// 扩展属性，最大长度2000个字符。
        /// </summary>
        public string extension { get; set; }
        /// <summary>
        /// 入职时间，Unix时间戳，单位毫秒。
        /// </summary>
        public string hired_date { get; set; }
        /// <summary>
        /// 是否激活了钉钉：true：已激活; false：未激活
        /// </summary>
        public string active { get; set; }
        /// <summary>
        /// 是否完成了实名认证：true：已认证; false：未认证
        /// </summary>
        public string real_authed { get; set; }
        /// <summary>
        /// 是否为企业的高管：
        /// </summary>
        public string senior { get; set; }
        /// <summary>
        /// 是否为企业的管理员：
        /// </summary>
        public string admin { get; set; }
        /// <summary>
        /// 是否为企业的老板：
        /// </summary>
        public string boss { get; set; }
    }
}
