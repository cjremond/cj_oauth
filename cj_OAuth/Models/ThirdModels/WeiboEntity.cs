using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Models
{
    class WeiboEntity
    {
    }

    public class WbParamEntity
    {
        /// <summary>
        /// 申请应用时分配的AppKey。
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 申请应用时分配的AppSecret。
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// 授权回调地址，站外应用需与设置的回调地址一致，站内应用需填写canvas page的地址。
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 申请scope权限所需参数，可一次申请多个scope权限，用逗号分隔
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 用于保持请求和回调的状态，在回调时，会在Query Parameter中回传该参数。
        /// 开发者可以用这个参数验证请求有效性，也可以记录用户请求授权页前的位置。
        /// 这个参数可用于防止跨站请求伪造（CSRF）攻击
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 授权页面的终端类型
        /// </summary>
        public string display { get; set; } = "default";
        /// <summary>
        /// 是否强制用户重新登录，true：是，false：否。默认false。
        /// </summary>
        public bool forcelogin { get; set; } = false;
        /// <summary>
        /// 授权页语言，缺省为中文简体版，en为英文版。
        /// </summary>
        public string language { get; set; }
    }

    public class WbAccessTokenEntity
    {
        /// <summary>
        /// 用户授权的唯一票据，用于调用微博的开放接口，
        /// 同时也是第三方应用验证微博用户登录的唯一票据，
        /// 第三方应用应该用该票据和自己应用内的用户建立唯一影射关系，来识别登录状态，不能使用本返回值里的UID字段来做登录识别。
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token的生命周期，单位是秒数。
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// access_token的生命周期（该参数即将废弃，开发者请使用expires_in）。
        /// </summary>
        public string remind_in { get; set; }
        /// <summary>
        /// 授权用户的UID，本字段只是为了方便开发者，减少一次user/show接口调用而返回的，
        /// 第三方应用不能用此字段作为用户登录状态的识别，只有access_token才是用户授权的唯一票据。
        /// </summary>
        public string uid { get; set; }
    }

    public class WbUserinfoEntity
    {
        /// <summary>
        /// 用户UID
        /// </summary> 
        public long id { get; set; }
        /// <summary>
        /// 字符串型的用户UID
        /// </summary> 
        public string idstr { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary> 
        public string screen_name { get; set; }
        /// <summary>
        /// 友好显示名称
        /// </summary> 
        public string name { get; set; }
        /// <summary>
        /// 用户所在省级ID
        /// </summary> 
        public int province { get; set; }
        /// <summary>
        /// 用户所在城市ID
        /// </summary> 
        public int city { get; set; }
        /// <summary>
        /// 用户所在地
        /// </summary> 
        public string location { get; set; }
        /// <summary>
        /// 用户个人描述
        /// </summary> 
        public string description { get; set; }
        /// <summary>
        /// 用户博客地址
        /// </summary> 
        public string url { get; set; }
        /// <summary>
        /// 用户头像地址（中图），50×50像素
        /// </summary> 
        public string profile_image_url { get; set; }
        /// <summary>
        /// 用户的微博统一URL地址
        /// </summary> 
        public string profile_url { get; set; }
        /// <summary>
        /// 用户的个性化域名
        /// </summary> 
        public string domain { get; set; }
        /// <summary>
        /// 用户的微号
        /// </summary> 
        public string weihao { get; set; }
        /// <summary>
        /// 性别，m：男、f：女、n：未知
        /// </summary> 
        public string gender { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary> 
        public int followers_count { get; set; }
        /// <summary>
        /// 关注数
        /// </summary> 
        public int friends_count { get; set; }
        /// <summary>
        /// 微博数
        /// </summary> 
        public int statuses_count { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary> 
        public int favourites_count { get; set; }
        /// <summary>
        /// 用户创建（注册）时间
        /// </summary> 
        public string created_at { get; set; }
        /// <summary>
        /// 暂未支持
        /// </summary> 
        public bool following { get; set; }
        /// <summary>
        /// 是否允许所有人给我发私信，true：是，false：否
        /// </summary> 
        public bool allow_all_act_msg { get; set; }
        /// <summary>
        /// 是否允许标识用户的地理位置，true：是，false：否
        /// </summary> 
        public bool geo_enabled { get; set; }
        /// <summary>
        /// 是否是微博认证用户，即加V用户，true：是，false：否
        /// </summary> 
        public bool verified { get; set; }
        /// <summary>
        /// 暂未支持
        /// </summary> 
        public int verified_type { get; set; }
        /// <summary>
        /// 用户备注信息，只有在查询用户关系时才返回此字段
        /// </summary> 
        public string remark { get; set; }
        /// <summary>
        /// 用户的最近一条微博信息字段 详细
        /// </summary> 
        public object status { get; set; }
        /// <summary>
        /// 是否允许所有人对我的微博进行评论，true：是，false：否
        /// </summary> 
        public bool allow_all_comment { get; set; }
        /// <summary>
        /// 用户头像地址（大图），180×180像素
        /// </summary> 
        public string avatar_large { get; set; }
        /// <summary>
        /// 用户头像地址（高清），高清头像原图
        /// </summary> 
        public string avatar_hd { get; set; }
        /// <summary>
        /// 认证原因
        /// </summary> 
        public string verified_reason { get; set; }
        /// <summary>
        /// 该用户是否关注当前登录用户，true：是，false：否
        /// </summary> 
        public bool follow_me { get; set; }
        /// <summary>
        /// 用户的在线状态，0：不在线、1：在线
        /// </summary> 
        public int online_status { get; set; }
        /// <summary>
        /// 用户的互粉数
        /// </summary> 
        public int bi_followers_count { get; set; }
        /// <summary>
        /// 用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语
        /// </summary> 
        public string lang { get; set; }

    }
}
