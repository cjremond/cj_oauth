using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Base
{
    interface IBasePlatform
    {
        /// <summary>
        /// 获取跳转地址
        /// </summary>
        /// <returns></returns>
        string GetRedirectUrl();
        /// <summary>
        /// 获取用户的openid标识
        /// </summary>
        /// <returns></returns>
        string GetOpenId();
        /// <summary>
        /// 获取格式化后的用户信息
        /// </summary>
        FormatedUserInfo GetUserInfo();
        /// <summary>
        /// 获取接口返回的原始用户信息
        /// </summary>
        string GetRawUserInfo();
    }
}
