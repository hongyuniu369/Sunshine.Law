using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using System.Linq;

namespace Zephyr.Areas.Msg.Controllers
{
    public class FirmController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("firm"),
                resx = MsgHelper.GetIndexResx("律师事务所"),
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    FirmName = "" ,
                    FirmCode = "" ,
                    ContactNO = "" ,
                    FirmAddress = "" 
                },
                idField="FirmCode"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            //设置上传路径
            var response = System.Web.HttpContext.Current.Response;
            var cookieCKFinder_Path = new System.Web.HttpCookie("CKFinder_RelativePath") { Value = "~/files/firm/", Expires = DateTime.MaxValue };
            response.Cookies.Remove("CKFinder_RelativePath");
            response.Cookies.Add(cookieCKFinder_Path);

            var model = new
            {
                urls = MsgHelper.GetEditUrls("firm"),
                resx = MsgHelper.GetEditResx("律所"),
                dataSource = new
                {
                    pageData = new FirmApiController().GetEditMaster(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new law_firm().Extend(new { FirmCode = id, FirmName = "", FirmLogo="", FirmAddress = "", FrimDetailAddress = "", ZIPCode = "", ContactNO = "", Fax = "", Email = "", WebAddress = "", FirmDescription = "", FirmRemark = "", FirmOrder = "99" }),
                    primaryKeys = new string[] { "FirmCode" }
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

    public class FirmApiController : MsgBaseApi<law_firm, law_firmService>
    {
        public override dynamic Get(RequestWrapper query)
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

        public override dynamic GetEditMaster(string id)
        {
            var masterService = new law_firmService();
            var pQuery = ParamQuery.Instance().AndWhere("FirmCode", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("FirmCode", id),
                firmClass= masterService.GetFirmClass(id)

                //明细数据
            };
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

        #region 自动完成
        // GET api/msg/firm/getfirmname
        public virtual List<dynamic> GetFirmName(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 FirmName").AndWhere("FirmName", q, Cp.Like);
            var service = new law_firmService();
            return service.GetDynamicList(pQuery);
        }
        // GET api/msg/firm/getfirmname
        public virtual List<dynamic> GetFirmCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 FirmCode").AndWhere("FirmCode", q, Cp.Like);
            var service = new law_firmService();
            return service.GetDynamicList(pQuery);
        }
        #endregion
    }
}
