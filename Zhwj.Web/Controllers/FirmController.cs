using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    [MvcMobileFilter(true)]
    public class FirmController : Controller
    {
        //
        // GET: /Firm/
        public ActionResult Index()
        {
            var model = new 
            {
                urls = new { query = "/api/Firm/" },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                pagination = new
                {
                    page = 1,
                    rows = 20
                },
                form = new
                {
                    FirmName = "",
                    FirmCode = "",
                    ContactNO = "",
                    FirmAddress = ""
                },
                idField = "FirmCode"
            };

            return View(model);
        }
        public ActionResult Show(string id)
        {
            dynamic Model = new ExpandoObject();
            Model.FirmClasses = new law_firmService().GetFirmClass(id);
            Model.Lawyers = new law_firmService().GetLawyers(id);
            Model.Form = new law_firmService().GetModel(ParamQuery.Instance().AndWhere("FirmCode", id));

            return View(Model);
        }
    }

    [System.Web.Http.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class FirmApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='FirmOrder,FirmCode desc'>
    <select>*</select>
    <from>law_firm</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='FirmName'		cp='like'></field>   
        <field name='FirmCode'		cp='like'></field>   
        <field name='ContactNO'		cp='like'></field>   
        <field name='FirmAddress'		cp='equal'></field>   
    </where>
</settings>");
            var service = new law_firmService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public IEnumerable<dynamic> GetFirmClasses(string id)
        {
            return new law_firmService().GetFirmClasses(id);
        }

        [System.Web.Http.HttpPost]
        public void EditFirmClasses(string id, dynamic data)
        {
            var service = new law_firmService();
            service.SaveFirmClasses(id, data as JToken);
        }
    }
}
