using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zephyr.Models;

namespace Zephyr.Web.Api
{
    public class ToYglz
    {
        private static readonly string urlcode = "yglz_advisoryAnswer";

        public static void InsertPost(string answerCode)
        {
            msg_advisoryAnswer answer = new msg_advisoryAnswerService().GetModel(Core.ParamQuery.Instance().AndWhere("AnswerCode", answerCode));
            api_advisoryAccess access = new api_advisoryAccessService().GetModel(Core.ParamQuery.Instance().AndWhere("AdvisoryCode", answer.AdvisoryCode).AndWhere("AccessResult", 1));
            if (access != null)
            {
                api_postService service = new api_postService();
                PostUpdateInfo puInfo = new PostUpdateInfo();

                puInfo.num = access.MasterCode;
                puInfo.msgid_app = access.AdvisoryCode;
                puInfo.content = answer.AnswerContent;
                puInfo.status = "";
                puInfo.time = DateTime.Now.ToString();

                string postCode = service.GetNewKey("PostCode", "dateplus");
                service.Insert(Core.ParamInsert.Instance()
                    .Column("PostCode", postCode)
                    .Column("UrlCode", urlcode)
                    .Column("PostContent", LitJson.JsonMapper.ToJson(puInfo))
                    );

                LawApi.Synchronous();
            }
        }

        public static void PostReply()
        {
            api_postService service = new api_postService();
            api_siteUrl siteUrl = new api_siteUrlService().GetModel(Core.ParamQuery.Instance().AndWhere("UrlCode", urlcode));
            List<api_post> list = service.GetModelList(Core.ParamQuery.Instance().AndWhere("UrlCode", urlcode).AndWhere("PostResult", 1, Core.Cp.NotEqual, new object[] { }));
            list = list.OrderBy(c => c.PostCode).ToList();

            foreach (api_post info in list)
            {
                string strResponse = "";
                string timestamp = ApiFun.ConvertDateTimeInt(DateTime.Now).ToString();
                string nonce = new Random().Next(1000, 9999).ToString();
                string signature = ApiFun.Encrypt(siteUrl.Token, timestamp, nonce);
                string para = "signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce;
                string url = siteUrl.ApiUrl + "?" + para;

                try
                {
                    strResponse = ApiFun.DoRequest(url, info.PostContent);

                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        AjaxResult ar = LitJson.JsonMapper.ToObject<AjaxResult>(strResponse);
                        if (ar.code == (int)AppEnums.ResponseStatus.请求成功 || ar.code == 70001 || ar.code == 20001)
                        {
                            service.Update(Core.ParamUpdate.Instance()
                                .Column("PostResult", 1)
                                .Column("PostRemark", "发送成功")
                                .AndWhere("PostCode", info.PostCode)
                                );
                        }
                        else
                        {
                            service.Update(Core.ParamUpdate.Instance()
                                .Column("PostResult", ar.code)
                                .Column("PostRemark", ar.msg)
                                .AndWhere("PostCode", info.PostCode));
                        }
                    }
                    else
                    {
                        service.Update(Core.ParamUpdate.Instance()
                            .Column("PostResult", -1)
                            .Column("PostRemark", "post请求没响应")
                            .AndWhere("PostCode", info.PostCode));
                    }
                }
                catch (Exception ex)
                {
                    service.Update(Core.ParamUpdate.Instance()
                        .Column("PostResult", -9)
                        .Column("PostRemark", ex.Message)
                        .AndWhere("PostCode", info.PostCode));
                }
            }
        }
    }
}