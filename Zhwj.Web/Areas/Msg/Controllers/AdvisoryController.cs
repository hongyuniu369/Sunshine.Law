
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Zephyr.Areas.Msg.Controllers
{
    public class AdvisoryController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new{
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new{
                    AdvisoryCode = "" ,
                    MsgTitle = "" ,
                    MsgClass = "" ,
                    MsgStatus = "" ,
                    MsgInDate = "" 
                },
                idField="AdvisoryCode"
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
            model.form = new msg_advisoryService().GetModel(ParamQuery.Instance().AndWhere("AdvisoryCode", id));
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
        public ActionResult NewSubmit()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "0",
                    MsgInDate = ""
                },
                idField = "AdvisoryCode"
            };

            return View("AdminList", model);
        }
        public ActionResult Checked()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "5",
                    MsgInDate = ""
                },
                idField = "AdvisoryCode"
            };

            return View("AdminList", model);
        }
        public ActionResult Replied()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "30",
                    MsgInDate = ""
                },
                idField = "AdvisoryCode"
            };

            return View("AdminList", model);
        }
        public ActionResult Overed()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "99",
                    MsgInDate = ""
                },
                idField = "AdvisoryCode"
            };

            return View("AdminList", model);
        }
        public ActionResult Ignored()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "-9",
                    MsgInDate = ""
                },
                idField = "AdvisoryCode"
            };

            return View("AdminList", model);
        }
        public ActionResult Edit(string id = "")
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Advisory"),
                resx = MsgHelper.GetEditResx("咨询"),
                dataSource = new
                {
                    pageData = new AdvisoryApiController().GetEditMaster(id),
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    defaults = new msg_advisory().Extend(new { AdvisoryCode = id, MsgTitle = "", MsgContent = "", LawyerCode = "", Address = "", DetailAddress = "", Keyword = "", MsgClass = "", IsOpen = "", ContactNO = "", PhoneIsOpen = "", Email = "", MsgIP = "", IgnoreCode = "", MsgStatus = "", HitCount = "", MsgInDate = "", MsgRemark = "", UserCode = "" }),
                    primaryKeys = new string[] { "AdvisoryCode" }
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
        /// <summary>
        /// 律师咨询管理:我的咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult Lawyer()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Where(s => (int)s >= 5).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "",
                    MsgStatusGreater = "5",
                    MsgInDate = "",
                    //查询该律师关联的留言类型的留言
                    Lawyer = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode
                },
                idField = "AdvisoryCode"
            };
            model.urls.edit = "/Msg/advisory/answer/";

            return View(model);
        }
        /// <summary>
        /// 律师咨询管理：咨询我的
        /// </summary>
        /// <returns></returns>
        public ActionResult Mine()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Where(s => (int)s >= 5).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "",
                    MsgStatusGreater = "5",
                    MsgInDate = "",
                    //查询咨询该律师的留言
                    LawyerCode  = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode,
                },
                idField = "AdvisoryCode"
            };
            model.urls.edit = "/Msg/advisory/answer/";

            return View("Lawyer", model);
        }
        /// <summary>
        /// 律师回复
        /// </summary>
        /// <returns></returns>
        public ActionResult Answer(string id = "")
        {
            var advisory = new msg_advisoryService().GetModel(ParamQuery.Instance().AndWhere("AdvisoryCode", id));
            var className = new msg_classService().GetModel(ParamQuery.Instance().AndWhere("ClassCode", advisory.MsgClass)).ClassName;
            
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Advisory"),
                resx = MsgHelper.GetEditResx("咨询"),
                dataSource = new
                {
                    pageData = new AdvisoryAnswerApiController().GetEditMaster("-1"),
                    dsBit = Common.MsgData.Bit.Select(s => new { value = s.Key, text = s.Value }),
                    advisory = advisory.Extend(new { ClassName=className})
                },
                form = new
                {
                    defaults = new msg_advisoryAnswer().Extend(new { AnswerCode = "", AdvisoryCode = id, AnswerContent = "", IsSecret = false, AnswerDate = DateTime.Now.ToString(), PraiseCount = 0, AnswerOrder = 99 }),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/Msg/advisoryanswer/edit/";
            return View(model);
        }
        /// <summary>
        /// 律师组长:答复择优
        /// </summary>
        /// <returns></returns>
        public ActionResult Best()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Advisory"),
                resx = MsgHelper.GetIndexResx("咨询"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Common.MsgEnum.AdvisoryStatus))).Where(s => (int)s >= 5).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "",
                    MsgStatusGreater = "5",
                    MsgInDate = "",
                    //查询该律师关联的留言类型的留言
                    Lawyer = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode
                },
                idField = "AdvisoryCode"
            };
            model.urls.edit = "/Msg/advisory/answer/";

            return View("lawyer", model);
        }
    }

    public class AdvisoryApiController : MsgBaseApi<msg_advisory, msg_advisoryService>
    {
        public override dynamic GetEditMaster(string id)
        {
            dynamic extend = new ExpandoObject();
            var form = masterService.GetModel(ParamQuery.Instance().AndWhere("AdvisoryCode", id));
            if (form != null && !string.IsNullOrEmpty(form.IgnoreCode))
            {
                var ignore = new msg_ignoreService().GetModel(ParamQuery.Instance().AndWhere("IgnoreCode", form.IgnoreCode, Cp.Equal, new object[]{}));
                if (ignore != null)
                    extend.IgnoreReason = ignore.IgnoreReason;
            }
            return new
            {
                form = form,
                extend = extend,
                scrollKeys = masterService.ScrollKeys("AdvisoryCode", id, ParamQuery.Instance())
            };
        }
        
        public override dynamic Get(RequestWrapper query)
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

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("msg_advisory")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("AdvisoryCode", id);

            var service = new msg_advisoryService();
            var rowsAffected = service.Update(pUpdate);
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "审核失败[咨询编号={0}]，请重试或联系管理员！", id);
        }

        #region 自动完成
        // GET api/msg/advisory/getadvisoryname
        public virtual List<dynamic> GetAdvisoryName(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 MsgTitle_Show").AndWhere("MsgTitle_Show", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        // GET api/msg/advisory/getadvisorycode
        public virtual List<dynamic> GetAdvisoryCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 AdvisoryCode").AndWhere("AdvisoryCode", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion
    }
}
