
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Msg.Controllers
{
    public class CommentController : Controller
    {
        public ActionResult Advisory()
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("comment"),
                resx = MsgHelper.GetEditResx("咨询评论"),
                dataSource = new
                {
                    dsAudit = Zephyr.Areas.Msg.Common.MsgData.Audit.Select(s => new { value = s.Key, text = s.Value }),
                },
                form = new{
                    CommentCode = "" ,
                    MasterValue = "" ,
                    CommentContent = "" ,
                    CommentorName = "" ,
                    CommentDate = "" ,
                    ApproveState = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "CommentCode",
                    postListFields = new string[] { "CommentCode" ,"MasterValue" ,"CommentContent" ,"CommentorName" ,"CommentDate" ,"PraiseCount" ,"ApproveState" ,"CommentOrder" }
                }
            };
            model.urls.query = "/api/msg/comment/getbyadvisory";
            return View("index", model);
        }
    }

    public class CommentApiController : MsgBaseApi<msg_comment, msg_commentService>
    {
        public dynamic GetByAdvisory(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='CommentCode'>
    <select>C.*, A.MsgTitle</select>
    <from>msg_comment C inner join msg_advisory A on C.MasterKey='msg_advisory' and C.MasterValue=A.AdvisoryCode</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='CommentCode'		cp='equal'></field>   
        <field name='MasterKey'		cp='equal'></field>   
        <field name='MasterValue'		cp='equal'></field>   
        <field name='CommentContent'		cp='like'></field>   
        <field name='CommentorName'		cp='equal'></field>   
        <field name='CommentDate'		cp='daterange'></field>   
        <field name='C.ApproveState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_commentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public dynamic GetByEntrust(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='CommentCode'>
    <select>C.*, E.MsgTitle</select>
    <from>msg_comment C inner join msg_entrust E on C.MasterKey='msg_entrust' and C.MasterValue=E.AdvisoryCode</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='CommentCode'		cp='equal'></field>   
        <field name='MasterKey'		cp='equal'></field>   
        <field name='MasterValue'		cp='equal'></field>   
        <field name='CommentContent'		cp='like'></field>   
        <field name='CommentorName'		cp='equal'></field>   
        <field name='CommentDate'		cp='daterange'></field>   
        <field name='A.ApproveState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_commentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new msg_commentService().GetNewKey("CommentCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public override void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        msg_comment
    </table>
    <where>
        <field name='CommentCode' cp='equal'></field>
    </where>
</settings>");
            var service = new msg_commentService();
            var result = service.Edit(null, listWrapper, data);
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("msg_comment")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("CommentCode", id);

            var service = new msg_commentService();
            var rowsAffected = service.Update(pUpdate);
            MsgHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "审核失败[评论编号={0}]，请重试或联系管理员！", id);
        }

        #region 自动完成
        // GET api/msg/comment/GetCommentCode
        public virtual List<dynamic> GetCommentCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 CommentCode").AndWhere("CommentCode", q, Cp.Like);
            return new msg_commentService().GetDynamicList(pQuery);
        }
        #endregion
    }
}
