using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using Zephyr.Models;

namespace Zephyr.Web.Api
{
    /// <summary>
    /// get 的摘要说明
    /// </summary>
    public class get : IHttpHandler
    {
        private readonly string urlcode = "yglz_access";
        api_siteUrl siteUrl;
        JsonData jsonData = new JsonData();

        HttpContext context;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            this.siteUrl = new api_siteUrlService().GetModel(Core.ParamQuery.Instance().AndWhere("UrlCode", urlcode));
                api_advisoryAccess accessinfo = new api_advisoryAccess();
                msg_advisory ainfo = new msg_advisory();
                api_advisoryAccessService accessService = new api_advisoryAccessService();

            try
            {
                if (!IsValidRequest())
                {
                    return;
                }

                string strInput = Zephyr.Web.Api.ApiFun.GetRequestContent();
                jsonData = JsonMapper.ToObject(strInput);
                GetModels(ref ainfo, ref accessinfo);

                accessinfo = accessService.Insert(accessinfo, ainfo);

                if (!string.IsNullOrEmpty(accessinfo.AccessCode))
                {
                    accessinfo.AccessResult = 1;
                    accessinfo.AccessRemark = "请求成功";
                    accessService.Update(Core.ParamUpdate.Instance()
                        .Column("AccessResult", accessinfo.AccessResult)
                        .Column("AccessRemark", accessinfo.AccessRemark)
                        .AndWhere("AccessCode", accessinfo.AccessCode)
                        );

                    context.Response.Write(AjaxResult.Success((int)AppEnums.ResponseStatus.请求成功, accessinfo.AdvisoryCode, AppEnums.ResponseStatus.请求成功.ToString()));
                }
                else
                {
                    accessinfo.AccessResult = -1;
                    accessinfo.AccessRemark = "数据库录入错误";
                    accessService.Update(Core.ParamUpdate.Instance()
                        .Column("AccessResult", accessinfo.AccessResult)
                        .Column("AccessRemark", accessinfo.AccessRemark)
                        .AndWhere("AccessCode", accessinfo.AccessCode)
                        );

                    context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.自定义错误, "数据库录入错误"));
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(accessinfo.AccessCode))
                {
                    accessinfo.AccessResult = -9;
                    accessinfo.AccessRemark = ex.Message;
                    accessService.Update(Core.ParamUpdate.Instance()
                        .Column("AccessResult", accessinfo.AccessResult)
                        .Column("AccessRemark", accessinfo.AccessRemark)
                        .AndWhere("AccessCode", accessinfo.AccessCode)
                        );
                }
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.自定义错误, ex.Message));
            }
        }

        public void GetModels(ref msg_advisory ainfo, ref api_advisoryAccess accessinfo)
        {
            if (((IDictionary)jsonData).Contains("title") && string.IsNullOrEmpty(jsonData["title"].ToString()))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数标题title, AppEnums.ResponseStatus.缺少参数标题title.ToString()));
                return;
            }
            ainfo.MsgTitle = jsonData["title"].ToString();

            if (((IDictionary)jsonData).Contains("num") && string.IsNullOrEmpty(jsonData["num"].ToString()))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数编号num, AppEnums.ResponseStatus.缺少参数编号num.ToString()));
                return;
            }
            accessinfo.MasterCode = jsonData["num"].ToString();

            if (((IDictionary)jsonData).Contains("content") && string.IsNullOrEmpty(jsonData["content"].ToString()))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数信息内容content, AppEnums.ResponseStatus.缺少参数信息内容content.ToString()));
                return;
            }
            ainfo.MsgContent = jsonData["content"].ToString();

            if (((IDictionary)jsonData).Contains("ispublish") && string.IsNullOrEmpty(jsonData["ispublish"].ToString()))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数是否公开ispublish, AppEnums.ResponseStatus.缺少参数是否公开ispublish.ToString()));
                return;
            }
            ainfo.IsOpen = Convert.ToBoolean(jsonData["ispublish"].ToString());

            if (((IDictionary)jsonData).Contains("ip") && string.IsNullOrEmpty(jsonData["ip"].ToString()))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数msgip, AppEnums.ResponseStatus.缺少参数msgip.ToString()));
                return;
            }
            ainfo.MsgIP = jsonData["ip"].ToString();

            if (((IDictionary)jsonData).Contains("location") && !string.IsNullOrEmpty(jsonData["location"].ToString()))
            {
                ainfo.DetailAddress = jsonData["location"].ToString();
            }

            ainfo.ContactNO = jsonData["mobile"].ToString();
            ainfo.Email = jsonData["email"].ToString();

            ainfo.Address = "";
            ainfo.MsgClass = "007";
            accessinfo.AccessDate = DateTime.Now;
            accessinfo.UrlCode = urlcode;
            ainfo.MsgInDate = DateTime.Now;
        }

        public bool IsValidRequest()
        {
            if (string.IsNullOrEmpty(context.Request.QueryString["timestamp"]))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数时间戳timestamp, AppEnums.ResponseStatus.缺少参数时间戳timestamp.ToString()));
                return false;
            }
            string timestamp = context.Request.QueryString["timestamp"];

            //判断是否超时
            DateTime dtRequest =ApiFun.ConvertIntDateTime(Convert.ToDouble(timestamp));
            TimeSpan ts = DateTime.Now - dtRequest;
            if (Math.Abs(ts.TotalMilliseconds) > 300000)
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.时间戳超时, AppEnums.ResponseStatus.时间戳超时.ToString()));
                return false;
            }

            if (string.IsNullOrEmpty(context.Request.QueryString["signature"]))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数签名signature, AppEnums.ResponseStatus.缺少参数签名signature.ToString()));
                return false;
            }
            string signature = context.Request.QueryString["signature"];

            if (string.IsNullOrEmpty(context.Request.QueryString["nonce"]))
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.缺少参数随机数nonce, AppEnums.ResponseStatus.缺少参数随机数nonce.ToString()));
                return false;
            }
            string nonce = context.Request.QueryString["nonce"];

            string strEncryted = ApiFun.Encrypt(siteUrl.Token, timestamp, nonce);

            if (string.Equals(strEncryted.ToLower(), signature.ToLower(), StringComparison.Ordinal))
            {
                return true;
            }
            else
            {
                context.Response.Write(AjaxResult.Error((int)AppEnums.ResponseStatus.自定义错误, "密钥错误"));
                return false;
            }
        }
    }
}