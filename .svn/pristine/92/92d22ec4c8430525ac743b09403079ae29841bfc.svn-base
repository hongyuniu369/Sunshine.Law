
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
    public class AdvisoryAccessController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = MsgHelper.GetIndexUrls("AdvisoryAccess"),
                resx = MsgHelper.GetIndexResx("咨询接收"),
                form = new
                {
                    UrlCode = "" ,
                    MasterCode = "" ,
                    AdvisoryCode = "" ,
                    AccessResult = "" ,
                    AccessDate = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "AccessCode",
                    postListFields = new string[] { "AccessCode" ,"UrlCode" ,"MasterCode" ,"AdvisoryCode" ,"AccessResult" ,"AccessRemark" ,"AccessDate" }
                }
            };
            model.urls.edit = "/api/Msg/AdvisoryAccess/edit/";

            return View(model);
        }
    }

    public class AdvisoryAccessApiController : MsgBaseApi<api_advisoryAccess, api_advisoryAccessService>
    {
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AccessCode'>
    <select>*</select>
    <from>api_advisoryAccess</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UrlCode'		cp='equal'></field>   
        <field name='MasterCode'		cp='equal'></field>   
        <field name='AdvisoryCode'		cp='equal'></field>   
        <field name='AccessResult'		cp='equal'></field>   
        <field name='AccessDate'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new api_advisoryAccessService();
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
        api_advisoryAccess
    </table>
    <where>
        <field name='AccessCode' cp='equal'></field>
    </where>
</settings>");
            var service = new api_advisoryAccessService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
