using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace cj_OAuth.Utils
{
    public class HttpUtil
    {
        /// <summary>
        /// POST 请求，UTF8编码数据
        /// </summary>
        public static string Post(string postData, string url, int timeout = 15, string contentType = "application/x-www-form-urlencoded", bool isUseCert = false, string certPath = null, string certPassword = null)
        {
            if (isUseCert)
            {
                if (certPath == null)
                {
                    throw new ArgumentException("Cert Path is Null");
                }

                if (certPassword == null)
                {
                    throw new ArgumentException("Cert Password is Null");
                }
            }

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                request = (HttpWebRequest)WebRequest.Create(url);

                //支持HTTPS
                if (url.StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                    //默认执行Basic验证
                    if (!string.IsNullOrEmpty(certPath))
                    {
                        request.Credentials = new NetworkCredential(certPath, certPassword);
                        request.PreAuthenticate = true;
                    }
                }

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置POST的数据类型和长度
                request.ContentType = contentType;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
                    request.ClientCertificates.Add(cert);
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();

            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }


        /// <summary>
        /// GET 请求，UTF8编码数据
        /// </summary>
        public static string Get(string url, int timeout = 15, Encoding encoding = null, string certPath = null, string certPassword = null)
        {
            string result = null;//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.Timeout = timeout * 1000;

                //支持HTTPS
                if (url.StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                    //默认执行Basic验证
                    if (!string.IsNullOrEmpty(certPath))
                    {
                        request.Credentials = new NetworkCredential(certPath, certPassword);
                        request.PreAuthenticate = true;
                    }
                }

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();
                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), encoding ?? Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


        ///将数据流转为byte[]
        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }

    }
}
