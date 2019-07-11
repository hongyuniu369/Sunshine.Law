using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zephyr.Areas.Msg;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Web.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class WapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string id)
        {
            dynamic model = new ExpandoObject();
            model.page = new
            {
                urls = MsgHelper.GetEditUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetEditResx("咨询答复"),
                dataSource = new
                {
                    pageData = new Zephyr.Areas.Msg.Controllers.AdvisoryAnswerApiController().GetEditMaster("-1"),
                    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    defaults = new msg_advisoryAnswer().Extend(new { AnswerCode = "", AdvisoryCode = id, AnswerContent = "", IsSecret = "", AnswerDate = "", PraiseCount = "", AnswerOrder = "" }),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.page.urls.edit = "/api/zx/PostAnswer";
            model.page.resx.editSuccess = "答复已提交，感谢您的热心帮助。";
            //获取咨询信息
            model.form = new msg_advisoryService().GetModel(ParamQuery.Instance().AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { }).AndWhere("AdvisoryCode", id));
            if (model.form == null)
                return RedirectToAction("Error", "Home");
            model.msgStatus = Enum.GetName(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus), Convert.ToInt32(model.form.MsgStatus));
            model.user = new sys_user();
            if (!string.IsNullOrEmpty(model.form.UserCode))
            {
                model.user = new sys_userService().GetModel(ParamQuery.Instance().AndWhere("UserCode", model.form.UserCode));
            }
            //获取答复信息
            var pQuery = ParamQuery.Instance().Select("A.*, L.LawyerName, L.LawyerCode,L.ContactNO,L.LawyerPic")
                .From("msg_advisoryAnswer A left outer join law_lawyer L on A.LawyerCode=L.LawyerCode")
                .AndWhere("AdvisoryCode", id)
                .AndWhere("ApproveState", "passed");
            model.answers = new msg_advisoryAnswerService().GetDynamicList(pQuery);
            //已解决咨询
            ParamQuery pQuery1 = ParamQuery.Instance().Select("A.*, C.ClassName")
                .From("msg_advisory as A left outer join msg_class C on A.MsgClass=C.ClassCode")
                .AndWhere("MsgClass", model.form.MsgClass);
            pQuery1.AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { }).OrderBy("ApproveDate Desc").Paging(1, 5);
            model.zxOver = new msg_advisoryAnswerService().GetDynamicList(pQuery1);
            //律师回复
            //model.page.form = new
            //{
            //    defaults = new msg_advisoryAnswer().Extend(new { AnswerCode = "", AdvisoryCode = "", AnswerContent = "", IsSecret = "", AnswerDate = "", PraiseCount = "", AnswerOrder = "" }),
            //    primaryKeys = new string[] { "AnswerCode" }
            //};
            //页面SEO配置
            ViewBag.Title = model.form.MsgTitle + "-阳光问法-河北新闻网";
            if (!string.IsNullOrEmpty(model.form.Keyword))
                ViewBag.Keywords = model.form.Keyword;

            return View(model);
        }

        public ActionResult list(RequestWrapper query)
        {
            //页面SEO配置
            ViewBag.Title = "法律咨询-阳光问法-河北新闻网";

            dynamic model = new ExpandoObject();
            model.page = new ExpandoObject();
            model.page.urls = new { query = "/wap/list" };
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AdvisoryCode desc'>
    <select>A.*, C.ClassName</select>
    <from>msg_advisory as A 
        left outer join msg_class C on A.MsgClass=C.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AdvisoryCode'	cp='equal'  variable='ac'></field>   
        <field name='MsgTitle'		cp='like'   variable='mt'></field>   
        <field name='MsgClass'		cp='childalias' extend='msg_class'></field>   
        <field name='MsgStatus'		cp='equal'></field>   
        <field name='MsgStatus'		cp='greaterequal'        variable='MsgStatusGreater'></field>   
        <field name='MsgInDate'		cp='daterange'  variable='id'></field>  
        <field name='LawyerCode'	cp='equal'></field>  
        <field name='MsgClass'      cp='mapalias'       extend='ClassCode,law_lawyerClassMap,LawyerCode'    variable='Lawyer'   ignoreEmpty='true'></field> 
    </where>
</settings>");
            var service = new msg_advisoryService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { });
            var result = service.GetDynamicListWithPaging(pQuery);

            model.rows = result.rows;
            model.page.dataSource = new
            {
                total = result.total,
                msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
            };
            model.page.pagination = new
            {
                page = query["page"] ?? "1",
                rows = query["rows"] ?? "20",
            };
            model.page.form = new
            {
                AdvisoryCode = query["AdvisoryCode"],
                MsgTitle = query["MsgTitle"],
                MsgClass = query["MsgClass"],
                MsgStatus = query["MsgStatus"],
                MsgInDate = query["MsgInDate"]
            };
            return View(model);
        }

        public ActionResult Query()
        {
            return View();
        }

        public ActionResult wyzx(string id)
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Advisory"),
                resx = MsgHelper.GetEditResx("咨询"),
                dataSource = new
                {
                    pageData = new Zephyr.Areas.Msg.Controllers.AdvisoryApiController().GetEditMaster("-1"),
                    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    defaults = new msg_advisory().Extend(new { AdvisoryCode = "", MsgTitle = "", MsgContent = "", LawyerCode = id, Address = "", DetailAddress = "", Keyword = "", MsgClass = "", IsOpen = "true", ContactNO = "", PhoneIsOpen = "", Email = "", MsgIP = "", IgnoreCode = "", MsgStatus = "", HitCount = "", MsgInDate = "", MsgRemark = "", UserCode = "" }),
                    primaryKeys = new string[] { "AdvisoryCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/zx/edit";

            return View(model);
        }

        public ActionResult Firm()
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

        public ActionResult FirmDetail(string id)
        {
            dynamic Model = new ExpandoObject();
            Model.FirmClasses = new law_firmService().GetFirmClass(id);
            Model.Lawyers = new law_firmService().GetLawyers(id);
            Model.Form = new law_firmService().GetModel(ParamQuery.Instance().AndWhere("FirmCode", id));

            return View(Model);
        }

        public ActionResult Lawyer(RequestWrapper query)
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
                    LawyerCode = "",
                    LawyerName = "",
                    LawyerPic = "",
                    ContactNO = "",
                    MobilePhone = "",
                    ProfessionalTitle = "",
                    Address = "",
                    LawyerDescription = "",
                    LawyerRemark = "",
                    ClassCode = query["MsgClass"] ?? ""
                },
                idField = "LawyerCode"
            };

            return View(model);
        }

        public ActionResult LawyerDetail(string id)
        {
            dynamic Model = new ExpandoObject();
            Model.Lawyer = new law_lawyerService().GetLawyer(id);
            Model.Advisorys = new law_lawyerService().GetAdvisorys(id);
            return View(Model);
        }
    }
}
