
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            var service = new sys_menuService();
                RequestWrapper requestWrapper = new RequestWrapper();
                var pQuerry = ParamQuery.Instance()
                    .Select("MenuCode as value, MenuName as text")
                    .AndWhere("ParentCode", DBNull.Value)
                    .OrWhere("ParentCode", "");
                var parentList = service.GetDynamicList(pQuerry);
            var model = new
            {
                dataSource = new
                {
                    parentList = parentList
                },
                urls = new
                {
                    query = "/api/Sys/Test",
                    newkey = "/api/Sys/Test/getnewkey",
                    edit = "/api/Sys/Test/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ParentCode=""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "_id",
                    postListFields = new string[] { "MenuCode", "ParentCode", "MenuName", "URL", "IconClass", "IconURL", "MenuSeq", "IsVisible", "IsEnable" }
                }
            };

            return View(model);
        }
    }

    public class TestApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='MenuCode'>
    <select>a.*,b.MenuName as ParentName</select>
    <from>sys_menu a left join sys_menu b on a.ParentCode=b.MenuCode</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='a.ParentCode' cp='equal' variable='ParentCode'></field>
    </where>
</settings>");
            var service = new sys_menuService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new sys_menuService().GetNewKey("MenuCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_menu
    </table>
    <where>
        <field name='MenuCode' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_menuService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
