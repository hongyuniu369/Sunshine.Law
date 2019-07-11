using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class ListController : Controller
    {
        public ActionResult Index(string id)
        {
            news_class newsClass;
            if (id == "jwdt")
                newsClass = new news_class() {ClassCode="jwdt", ClassName = "教务动态" };
            else
                newsClass = new news_classService().GetModel(ParamQuery.Instance().AndWhere("ClassCode", id));

            var model = new
            {
                urls = new { query = "/api/List/" },
                dataSource = new
                {
                    newsClass = newsClass
                },
                pagination = new
                {
                    page = 1,
                    rows = 20
                },
                form = new
                {
                    Title = "",
                    ClassCode = (newsClass == null|| id=="jwdt") ? "" : newsClass.ClassCode,
                    ClassCodeIn = id == "jwdt" ? "'0010','0006','0007','0005','0008','0009'" : "",
                    ApproveState = "passed",
                    Signed = true
                },
                idField = "NewsCode"
            };

            return View(model);
        }
    }

    [System.Web.Http.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class ListApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='SignedTime desc'>
    <select>Title,NewsCode,SignedTime</select>
    <from>news</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='NewsCode'		cp='equal'></field>   
        <field name='ClassCode'		cp='equal'></field>   
        <field name='ClassCode'		cp='in'  variable='ClassCodeIn'></field>   
        <field name='Title'		cp='like'></field>   
        <field name='Checked'		cp='equal'></field>   
        <field name='Signed'		cp='equal'></field>   
        <field name='InTime'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new newsService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
