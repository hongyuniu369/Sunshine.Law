
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = SysHelper.GetIndexUrls("User"),
                resx = SysHelper.GetIndexResx("sys_user"),
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    Description = "" 
                },
                idField="UserCode"
            };

            return View(model);
        }
    }

    public class UserApiController : SysBaseApi<sys_user, sys_userService>
    {
        
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='UserCode'>
    <select>*</select>
    <from>sys_user</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Description'		cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_userService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
