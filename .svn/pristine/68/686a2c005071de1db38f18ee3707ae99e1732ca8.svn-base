
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Msg.Controllers
{
    public class SiteUrlController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = MsgHelper.GetIndexUrls("SiteUrl"),
                resx = MsgHelper.GetIndexResx("站点接口"),
                form = new
                {
                    UrlCode = "" ,
                    SiteCode = "" ,
                    ApiUrl = "" ,
                    Token = "" ,
                    HttpMode = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "UrlCode",
                    postListFields = new string[] { "UrlCode" ,"SiteCode" ,"ApiUrl" ,"SiteRmark" ,"Token" ,"HttpMode" ,"Indate" }
                }
            };
            model.urls.edit = "/api/Msg/SiteUrl/edit/";

            return View(model);
        }
    }

    public class SiteUrlApiController : MsgBaseApi<api_siteUrl, api_siteUrlService>
    {
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='UrlCode'>
    <select>*</select>
    <from>api_siteUrl</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UrlCode'		cp='equal'></field>   
        <field name='SiteCode'		cp='equal'></field>   
        <field name='ApiUrl'		cp='like'></field>   
        <field name='Token'		cp='like'></field>   
        <field name='HttpMode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new api_siteUrlService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public override void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        api_siteUrl
    </table>
    <where>
        <field name='UrlCode' cp='equal'></field>
    </where>
</settings>");
            var service = new api_siteUrlService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
