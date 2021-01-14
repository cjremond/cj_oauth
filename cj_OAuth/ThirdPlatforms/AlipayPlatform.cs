using cj_OAuth.Base;
using cj_OAuth.Models;
using cj_OAuth.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace cj_OAuth.ThirdPlatforms
{
    /// <summary>
    /// 第三方登录：支付宝扫码登录
    /// 参考网站：https://opendocs.alipay.com/open/284/web
    /// </summary>
    public class AlipayPlatform : BasePlatform
    {
        private string Authorize_Url = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";
        private string AccessToken_Url = "https://openapi.alipay.com/gateway.do";

        private AlipayParamEntity param = new AlipayParamEntity();
        public AlipayPlatform(ConfigEntity config) : base(config)
        {
            param.app_id = config.app_id;
            param.redirect_uri = config.callback;
            param.scope = config.scope;
            param.state = config.state;
        }

        public override string GetRedirectUrl()
        {
            return $"{Authorize_Url}?{ParamUtil.BuildParams(param)}";
        }

        public override string GetOpenId()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null)
            {
                throw new Exception("没有获取到支付宝用户ID！");
            }
            return accessToken.alipay_system_oauth_token_response.user_id;
        }

        public override FormatedUserInfo GetUserInfo()
        {
            var userInfoRawJson = GetRawUserInfo();
            if (string.IsNullOrWhiteSpace(userInfoRawJson))
            {
                throw new Exception("获取支付宝用户信息失败");
            }

            AlipayUserinfoEntity alipay_Userinfo = JsonConvert.DeserializeObject<AlipayUserinfoEntity>(userInfoRawJson);
            return new FormatedUserInfo()
            {
                OpenId = alipay_Userinfo.alipay_user_info_share_response.user_id,
                Channel = "alipay",
                NickName = alipay_Userinfo.alipay_user_info_share_response.nick_name,
                Gender = alipay_Userinfo.alipay_user_info_share_response.gender.ToLower(),
                Avatar = alipay_Userinfo.alipay_user_info_share_response.avatar
            };
        }


        public override string GetRawUserInfo()
        {
            var accessToken = GetAccessToken();
            if (accessToken == null || accessToken.alipay_system_oauth_token_response == null)
            {
                throw new Exception("获取支付宝 ACCESS_TOKEN 出错");
            }

            AlipayAccessTokenParamEntity alipayAccessToken = new AlipayAccessTokenParamEntity
            {
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                method = "alipay.user.info.share",
                app_id = param.app_id,
                sign_type = "RSA2",
                version = "1.0",
                charset = "utf-8",
                auth_token = accessToken.alipay_system_oauth_token_response.access_token,
            };
            alipayAccessToken.sign = System.Web.HttpUtility.UrlEncode(Signature(alipayAccessToken));
            var url = $"{AccessToken_Url}?{ParamUtil.BuildSortedParams(alipayAccessToken, false, "")}";
            var result = HttpUtil.Get(url);

            AlipayUserinfoEntity alipay_Userinfo = JsonConvert.DeserializeObject<AlipayUserinfoEntity>(result);
            if (alipay_Userinfo == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(alipay_Userinfo);
            }
        }

        /// <summary>
        /// http直接访问 https://openapi.alipay.com/gateway.do?timestamp=2013-01-01 08:08:08&method=alipay.system.oauth.token&app_id=4472&sign_type=RSA2&sign=ERITJKEIJKJHKKKKKKKHJEREEEEEEEEEEE&version=1.0&charset=GBK&grant_type=authorization_code&code=4b203fe6c11548bcabd8da5bb087a83b
        /// auth_code授权之后的回执
        /// </summary>
        /// <returns></returns>
        private AlipayAccessTokenEntity GetAccessToken()
        {
            AlipayAccessTokenParamEntity alipayAccessToken = new AlipayAccessTokenParamEntity
            {
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                method = "alipay.system.oauth.token",
                app_id = param.app_id,
                sign_type = "RSA2",
                version = "1.0",
                charset = "utf-8",
                grant_type = "authorization_code",
                code = HttpContext.Current.Request.QueryString["auth_code"],
                format = "",
            };
            alipayAccessToken.sign = System.Web.HttpUtility.UrlEncode(Signature(alipayAccessToken));
            var url = $"{AccessToken_Url}?{ParamUtil.BuildSortedParams(alipayAccessToken, false, "")}";
            var result = HttpUtil.Get(url);
            AlipayAccessTokenEntity alipay_AccessToken = JsonConvert.DeserializeObject<AlipayAccessTokenEntity>(result);
            return alipay_AccessToken;
        }

        private static string GetCurrentPath()
        {
            string basePath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
            basePath = "D:\\QrCodeLogin\\01GitHub\\QrCode_Login";
            return basePath + "/AliKeyPems/";
        }

        /// <summary>
        /// 使用工具调试签名流程介绍 https://opensupport.alipay.com/support/helpcenter/286/201602644045?ant_source=antsupport
        /// </summary>
        /// <param name="alipayAccessToken"></param>
        /// <returns></returns>
        private string Signature(AlipayAccessTokenParamEntity alipayAccessToken)
        {
            //所有请求参数，不包括字节类型参数，如文件、字节流，剔除 sign 字段，剔除值为空的参数，
            //并按照第一个字符的键值 ASCII 码递增排序（字母升序排序），如果遇到相同字符则按照第二个字符的键值 ASCII 码递增排序，以此类推。
            var unsignString = ParamUtil.BuildSortedParams(alipayAccessToken);
            //私钥的绝对路径
            var privateKeyPemPath = GetCurrentPath() + "RSA2_private_key.txt";
            //加密。目前只考虑“普通公钥”方式
            return RSASignCharSet(unsignString, privateKeyPemPath, "utf-8", "RSA2");
        }

        #region 拷贝至阿里加密  AlipaySignature.cs

        public static string RSASignCharSet(string data, string privateKeyPem, string charset, string signType)
        {
            RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);
            byte[] dataBytes = null;
            if (string.IsNullOrEmpty(charset))
            {
                dataBytes = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
            }

            if ("RSA2".Equals(signType))
            {
                byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA256");
                return Convert.ToBase64String(signatureBytes);
            }
            else
            {
                byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");
                return Convert.ToBase64String(signatureBytes);
            }
        }

        private static RSACryptoServiceProvider LoadCertificateFile(string filename, string signType)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res, signType);
                    return rsa;
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }

        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            //string header = String.Format("-----BEGIN {0}-----\\n", type);
            //string footer = String.Format("-----END {0}-----", type);
            //int start = pem.IndexOf(header) + header.Length;
            //int end = pem.IndexOf(footer, start);
            //string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(pem);
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #endregion
    }
}
