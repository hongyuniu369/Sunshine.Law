
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using System.Linq;

namespace Zephyr.Areas.Msg.Controllers
{
    public class EntrustAnswerController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("EntrustAnswer"),
                resx = MsgHelper.GetIndexResx("接洽"),
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    AnswerCode = "",
                    EntrustCode = "",
                    LawyerCode = "",
                    ApproveState = "",
                    IsBestAnswer = "",
                    AnswerDate = ""
                },
                idField = "AnswerCode"
            };

            return View(model);
        }

        /// <summary>
        /// 律师：我的答复
        /// </summary>
        /// <returns></returns>
        public ActionResult Lawyer()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("EntrustAnswer"),
                resx = MsgHelper.GetIndexResx("接洽"),
                dataSource = new
                {
                    extend = new { IsLawyer = true }
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    AnswerCode = "",
                    EntrustCode = "",
                    LawyerCode = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode,
                    ApproveState = "",
                    IsBestAnswer = "",
                    AnswerDate = ""
                },
                idField = "AnswerCode"
            };

            return View("index", model);
        }

        /// <summary>
        /// 律师组长：答复择优
        /// </summary>
        /// <returns></returns>
        public ActionResult Best()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("EntrustAnswer"),
                resx = MsgHelper.GetIndexResx("委托答复"),
                dataSource = new
                {
                    extend = new { IsLawyerGrouper = true }
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    AnswerCode = "",
                    EntrustCode = "",
                    LawyerCode = "",
                    ApproveState = "passed",
                    IsBestAnswer = "",
                    AnswerDate = ""
                },
                idField = "AnswerCode"
            };

            return View("index", model);
        }

        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = MsgHelper.GetEditUrls("EntrustAnswer"),
                resx = MsgHelper.GetEditResx("委托答复"),
                dataSource = new
                {
                    pageData = new EntrustAnswerApiController().GetEditMaster(id),
                    dsAudit = Common.MsgData.Audit.Select(s => new { value = s.Key, text = s.Value }),
                    dsBit = Common.MsgData.Bit.Select(s => new { value = s.Key, text = s.Value })
                },
                form = new
                {
                    defaults = new msg_entrustAnswer().Extend(new { AnswerCode = "", EntrustCode = "", AnswerContent = "", IsSecret = "", AnswerDate = "", PraiseCount = "", AnswerOrder = "" }),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "form",
                      primaryKeys = new string[]{"AnswerCode"},
                      defaults = new {ApproveState = "",ApprovePerson = "",ApproveDate = "",ApproveRemark = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"LawyerCode"},
                      defaults = new {LawyerName = "",LicenseNumber = "",ContactNO = "",MobilePhone = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"AnswerCode"},
                      defaults = new {IsBestAnswer = "",SelectDate = "",SelectUserCode = ""}
                    }    
                }
            };
            return View(model);
        }
    }

    public class EntrustAnswerApiController : MsgBaseApi<msg_entrustAnswer, msg_entrustAnswerService>
    {

        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AnswerCode desc'>
    <select>*</select>
    <from>msg_entrustAnswer</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AnswerCode'		cp='equal'></field>   
        <field name='EntrustCode'		cp='equal'></field>   
        <field name='LawyerCode'		cp='equal'></field>   
        <field name='ISChecked'		cp='equal'></field>   
        <field name='IsBestAnswer'		cp='equal'></field>   
        <field name='AnswerDate'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new msg_entrustAnswerService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public IEnumerable<dynamic> GetByEntrustCode(string id)
        {
            var pQuery = ParamQuery.Instance().Select("A.*, case when A.IsBestAnswer = 1 then 1 else 0 end as Selected")
                .From(@"msg_entrustAnswer as A")
                .AndWhere(@"EntrustCode", id)
                .AndWhere("ApproveState", "passed")
                .OrderBy("A.AnswerOrder");
            var result = masterService.GetDynamicList(pQuery);
            return result;
        }

        public override dynamic GetEditMaster(string id)
        {
            var masterService = new msg_entrustAnswerService();
            var pQuery = ParamQuery.Instance().AndWhere("AnswerCode", id);
            var masterModel = masterService.GetModel(pQuery);

            var result = new
            {
                //主表数据
                form = masterModel,
                scrollKeys = masterService.ScrollKeys("AnswerCode", id),

                //明细数据
                //tab0 = new msg_entrustAnswerService().GetModel(pQuery),
                tab1 = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("LawyerCode", masterModel == null ? "" : masterModel.LawyerCode)),
                //tab2 = new msg_entrustAnswerService().GetModel(pQuery)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("msg_entrustAnswer")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("AnswerCode", id);

            var service = new msg_entrustAnswerService();
            var rowsAffected = service.Update(pUpdate);
            //评分处理
            if (rowsAffected > 0)
            {
                var entrustAnswer = masterService.GetModel(ParamQuery.Instance().AndWhere("AnswerCode", id));
                var score = new sys_parameterService().GetModel(ParamQuery.Instance().AndWhere("ParamCode", "Score_Answer"));
                if (data["status"].ToString().ToLower() == "passed")
                    new law_lawyerScoreService().EditScore(entrustAnswer.LawyerCode, "msg_entrustAnswer", id, "AnswerScore", Convert.ToDecimal(score.ParamValue));
                else
                {
                    law_lawyerScoreService s = new law_lawyerScoreService();
                    var scoreCode = s.GetScoreCode("msg_entrustAnswer", id);
                    s.Delete(ParamDelete.Instance().AndWhere("ScoreCode", scoreCode));
                }
            }
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "审核失败，请重试或联系管理员！", id);
        }

        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存数据
        [System.Web.Http.HttpPost]
        public override void Edit(dynamic data)
        {
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
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>msg_entrustAnswer</table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>law_lawyer</table>
    <where>
        <field name='LawyerCode' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>msg_entrustAnswer</table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>"));

            var service = new msg_entrustAnswerService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        /// <summary>
        /// 最佳答复
        /// </summary>
        /// <param name="id"></param>
        public void Best(string id)
        {
            masterService.Best(id);
        }

        #region 自动完成
        // GET api/msg/entrustanswer/getentrustAnswercode
        public virtual List<dynamic> GetEntrustAnswerCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 AnswerCode").AndWhere("AnswerCode", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion
    }
}
