using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Msg;
using Zephyr.Areas.Msg.Controllers;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Controllers
{
    
    [MvcMenuFilter(false)]
    public class WTController : Controller
    {

        public ActionResult wywt()
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Entrust"),
                resx = MsgHelper.GetEditResx("委托"),
                dataSource = new
                {
                    pageData = "",
                    msgStatus = "",
                    involvedMoney = new sys_codeService().GetKeyTextListByType("InvolvedMoney")
                },
                form = new
                {
                    defaults = new msg_entrust().Extend(new { EntrustCode = "", MsgTitle = "", MsgContent = "", LawyerCode = "", Address = "", DetailAddress = "", Keyword = "", MsgClass = "", ContactNO = "", Email = "", MsgIP = "", MsgStatus = "", HitCount = "", MsgInDate = "", MsgRemark = "", UserCode = "" }),
                    primaryKeys = new string[] { "EntrustCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/wt/edit";

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index(RequestWrapper query)
        {
            dynamic model = new ExpandoObject();
            model.page = new ExpandoObject();
            model.page.urls = new { query = "/wt" };
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='EntrustCode desc'>
    <select>A.*, C.ClassName</select>
    <from>msg_entrust as A 
        left outer join msg_class C on A.MsgClass=C.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EntrustCode'	cp='equal'></field>   
        <field name='MsgTitle'		cp='like'></field>   
        <field name='MsgClass'		cp='equal'></field>   
        <field name='MsgStatus'		cp='equal'></field>   
        <field name='MsgStatus'		cp='greaterequal'        variable='MsgStatusGreater'></field>   
        <field name='MsgInDate'		cp='daterange'></field>  
        <field name='LawyerCode'	cp='equal'></field>  
        <field name='MsgClass'      cp='mapalias'       extend='ClassCode,law_lawyerClassMap,LawyerCode'    variable='Lawyer'   ignoreEmpty='true'></field> 
    </where>
</settings>");
            var service = new msg_advisoryService();
            var pQuery = query.ToParamQuery();
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
                EntrustCode = query["EntrustCode"],
                MsgTitle = query["MsgTitle"],
                MsgClass = query["MsgClass"],
                MsgStatus = query["MsgStatus"],
                MsgInDate = query["MsgInDate"]
            };
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult show(string id)
        {
            dynamic model = new ExpandoObject();
            //model.page = new ExpandoObject();
            //model.page.dataSource = new
            //{
            //    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
            //};
            model.page = new
            {
                urls = MsgHelper.GetEditUrls("EntrustAnswer"),
                resx = MsgHelper.GetEditResx("委托答复"),
                dataSource = new
                {
                    pageData = new EntrustAnswerApiController().GetEditMaster("-1"),
                    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.EntrustStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    defaults = new msg_entrustAnswer(),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.page.form.defaults.EntrustCode = id;
            model.page.urls.edit = "/api/wt/PostAnswer";
            model.page.resx.editSuccess = "答复已提交，感谢您的热心帮助。";
            //获取资讯信息
            model.form = new msg_entrustService().GetModel(ParamQuery.Instance().AndWhere("EntrustCode", id));
            model.msgStatus = Enum.GetName(typeof(Zephyr.Areas.Msg.Common.MsgEnum.EntrustStatus), Convert.ToInt32(model.form.MsgStatus));
            model.user = new sys_user();
            if (!string.IsNullOrEmpty(model.form.UserCode))
            {
                model.user = new sys_userService().GetModel(ParamQuery.Instance().AndWhere("UserCode", model.form.UserCode));
            }

            model.lawyerLoaded = "";
            if (!String.IsNullOrEmpty(MsgHelper.GetUserCode()))
            {
                if (FormsAuth.GetUserData().Roles.Contains("Lawyer")) {
                    model.lawyerLoaded = "已登录";
                }                
            }
            //获取答复信息
            var pQuery = ParamQuery.Instance().Select("A.*, L.LawyerName,L.ProfessionalTitle,L.LawyerCode,L.ContactNO,L.LawyerPic")
                .From("msg_entrustAnswer A left outer join law_lawyer L on A.LawyerCode=L.LawyerCode")
                .AndWhere("EntrustCode", id)
                .AndWhere("ApproveState", "passed");
            model.answers = new msg_entrustAnswerService().GetDynamicList(pQuery);
            //律师回复
            //model.page.form = new
            //{
            //    defaults = new msg_advisoryAnswer().Extend(new { AnswerCode = "", AdvisoryCode = "", AnswerContent = "", IsSecret = "", AnswerDate = "", PraiseCount = "", AnswerOrder = "" }),
            //    primaryKeys = new string[] { "AnswerCode" }
            //};



            return View(model);
        }
    }

    
    public class WTApiController : ApiController
    {
        [System.Web.Http.AllowAnonymous]
        [MvcMenuFilter(false)]
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='EntrustCode desc'>
    <select>A.*, C.ClassName</select>
    <from>msg_entrust as A 
        left outer join msg_class C on A.MsgClass=C.ClassCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EntrustCode'	cp='equal'></field>   
        <field name='MsgTitle'		cp='like'></field>   
        <field name='MsgClass'		cp='equal'></field>   
        <field name='MsgStatus'		cp='equal'></field>   
        <field name='MsgStatus'		cp='greaterequal'        variable='MsgStatusGreater'></field>   
        <field name='MsgInDate'		cp='daterange'></field>  
        <field name='LawyerCode'	cp='equal'></field>  
        <field name='MsgClass'      cp='mapalias'       extend='ClassCode,law_lawyerClassMap,LawyerCode'    variable='Lawyer'   ignoreEmpty='true'></field> 
    </where>
</settings>");
            var service = new msg_entrustService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { });
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        // 保存 POST api/mms/send
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", typeof(msg_entrust).Name, "EntrustCode");

            var tabsWrapper = new List<RequestWrapper>();
            var service = new msg_entrustService();
            var entrustCode = service.GetNewKey("EntrustCode", "dateplus");
            data.form.EntrustCode = entrustCode;
            data.form.UserCode = MsgHelper.GetUserCode();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
        /// <summary>
        /// 添加律师回复
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public dynamic PostAnswer(dynamic data)
        {
            if (!FormsAuth.GetUserData().Roles.Contains("Lawyer"))
                return new { status = "role", message = "只有律师才能回复！" };
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        msg_entrustAnswer
    </table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>");
            var tabsWrapper = new List<RequestWrapper>();
            var service = new msg_entrustAnswerService();
            var AnswerCode = service.GetNewKey("AnswerCode", "dateplus");
            data.form.AnswerCode = AnswerCode;
            
            var result = service.EditPage(data, formWrapper, tabsWrapper);

            return new { status = "success", message = "网友衷心感谢您的答复..." };
        }
    }
}
