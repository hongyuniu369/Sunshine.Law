using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    [MvcMobileFilter(true)]
    public class LawyerController : Controller
    {
        //
        // GET: /Lawyer/

        public ActionResult Index(RequestWrapper query)
        {
            var model = new
            {
                urls = new { query = "/api/Lawyer/" },
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
                    LawyerCode="",
                    LawyerName="",
                    LawyerPic="",
                    ContactNO = "",
                    MobilePhone="",
                    ProfessionalTitle="",
                    Address="",
                    LawyerDescription="",
                    LawyerRemark="",
                    ClassCode = query["MsgClass"] ?? ""
                },
                idField = "LawyerCode"
            };

            return View(model);
        }
        public ActionResult Show(string id)
        {
            dynamic Model = new ExpandoObject();
            Model.Lawyer = new law_lawyerService().GetLawyer(id);
            Model.Advisorys = new law_lawyerService().GetAdvisorys(id);
            return View(Model);
        }

    }


     [System.Web.Http.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LawyerApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='TotalScore desc,LawyerOrder,LawyerCode desc'>
    <select>*</select>
    <from>v_lawyerScore</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='LawyerName'		cp='like'></field>     
        <field name='Address'		    cp='like'></field>   
        <field name='LawyerCode'        cp='MapChild'         extend='law_lawyerClassMap,ClassCode,msg_class'    variable='ClassCode'   ignoreEmpty='true'>  </field> 
    </where>
</settings>");
            var service = new law_lawyerService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            foreach (var row in result.rows)
            {
                dynamic classes = service.GetLawyerClass(row.LawyerCode);
                string msgClasses = "";
                foreach (var presentClass in classes)
                {
                    msgClasses = msgClasses + presentClass.ClassName + ',';

                }
                if (msgClasses.EndsWith(","))
                {
                    msgClasses = msgClasses.Substring(0, msgClasses.Length - 1);
                }
                row.MsgClasses = msgClasses;
            }
            return result;
        }

        public IEnumerable<dynamic> GetLawyerClasses(string id)
        {
            return new law_lawyerService().GetLawyerClasses(id);
        }

    }
}
