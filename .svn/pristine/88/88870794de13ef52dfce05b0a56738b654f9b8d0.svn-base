
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
    public class AdvisoryAnswerController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetIndexResx("咨询答复"),
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    AnswerCode = "" ,
                    AdvisoryCode = "" ,
                    LawyerCode = "" ,
                    ApproveState = "" ,
                    IsBestAnswer = "" ,
                    AnswerDate = "" 
                },
                idField="AnswerCode"
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
                urls = MsgHelper.GetIndexUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetIndexResx("咨询答复"),
                dataSource = new
                {
                    extend= new {IsLawyer = true}
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    AnswerCode = "",
                    AdvisoryCode = "",
                    LawyerCode = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).LawyerCode,
                    ApproveState = "",
                    IsBestAnswer = "",
                    AnswerDate = ""
                },
                idField = "AnswerCode"
            };

            return View("index",model);
        }

        /// <summary>
        /// 律师组长：答复择优
        /// </summary>
        /// <returns></returns>
        public ActionResult Best()
        {
            var model = new
            {
                urls = MsgHelper.GetIndexUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetIndexResx("咨询答复"),
                dataSource = new
                {
                    extend = new { IsLawyerGrouper = true }
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    AnswerCode = "",
                    AdvisoryCode = "",
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
                urls = MsgHelper.GetEditUrls("AdvisoryAnswer"),
                resx = MsgHelper.GetEditResx("咨询答复"),
                dataSource = new
                {
                    pageData = new AdvisoryAnswerApiController().GetEditMaster(id),
                    dsAudit = Common.MsgData.Audit.Select(s => new { value = s.Key, text = s.Value }),
                    dsBit = Common.MsgData.Bit.Select(s => new { value = s.Key, text = s.Value })
                },
                form = new
                {
                    defaults = new msg_advisoryAnswer().Extend(new { AnswerCode = "", AdvisoryCode = "", AnswerContent = "", IsSecret = "", AnswerDate = "", PraiseCount = "", AnswerOrder = "" }),
                    primaryKeys = new string[] { "AnswerCode" }
                },
                tabs = new object[]{
                    new{
                      type = "form",
                      primaryKeys = new string[]{"AdvisoryCode"},
                      defaults = new {AdvisoryCode="", MsgTitle="", MsgContent=""}
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

    public class AdvisoryAnswerApiController : MsgBaseApi<msg_advisoryAnswer, msg_advisoryAnswerService>
    {
        
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AnswerCode desc'>
    <select>msg_advisoryAnswer.*,ls.AdminScore</select>
    <from>msg_advisoryAnswer left outer join law_lawyerScore as ls on ls.MasterKey='msg_advisoryAnswer' and msg_advisoryAnswer.AnswerCode=ls.MasterValue</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='msg_advisoryAnswer.AnswerCode'		cp='equal'></field>   
        <field name='AdvisoryCode'		cp='equal'></field>   
        <field name='msg_advisoryAnswer.LawyerCode'		cp='equal'></field>   
        <field name='ISChecked'		cp='equal'></field>   
        <field name='IsBestAnswer'		cp='equal'></field>   
        <field name='AnswerDate'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new msg_advisoryAnswerService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public IEnumerable<dynamic> GetByAdvisoryCode(string id)
        {
            var pQuery = ParamQuery.Instance().Select("A.*, case when A.IsBestAnswer = 1 then 1 else 0 end as Selected")
                .From(@"msg_advisoryAnswer as A")
                .AndWhere(@"AdvisoryCode", id)
                .AndWhere("ApproveState", "passed")
                .OrderBy("A.AnswerOrder");
            var result = masterService.GetDynamicList(pQuery);
            return result;
        }

        public override dynamic GetEditMaster(string id)
        {
            var masterService = new msg_advisoryAnswerService();
            var pQuery = ParamQuery.Instance().AndWhere("AnswerCode", id);
            var masterModel = masterService.GetModel(pQuery);

            var result = new
            {
                //主表数据
                form = masterModel,
                scrollKeys = masterService.ScrollKeys("AnswerCode", id),

                //明细数据
                tab0 = new msg_advisoryService().GetModel(ParamQuery.Instance().AndWhere("AdvisoryCode", masterModel==null?"": masterModel.AdvisoryCode)),
                tab1 = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("LawyerCode",masterModel==null?"": masterModel.LawyerCode)),
                //tab2 = new msg_advisoryAnswerService().GetModel(pQuery)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var advisoryAnswer = masterService.GetModel(ParamQuery.Instance().AndWhere("AnswerCode", id));

            var pUpdate = ParamUpdate.Instance()
                .Update("msg_advisoryAnswer")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("AnswerCode", id);

            var service = new msg_advisoryAnswerService();
            var rowsAffected = service.Update(pUpdate);
            //评分处理
            if (rowsAffected > 0)
            {
                var score = new sys_parameterService().GetModel(ParamQuery.Instance().AndWhere("ParamCode", "Score_Answer"));
                if (data["status"].ToString().ToLower() == "passed")
                {
                    new law_lawyerScoreService().EditScore(advisoryAnswer.LawyerCode, "msg_advisoryAnswer", id, "AnswerScore", Convert.ToDecimal(score.ParamValue));
                    if (advisoryAnswer.ApproveState != "passed")//向接口添加发送数据
                        Web.Api.ToYglz.InsertPost(id);
                }
                else
                {
                    law_lawyerScoreService s = new law_lawyerScoreService();
                    var scoreCode = s.GetScoreCode("msg_advisoryAnswer", id);
                    s.Delete(ParamDelete.Instance().AndWhere("ScoreCode", scoreCode));
                }
            }
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "审核失败，请重试或联系管理员！", id);
        }
        
        //后台评分
        [System.Web.Http.HttpPost]
        public void UpdateAdminScore(JObject data)
        {
            var lawyerCode = data.Value<string>("lawyerCode");
            var answerCode = data["answerCode"].ToString();
            var adminScore =  Convert.ToDecimal(data["adminScore"]);

            var rowsAffected = new law_lawyerScoreService().EditScore(lawyerCode, "msg_advisoryAnswer", answerCode, "AdminScore", adminScore);

            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "后台评分失败，请重试或联系管理员！", answerCode);
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
        msg_advisoryAnswer
    </table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>msg_advisoryAnswer</table>
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
    <table>msg_advisoryAnswer</table>
    <where>
        <field name='AnswerCode' cp='equal'></field>
    </where>
</settings>"));

            var service = new msg_advisoryAnswerService();
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
        // GET api/msg/advisoryanswer/getadvisoryAnswercode
        public virtual List<dynamic> GetAdvisoryAnswerCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 AnswerCode").AndWhere("AnswerCode", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion
    }
}
