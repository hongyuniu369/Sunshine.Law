using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Msg.Controllers
{
    public class LawyerController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("lawyer"),
                resx = MsgHelper.GetIndexResx("律师"),
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    LawyerCode = "",
                    FirmCode = "",
                    LawyerName = "",
                    LicenseNumber = ""
                },
                idField = "LawyerCode"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            //如果是律师自己修改律师信息
            if (string.IsNullOrEmpty(id))
            {
                id = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode;
            }

            //设置上传路径
            var response = System.Web.HttpContext.Current.Response;
            var cookieCKFinder_Path = new System.Web.HttpCookie("CKFinder_RelativePath") { Value = "~/files/lawyer/" + id + "/", Expires = DateTime.MaxValue };
            response.Cookies.Remove("CKFinder_RelativePath");
            response.Cookies.Add(cookieCKFinder_Path);

            var model = new
            {
                urls = MsgHelper.GetEditUrls("lawyer"),
                resx = MsgHelper.GetEditResx("律师"),
                dataSource = new
                {
                    pageData = new LawyerApiController().GetEditMaster(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new law_lawyer().Extend(new { LawyerCode = id, FirmCode = "", LawyerName = "", LicenseNumber = "", LawyerPic = "", ContactNO = "", MobilePhone = "", Email = "", ProfessionalTitle = "", Address = "", DetailAddress = "", LawyerDescription = "", LawyerRemark = "", LawyerOrder = "99" }),
                    primaryKeys = new string[] { "LawyerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            return View(model);
        }
    }

    public class LawyerApiController : MsgBaseApi<law_lawyer, law_lawyerService>
    {
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='LawyerCode desc'>
    <select>A.*, B.FirmName</select>
    <from>
        law_lawyer A Left outer join law_firm B on A.FirmCode=B.FirmCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='LawyerCode'		cp='like'></field>   
        <field name='A.FirmCode'		cp='equal'></field>   
        <field name='LawyerName'		cp='like'></field>   
        <field name='LicenseNumber'		cp='like'></field>   
    </where>
</settings>");
            var service = new law_lawyerService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public override dynamic GetEditMaster(string id)
        {
            var masterService = new law_lawyerService();
            var pQuery = ParamQuery.Instance().AndWhere("LawyerCode", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("LawyerCode", id),
                lawyerClass = masterService.GetLawyerClass(id)

                //明细数据
            };
            return result;
        }

        public IEnumerable<dynamic> GetLawyerClasses(string id)
        {
            return new law_lawyerService().GetLawyerClasses(id);
        }

        [System.Web.Http.HttpPost]
        public void EditLawyerClasses(string id, dynamic data)
        {
            var service = new law_lawyerService();
            service.SavelawyerClasses(id, data as JToken);
        }

        #region 自动完成
        // GET api/msg/lawyer/getlayername
        public virtual List<dynamic> GetLayerName(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 LwyerName").AndWhere("LawyerName", q, Cp.Like);
            var service = new law_lawyerService();
            return service.GetDynamicList(pQuery);
        }
        // GET api/msg/firm/getfirmname
        public virtual List<dynamic> GetLawyerCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 LawyerCode").AndWhere("LawyerCode", q, Cp.Like);
            var service = new law_lawyerService();
            return service.GetDynamicList(pQuery);
        }
        // GET api/msg/firm/getlincencenumber
        public virtual List<dynamic> GetLincenceNumber(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 LincenceNumber").AndWhere("LincenceNumber", q, Cp.Like);
            var service = new law_lawyerService();
            return service.GetDynamicList(pQuery);
        }
        #endregion
    }
}
