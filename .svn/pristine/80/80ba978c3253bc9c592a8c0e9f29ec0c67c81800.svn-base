using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Areas.Msg;
using Zephyr.Areas.Msg.Controllers;
using Zephyr.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Zephyr.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    [MvcMobileFilter(true)]
    public class ZXController : Controller
    {
        public ActionResult Index(RequestWrapper query)
        {
            //页面SEO配置
            ViewBag.Title = "法律咨询-阳光问法-河北新闻网";

            dynamic model = new ExpandoObject();
            model.page = new ExpandoObject();
            model.page.urls = new { query = "/zx" };
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AdvisoryCode desc'>
    <select>A.*, C.ClassName</select>
    <from>msg_advisory as A 
        left outer join msg_class C on A.MsgClass=C.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AdvisoryCode'	cp='equal'></field>   
        <field name='MsgTitle'		cp='like'></field>   
        <field name='MsgClass'		cp='childalias' extend='msg_class'></field>   
        <field name='MsgStatus'		cp='equal'></field>   
        <field name='MsgStatus'		cp='greaterequal'        variable='MsgStatusGreater'></field>   
        <field name='MsgInDate'		cp='daterange'></field>  
        <field name='LawyerCode'	cp='equal'></field>  
        <field name='MsgClass'      cp='mapalias'       extend='ClassCode,law_lawyerClassMap,LawyerCode'    variable='Lawyer'   ignoreEmpty='true'></field> 
    </where>
</settings>");
            query["page"] = string.IsNullOrEmpty(query["page"]) ? "1" : query["page"];//初始化分页配置
            query["rows"] = string.IsNullOrEmpty(query["rows"]) ? "20" : query["rows"];
            var service = new msg_advisoryService();
            var pQuery = query.ToParamQuery(); 
            pQuery.AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { });
            var result = service.GetDynamicListWithPaging(pQuery);

            model.rows = result.rows;
            model.page.dataSource = new {
                total =  result.total,
                msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
            };
            model.page.pagination = new
            {
                page = query["page"]??"1",
                rows = query["rows"] ?? "20",
            };
            model.page.form = new
            {
                AdvisoryCode = query["AdvisoryCode"],
                MsgTitle = query["MsgTitle"],
                MsgClass = query["MsgClass"],
                MsgStatus = query["MsgStatus"],
                MsgStatusGreater = query["MsgStatusGreater"],
                MsgInDate = query["MsgInDate"]
            };
            return View(model);
        }
        public ActionResult show(string id)
        {
            dynamic model = new ExpandoObject();
            model.page = new
            {
                urls = MsgHelper.GetEditUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetEditResx("咨询答复"),
                dataSource = new
                {
                    pageData = new AdvisoryAnswerApiController().GetEditMaster("-1"),
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
            model.form = new msg_advisoryService().GetModel(ParamQuery.Instance().AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[]{}).AndWhere("AdvisoryCode", id));
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
                .From("msg_advisoryAnswer A left outer join v_lawyerScore L on A.LawyerCode=L.LawyerCode")
                .AndWhere("AdvisoryCode", id)
                .AndWhere("ApproveState", "passed");
            model.answers = new msg_advisoryAnswerService().GetDynamicList(pQuery);
            //已解决咨询
            ParamQuery pQuery1 = ParamQuery.Instance().Select("A.*, C.ClassName")
                .From("msg_advisory as A left outer join msg_class C on A.MsgClass=C.ClassCode")
                .AndWhere("MsgClass",model.form.MsgClass);
            pQuery1.AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { }).OrderBy("ApproveDate Desc").Paging(1,5);
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
        public ActionResult wyzx(string id)
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Advisory"),
                resx = MsgHelper.GetEditResx("咨询"),
                dataSource = new
                {
                    pageData = new AdvisoryApiController().GetEditMaster("-1"),
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
        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Comment(string id, RequestWrapper query)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Error", "Home");
            dynamic model = new ExpandoObject();
            model.page = new ExpandoObject();
            model.page.urls = new { query = "/zx/comment/"+id };
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='CommentOrder desc, CommentCode'>
    <select>*</select>
    <from>msg_comment</from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true' >
    </where>
</settings>");
            var service = new msg_advisoryService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("MasterKey", "msg_advisory").AndWhere("MasterValue", id).AndWhere("ApproveState", "passed");
            var result = service.GetDynamicListWithPaging(pQuery);

            model.rows = result.rows;
            model.page.dataSource = new
            {
                total = result.total,
                masterValue = id,
                masterKey = "msg_advisory"
                //msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
            };
            model.page.pagination = new
            {
                page = query["page"] ?? "1",
                rows = query["rows"] ?? "20",
            };
            model.page.form = new
            {
                //MasterValue = query["MasterValue"],
            };
            return View(model);
        }
    }

    public class ZXApiController : ApiController
    {
        [System.Web.Http.AllowAnonymous]
        [MvcMenuFilter(false)]
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AdvisoryCode desc'>
    <select>A.*, C.ClassName</select>
    <from>msg_advisory as A 
        left outer join msg_class C on A.MsgClass=C.ClassCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AdvisoryCode'	cp='equal'></field>   
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
            return result;
        }

        // 保存 POST api/mms/send
        [System.Web.Http.AllowAnonymous]
        [MvcMenuFilter(false)]
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", typeof(msg_advisory).Name, "AdvisoryCode");

            var tabsWrapper = new List<RequestWrapper>();
            var service = new msg_advisoryService();
            var advisoryCode = service.GetNewKey("AdvisoryCode", "dateplus");
            data.form.AdvisoryCode = advisoryCode;
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        [System.Web.Http.HttpPost]
        public dynamic PostAnswer(dynamic data)
        {
            if(!FormsAuth.GetUserData().Roles.Contains("Lawyer"))
                return  new { status = "role", message = "只有律师才能回复！" };
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        msg_advisoryAnswer
    </table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>");
            var tabsWrapper = new List<RequestWrapper>();

            var service = new msg_advisoryAnswerService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);

            return new { status = "success", message = "网友衷心感谢您的答复..." };
        }

        /// <summary>
        /// 热门资讯
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        [MvcMenuFilter(false)]
        public List<dynamic> GetRMZX(RequestWrapper query)
        {
            var pQuery = ParamQuery.Instance().Select("AdvisoryCode, MsgTitle").AndWhere("MsgStatus", 5, Cp.GreaterEqual, new object[] { }).OrderBy("HitCount desc").Paging(1, 7);
            var result = new msg_advisoryService().GetDynamicList(pQuery);
            return result;
        }

        #region 自动完成
        // GET api/msg/advisory/getadvisoryname
        public virtual List<dynamic> GetAdvisoryName(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 MsgTitle_Show").AndWhere("MsgTitle_Show", q, Cp.Like);
            var service = new msg_advisoryService();
            return service.GetDynamicList(pQuery);
        }
        // GET api/msg/advisory/getadvisorycode
        public virtual List<dynamic> GetAdvisoryCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 AdvisoryCode").AndWhere("AdvisoryCode", q, Cp.Like);
            var service = new msg_advisoryService();
            return service.GetDynamicList(pQuery);
        }
        #endregion
    }


}
