
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
    public class EntrustController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = MsgHelper.GetEditUrls("Entrust"),
                resx = MsgHelper.GetEditResx("案件委托"),
                dataSource = new{
                    pageData=new EntrustApiController().GetEditMaster(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new msg_entrust().Extend(new {  EntrustCode = "",MsgTitle = "",MsgSubdescription = "",MsgContent = "",MsgRequirement = "",InvolvedMoney = "",ActiveTime = "",Address = "",DetailAddress = "",TrueName = "",MsgClass = "",Keyword = "",IsInternalReference = "",ContactNO = "",Email = "",MsgIP = "",IgnoreCode = "",MsgStatus = "",HitCount = "",IsPublicity = "",IsClassic = "",MsgInDate = "",ApproveState = "",ApprovePerson = "",ApproveDate = "",ApproveRemark = "",MsgCheckDate = "",ReferenceSource = "",MsgRemark = "",UserCode = "",LawyerCode = "",AgreeTime = ""}),
                    primaryKeys = new string[]{"EntrustCode"}
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

    public class EntrustApiController : MsgBaseApi<msg_entrust, msg_entrustService>
    {
        public override dynamic GetEditMaster(string id)
        {
            var masterService = new msg_entrustService();
            var pQuery = ParamQuery.Instance().AndWhere("EntrustCode", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("EntrustCode", id),

                //明细数据
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("msg_entrust")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("EntrustCode", id);

            var service = new msg_entrustService();
            var rowsAffected = service.Update(pUpdate);
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
  
        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type,string key,int qty=1)
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
        msg_entrust
    </table>
    <where>
        <field name='EntrustCode' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
             
            var service = new msg_entrustService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
