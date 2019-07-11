
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Msg;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;
namespace Zephyr.Controllers
{
    [Zephyr.Web.MvcMenuFilter(false)]
    public class LawController : Controller
    {
        /// <summary>
        ///注册
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Register()
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Law"),
                resx = MsgHelper.GetEditResx("注册"),
                dataSource = new
                {
                    pageData = new
                    {
                        form = new
                        {
                            UserCode = "",
                            Password = "",
                            Password2="",
                            UserName="",
                            Email="",
                            ContactNO=""
                        }
                    }
                },
                form = new
                {
                    defaults = new
                    {
                        UserCode = "",
                        Password = "",
                        Password2 = "",
                        UserName = "",
                        Email = "",
                        ContactNO = ""
                    },
                    primaryKeys = new string[] { "UserCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/law/editregister";
            return View(model);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AllowAnonymous]

        public ActionResult Login()
        {

            return View();
        }

        public ActionResult Person()
        {
            dynamic Model = new ExpandoObject();
            Model.Person = new sys_userService().GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode()));
            return View(Model);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Personpwd()
        {
            var model = new
            {
                urls = MsgHelper.GetEditUrls("Law"),
                resx = MsgHelper.GetEditResx("修改密码"),
                dataSource = new
                {
                    pageData = new
                    {
                        form = new
                        {
                            UserCode=MsgHelper.GetUserCode(),
                            Password = "",
                            NewPassword=""
                            
                        }
                    }
                },
                form = new
                {
                    defaults = new
                    {
                        UserCode = MsgHelper.GetUserCode(),
                        Password = "",
                        NewPassword = ""

                    },
                    primaryKeys = new string[] { "UserCode" }
                },
                tabs = new object[]{
                    new{
                      type = "empty",
                      defaults = new {}
                    }    
                }
            };
            model.urls.edit = "/api/law/editpassword";
            return View(model);
        }
        /// <summary>
        /// 个人委托列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Personwt()
        {

            var model = new
            {
                urls = new { query = "/api/Law/GetEntrust" },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.EntrustStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.EntrustStatus))).Select(s => new { value = ((int)s), text = s.ToString() }),
                },
                pagination = new
                {
                    page = 1,
                    rows = 10
                },
                form = new
                {
                    EntrustCode = "",
                    MsgTitle = "",
                    MsgSubdescription = "",
                    InvolvedMoney = "",
                    MsgStatus = "",
                    MsgInDate = "",
                    WtReplyCount = "",
                    EntrustAnswer = ""
                },
                idField = "EntrustCode"
            };
            return View(model);
        }
        /// <summary>
        /// 个人中心列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Personzx()
        {
            var model = new
            {
                urls = new { query = "/api/Law/GetAdvisory" },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                    msgStatus = ((IList<Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus>)Enum.GetValues(typeof(Zephyr.Areas.Msg.Common.MsgEnum.AdvisoryStatus))).Select(s => new { value = ((int)s), text = s.ToString() }),
                },
                pagination = new
                {
                    page = 1,
                    rows = 10
                },
                form = new
                {
                    AdvisoryCode = "",
                    MsgTitle = "",
                    MsgContent = "",
                    MsgStatus = "",
                    MsgInDate = "",
                    ZxReplyCount = "",
                    AdvisoryAnswer = ""
                },
                idField = "AdvisoryCode"
            };

            return View(model);
        }
    }


    [MvcMenuFilter(false)]
    public class LawApiController : ApiController
    {
        /// <summary>
        /// 修改密码操作
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Object EditPassword(dynamic data)
        {
            var service = new sys_userService();
            bool state = false;
            string message = "";
            string userCode = MsgHelper.GetUserCode();
            string password = service.GetModel(ParamQuery.Instance().AndWhere("UserCode", MsgHelper.GetUserCode())).Password;
            if (password == data.form.Password.ToString())
            {
                data.form.Password = data.form.NewPassword;
                data.form.Remove("NewPassword");
                var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", "sys_user", "UserCode");

                var tabsWrapper = new List<RequestWrapper>();
                var result = service.EditPage(data, formWrapper, tabsWrapper);
                state = true;
                message = "修改成功";
            }
            else {
                message = "原密码不正确";
            }
            var json= JsonConvert.SerializeObject(new { state = state, message = message });
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        
        public Object EditRegister(dynamic data)
        {
            var service = new sys_userService();
            bool state = false;
            string message = "";
            string UserCode=data.form.UserCode.ToString();
            int count = service.GetModelList(ParamQuery.Instance().AndWhere("UserCode", UserCode)).Count();
            if (count != 0)
            {
                message = "用户已存在";
            }
            else {
                data.form.Remove("Password2");
                data.form.Add("IsEnable",true);
                
                var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", "sys_user", "UserCode");

                var tabsWrapper = new List<RequestWrapper>();
                var result = service.EditPage(data, formWrapper, tabsWrapper);
                state = true;
                message = "注册成功";
            }
            var json = JsonConvert.SerializeObject(new { state = state, message = message });
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// 个人委托列表获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic GetEntrust(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='EntrustCode desc'>
    <select>EntrustCode,MsgTitle,MsgSubdescription,InvolvedMoney,MsgStatus,MsgInDate
    </select>
    <from>msg_entrust
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EntrustCode'	cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_entrustService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("UserCode", MsgHelper.GetUserCode(), Cp.Equal);
            var result = service.GetDynamicListWithPaging(pQuery);
            foreach (var row in result.rows)
            {
                row.WtReplyCount = service.getReplyCount(row.EntrustCode);
                row.EntrustAnswer = "";
            }
            return result;
        }
        /// <summary>
        /// 个人委托回复获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic  GetEntrustReply (RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AnswerCode desc'>
    <select>E.AnswerCode,E.AnswerContent,E.AnswerDate,E.IsBestAnswer,E.SelectUserCode,L.LawyerPic,L.ProfessionalTitle,L.LawyerName,L.ContactNO,L.LawyerCode
    </select>
    <from>msg_entrustAnswer as E
left outer join law_lawyer as L on E.LawyerCode=L.LawyerCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EntrustCode'	cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_entrustAnswerService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("E.ApproveState", "passed", Cp.Equal);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        /// <summary>
        /// 设置委托最佳回复
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic getBestAnswer(string AnswerCode)
        {
            
            var service = new msg_entrustAnswerService();
            service.Best(AnswerCode);
            return 0;
        }
        /// <summary>
        ///个人咨询列表获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic GetAdvisory(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AdvisoryCode desc'>
    <select>AdvisoryCode,MsgTitle,MsgContent,MsgStatus,MsgInDate
    </select>
    <from>msg_advisory
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AdvisoryCode'	cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_advisoryService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("UserCode", MsgHelper.GetUserCode(), Cp.Equal);
            var result = service.GetDynamicListWithPaging(pQuery);
            foreach (var row in result.rows)
            {
                row.ZxReplyCount = service.getReplyCount(row.AdvisoryCode);
                row.AdvisoryAnswer = "";
            }
            return result;
        }
        /// <summary>
        /// 个人咨询回复获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic GetAdvisroyReply(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='AnswerCode desc'>
    <select>A.answerCode,A.AnswerContent,A.AnswerDate,L.LawyerPic,L.ProfessionalTitle,L.LawyerName,L.ContactNO,L.LawyerCode
    </select>
    <from>msg_advisoryAnswer as A
left outer join law_lawyer as L on A.LawyerCode=L.LawyerCode
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='AdvisoryCode'	cp='equal'></field>   
    </where>
</settings>");
            var service = new msg_advisoryAnswerService();
            var pQuery = query.ToParamQuery();
            pQuery.AndWhere("A.ApproveState", "passed", Cp.Equal);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        
        public IEnumerable<dynamic> GetLawyerClasses(string id)
        {
            return new law_lawyerService().GetLawyerClasses(id);
        }
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="data"></param>
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public void EditComment(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", typeof(msg_comment).Name, "CommentCode");

            var tabsWrapper = new List<RequestWrapper>();
            var service = new msg_commentService();
            data.form.CommentCode = service.GetNewKey("CommentCode", "dateplus");
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
