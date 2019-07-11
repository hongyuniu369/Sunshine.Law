using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Zephyr.Web.Api
{
    public static class ApiFun
    {
        static string token = "hebnews";

        /// <summary>
        /// 加密认证
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static string Encrypt(string token, string timestamp, string nonce)
        {
            IList<string> lst = new List<string>();
            lst.Add(token);
            lst.Add(timestamp); lst.Add(nonce);

            lst = lst.OrderBy(c => c).ToList();
            string strOrdered = "";
            foreach (var str in lst)
            {
                strOrdered += str;
            }
            string sResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strOrdered, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());

            return sResult;
        }

        #region Timestamp Handler
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {

            System.DateTime time = System.DateTime.MinValue;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            time = startTime.AddSeconds(d);

            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {

            double intResult = 0;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            intResult = (time - startTime).TotalSeconds;

            return intResult;

        }

        #endregion

        #region HttpWeb Handler

        /// <summary>
        /// 获取访问内容
        /// </summary>
        /// <returns></returns>
        public static string GetRequestContent()
        {
            Stream inputStream = HttpContext.Current.Request.InputStream;
            byte[] bytes = new byte[inputStream.Length];
            inputStream.Read(bytes, 0, Convert.ToInt32(inputStream.Length));
            string strInput = System.Text.Encoding.UTF8.GetString(bytes);

            return strInput;
        }

        /// <summary>
        /// 发送访问内容
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string DoRequest(string uri, string para)
        {
            string strResult = "";
            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                req = HttpWebRequest.Create(uri) as HttpWebRequest;
                //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
                req.Method = "POST";        // Post method
                req.ContentType = "text/plain";     // content type
                req.AllowAutoRedirect = false;
                // Wrap the request stream with a text-based writer
                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                // Write the text into the stream
                writer.WriteLine(para);
                writer.Close();

                //Send the data to the webserver
                rsp = (HttpWebResponse)req.GetResponse();
                Stream stream = rsp.GetResponseStream();
                byte[] bytes = new byte[rsp.ContentLength];
                rsp.GetResponseStream().Read(bytes, 0, Convert.ToInt32(rsp.ContentLength));
                strResult = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (WebException webEx)
            {
                strResult = webEx.Message;
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            finally
            {
                if (req != null) req.GetRequestStream().Close();
                if (rsp != null) rsp.GetResponseStream().Close();
            }
            return strResult;
        }

        #endregion
    }
    /// <summary>
    /// 请求结果枚举
    /// </summary>
    public static class AppEnums
    {
        public enum ResponseStatus
        {
            请求成功 = 20000,
            暂无数据 = 40001,
            系统繁忙请稍后再试 = 40002,
            时间戳超时 = 50001,
            非法的签名signature = 50002,
            非法的编号num = 50003,
            缺少参数时间戳timestamp = 60001,
            缺少参数签名signature = 60002,
            缺少参数随机数nonce = 60003,
            缺少参数信息id = 60004,
            缺少参数标题title = 60005,
            缺少参数信息内容content = 60006,
            缺少参数投诉类型type = 60007,
            缺少参数是否公开ispublish = 60008,
            缺少参数用户电话mobile = 60009,
            缺少参数编号num = 60010,
            缺少参数受理时间time = 60011,
            缺少参数受理部门depart = 60012,
            缺少参数受理状态status = 60013,
            缺少参数msgip = 60014,
            缺少参数msgid_app = 60015,
            缺少参数place = 60016,
            自定义错误 = 80000
        }
    }

    /// <summary>
    /// 请求返回结果.
    /// </summary>
    public partial class AjaxResult
    {
        public AjaxResult()
        {
        }

        private bool _success = true;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get { return _success; } }

        /// <summary>
        /// 返回代码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 错误信息，或者成功信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 成功时可能返回的数据
        /// </summary>
        public object data { get; set; }

        public string num { get; set; }

        #region Error
        public static AjaxResult Error()
        {
            return new AjaxResult()
            {
                _success = false
            };
        }
        public static AjaxResult Error(int code)
        {
            return new AjaxResult()
            {
                _success = false,
                code = code
            };
        }
        public static AjaxResult Error(int code, string msg)
        {
            return new AjaxResult()
            {
                _success = false,
                code = code,
                msg = msg
            };
        }
        #endregion

        #region Success
        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
                _success = true
            };
        }
        public static AjaxResult Success(int code, string num)
        {
            return new AjaxResult()
            {
                _success = true,
                code = code,
                num = num
            };
        }
        public static AjaxResult Success(int code, string num, object data)
        {
            return new AjaxResult()
            {
                _success = true,
                code = code,
                num = num,
                data = data
            };
        }
        #endregion

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }

    /// <summary>
    /// 数据Base64加密 解密
    /// </summary>
    public sealed class Base64
    {
        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            return EncodeBase64(bytes);
        }

        public static string EncodeBase64(byte[] bytes)
        {
            string sResult;
            try
            {
                sResult = Convert.ToBase64String(bytes);
            }
            catch
            {
                sResult = null;
            }
            return sResult;
        }

        public static string EncodeBase64(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
            return EncodeBase64(bytes);
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static byte[] DecodeBase64(string result)
        {
            return Convert.FromBase64String(result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }

    public class PostInfo
    {
        public string id { get; set; }
        public string num { get; set; }
        public string msgid_app { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string ispublish { get; set; }
        public string username { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string pic { get; set; }
        public string createdate { get; set; }
        public string location { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public IList<ReplyInfo> Replys { get; set; }
    }

    public class PostUpdateInfo
    {
        public string num { get; set; }
        public string replynum { get; set; }
        public string msgid_app { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public string depart { get; set; }
        public string status { get; set; }
    }

    public class ReplyInfo
    {
        public string replynum { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public string depart { get; set; }
        public string isopen { get; set; }
    }
}