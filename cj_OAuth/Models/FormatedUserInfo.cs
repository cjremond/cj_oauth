using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cj_OAuth
{
    public class FormatedUserInfo
    {
        public string OpenId { get; set; }
        public string UnionId { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Channel { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
    }
}
