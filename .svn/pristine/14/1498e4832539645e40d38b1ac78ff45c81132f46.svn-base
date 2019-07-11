
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using Zephyr.Areas.Msg.Common;

namespace Zephyr.Areas.Msg.Controllers
{
    public class EntrustController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Entrust"),
                resx = MsgHelper.GetIndexResx("案件委托"),
                dataSource = new{
                    msgStatus = ((IList<Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Common.MsgEnum.EntrustStatus))).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new{
                    EntrustCode = "" ,
                    MsgTitle = "" ,
                    MsgClass = "" ,
                    MsgStatus = "" ,
                    MsgInDate = "" 
                },
                idField="EntrustCode"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = MsgHelper.GetEditUrls("Entrust"),
                resx = MsgHelper.GetEditResx("案件委托"),
                dataSource = new
                {
                    pageData = new EntrustApiController().GetEditMaster(id),
                    msgStatus = ((IList<Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Common.MsgEnum.EntrustStatus))).Select(s => new { value = ((int)s), text = s.ToString() }),
                    involvedMoney = new sys_codeService().GetKeyTextListByType("InvolvedMoney")
                },
                form = new
                {
                    defaults = new msg_entrust().Extend(new { EntrustCode = id, MsgTitle = "", MsgSubdescription = "", MsgContent = "", MsgRequirement = "", InvolvedMoney = "", ActiveTime = "", Address = "", DetailAddress = "", TrueName = "", MsgClass = "", Keyword = "", IsInternalReference = "", ContactNO = "", Email = "", MsgIP = "", IgnoreCode = "", MsgStatus = "", HitCount = "", IsPublicity = "", IsClassic = "", MsgInDate = "", ApproveState = "", ApprovePerson = "", ApproveDate = "", ApproveRemark = "", MsgCheckDate = "", ReferenceSource = "", MsgRemark = "", UserCode = "", LawyerCode = "", AgreeTime = "" }),
                    primaryKeys = new string[] { "EntrustCode" }
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
        /// 律师委托管理:我的委托
        /// </summary>
        /// <returns></returns>
        public ActionResult Lawyer()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Entrust"),
                resx = MsgHelper.GetIndexResx("委托"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Common.MsgEnum.EntrustStatus))).Where(s => (int)s >= 5).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    EntrustCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "",
                    MsgStatusGreater = "5",
                    MsgInDate = "",
                    //查询该律师关联的留言类型的留言
                    Lawyer = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode
                },
                idField = "EntrustCode"
            };
            model.urls.edit = "/Msg/entrust/answer/";

            return View(model);
        }
        /// <summary>
        /// 律师委托管理：委托我的
        /// </summary>
        /// <returns></returns>
        public ActionResult Mine()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("Entrust"),
                resx = MsgHelper.GetIndexResx("委托"),
                dataSource = new
                {
                    msgStatus = ((IList<Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Common.MsgEnum.EntrustStatus))).Where(s => (int)s >= 5).Select(s => new { value = ((int)s), text = s.ToString() })
                },
                form = new
                {
                    EntrustCode = "",
                    MsgTitle = "",
                    MsgClass = "",
                    MsgStatus = "",
                    MsgStatusGreater = "5",
                    MsgInDate = "",
                    //查询委托该律师的留言
                    LawyerCode = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode,
                },
                idField = "EntrustCode"
            };
            model.urls.edit = "/Msg/entrust/answer/";

            return View("Lawyer", model);
        }
        /// <summary>
        /// 律师回复
        /// </summary>
        /// <returns></returns>
        public ActionResult Answer(string id = "")
        {
            var entrust = new msg_entrustService().GetModel(ParamQuery.Instance().AndWhere("EntrustCode", id));
            var className = new msg_classService().GetModel(ParamQuery.Instance().AndWhere("ClassCode", entrust.MsgClass)).ClassName;

            var model = new
            {
                urls = MsgHelper.GetEditUrls("Entrust"),
                resx = MsgHelper.GetEditResx("委托"),
                dataSource = new
                {
                    pageData = new EntrustAnswerApiController().GetEditMaster("-1"),
                    dsBit = Common.MsgData.Bit.Select(s => new { value = s.Key, text = s.Value }),
                    entrust = entrust.Extend(new { ClassName = className })
                },
                form = new
                {
                    defaults = new msg_entrustAnswer().Extend(new { AnswerCode = "", EntrustCode = id, AnswerContent = "", IsSecret = false, AnswerDate = DateTime.Now.ToString(), PraiseCount = 0, AnswerOrder = 99 }),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/Msg/entrustanswer/edit/";
            return View(model);
        }
    }

    public class EntrustApiController : MsgBaseApi<msg_entrust, msg_entrustService>
    {
        public override dynamic GetEditMaster(string id)
        {
            dynamic extend = new ExpandoObject();
            var form = masterService.GetModel(ParamQuery.Instance().AndWhere("EntrustCode", id));
            if (form != null && !string.IsNullOrEmpty(form.IgnoreCode))
            {
                var ignore = new msg_ignoreService().GetModel(ParamQuery.Instance().AndWhere("IgnoreCode", form.IgnoreCode, Cp.Equal, new object[] { }));
                if (ignore != null)
                    extend.IgnoreReason = ignore.IgnoreReason;
            }
            return new
            {
                form = form,
                extend = extend,
                scrollKeys = masterService.ScrollKeys("EntrustCode", id, ParamQuery.Instance())
            };
        }
        
        public override dynamic Get(RequestWrapper query)
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
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        /// <summary>
        /// 网友回复后律师审核，确定接受委托或拒绝委托。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            //var pUpdate = ParamUpdate.Instance()
            //    .Update("msg_entrust")
            //    .Column("ApproveState", data["status"])
            //    .Column("ApproveRemark", data["comment"])
            //    .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
            //    .Column("ApproveDate", DateTime.Now)
            //    .AndWhere("EntrustCode", id);

            var pUpdate = ParamUpdate.Instance()
                .Update("msg_entrust")
                .Column("ApproveState", data["status"])
                .Column("MsgStatus", (data["status"].ToString() == "passed") ? 99 : 30)
                .Column("ApproveRemark", data["comment"])
                .Column("AgreeTime", DateTime.Now)
                .AndWhere("EntrustCode", id);

            var service = new msg_entrustService();
            var rowsAffected = service.Update(pUpdate);
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }

        #region 自动完成
        // GET api/msg/Entrust/getEntrustname
        public virtual List<dynamic> GetEntrustName(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 MsgTitle_Show").AndWhere("MsgTitle_Show", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        // GET api/msg/Entrust/getEntrustcode
        public virtual List<dynamic> GetEntrustCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 EntrustCode").AndWhere("EntrustCode", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion
    }
}
