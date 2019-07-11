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
    public class NewsClassController : Controller
    {
        //
        // GET: /CMS/NewsClass/

        public ActionResult Index()
        {
            return View();
        }
    }

    public class NewsClassApiController : ApiController
    {
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='A.ClassOrder,A.ClassCode'>
    <select>
        A.*, B.ClassName As ParentClassName 
    </select>
    <from>
        news_class A
        left join news_class         B on A.ParentClassCode = B.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
        <field name='A.ParentClassCode'></field>
        <field name='A.IsEnable'></field>
    </where>
</settings>
");
            var result = new news_classService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public List<dynamic> GetAll(RequestWrapper request)
        {
            var pQuery = ParamQuery.Instance().AndWhere("IsEnable", 1);
            var result = new news_classService().GetDynamicList(pQuery);
            return result;
        }
        
        public dynamic GetRootNewsClass(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='ClassOrder, ClassCode'>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='IsEnable' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var paramQuery = request.ToParamQuery();
            paramQuery.OrWhere("ParentClassCode", DBNull.Value, Cp.IsNullOrEmpty);
            var result = new news_classService().GetDynamicListWithPaging(paramQuery);
            return result;
        }

        public string GetNewCode(RequestWrapper request)
        {
            var service = new news_classService();
            return service.GetNewKey("ClassCode", "maxplus").PadLeft(4, '0');
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings defaultOrderBy='ClassOrder, ClassCode'>
    <table>news_class</table>
    <where defaultForAll='false' defaultCp='equal' defaultIgnorEmpty='true'>
        <field name='ClassCode'></field>
    </where>
</settings>
");

            var service = new news_classService();
            service.Edit(null, listWrapper, data);
        }
    }
}
