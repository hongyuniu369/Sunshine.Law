using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Zephyr.Models;
using Zephyr.Core;

namespace Zephyr.Areas.CMS.Controllers
{
    public class LinkController : Controller
    {
        //
        // GET: /CMS/Link/

        public ActionResult Index()
        {
            //设置上传路径
            var response = System.Web.HttpContext.Current.Response;
            var cookieCKFinder_Path = new System.Web.HttpCookie("CKFinder_RelativePath") { Value = "~/files/news/link/", Expires = DateTime.MaxValue };
            response.Cookies.Remove("CKFinder_RelativePath");
            response.Cookies.Add(cookieCKFinder_Path);

            return View();
        }
    }

    public class LinkApiController : ApiController
    {
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='A.LinkOrder,A.LinkCode'>
    <select>
        A.*, B.LinkName As ParentLinkName 
    </select>
    <from>
        link A
        left join link         B on A.ParentLinkCode = B.LinkCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
        <field name='A.ParentLinkCode'></field>
        <field name='A.IsEnable'></field>
    </where>
</settings>
");
            var result = new linkService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public List<dynamic> GetAll(RequestWrapper request)
        {
            var pQuery = ParamQuery.Instance().AndWhere("IsEnable", 1);
            var result = new linkService().GetDynamicList(pQuery);
            return result;
        }

        public dynamic GetRootLink(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='LinkOrder, LinkCode'>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='IsEnable' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var paramQuery = request.ToParamQuery();
            paramQuery.OrWhere("ParentLinkCode", DBNull.Value, Cp.IsNullOrEmpty);
            var result = new linkService().GetDynamicListWithPaging(paramQuery);
            return result;
        }

        public string GetNewCode(RequestWrapper request)
        {
            var service = new linkService();
            return service.GetNewKey("LinkCode", "maxplus").PadLeft(4, '0');
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings defaultOrderBy='LinkOrder, LinkCode'>
    <table>link</table>
    <where defaultForAll='false' defaultCp='equal' defaultIgnorEmpty='true'>
        <field name='LinkCode'></field>
    </where>
</settings>
");

            var service = new linkService();
            service.Edit(null, listWrapper, data);
        }
    }
}
