
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
    public class IgnoreController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = new{
                    query = "/api/Msg/Ignore",
                    newkey = "/api/Msg/Ignore/getnewkey",
                    edit = "/api/Msg/Ignore/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    IgnoreCode = "" ,
                    IgnoreReason = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "IgnoreCode",
                    postListFields = new string[] { "IgnoreCode" ,"IgnoreReason" ,"IgnoreOrder" ,"IgnoreRemark" }
                }
            };

            return View(model);
        }
    }

    public class IgnoreApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='IgnoreCode'>
    <select>*</select>
    <from>msg_ignore</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='IgnoreCode'		cp='equal'></field>   
        <field name='IgnoreReason'		cp='like'></field>   
    </where>
</settings>");
            var service = new msg_ignoreService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new msg_ignoreService().GetNewKey("IgnoreCode", "prefix", 1, null,"HL").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        msg_ignore
    </table>
    <where>
        <field name='IgnoreCode' cp='equal'></field>
    </where>
</settings>");
            var service = new msg_ignoreService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
