using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth.Base
{
    public abstract class BasePlatform : IBasePlatform
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        protected ConfigEntity Config;

        public BasePlatform(ConfigEntity config)
        {
            this.Config = config ?? throw new Exception("传入配置参数不能为空");
        }

        public abstract string GetOpenId();
        public abstract string GetRawUserInfo();
        public abstract string GetRedirectUrl();
        public abstract FormatedUserInfo GetUserInfo();
    }
}
