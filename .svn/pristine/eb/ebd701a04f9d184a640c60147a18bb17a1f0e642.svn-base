using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    [MvcMobileFilter(true)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            dynamic model = new System.Dynamic.ExpandoObject();
            //最新咨询
            var pQuery = ParamQuery.Instance();
            pQuery.Select("A.*,C.ClassName").From("msg_advisory as A left outer join msg_class as C on A.MsgClass=C.ClassCode").AndWhere("MsgStatus", 5).OrderBy("ApproveDate Desc").Paging(1, 6);
            model.zxzx = new msg_advisoryService().GetDynamicList(pQuery);
            //已解决咨询
            pQuery.ClearWhere().AndWhere("MsgStatus", 30, Cp.GreaterEqual, new object[] { });
            model.yjjzx = new msg_advisoryService().GetDynamicList(pQuery);
            //最新委托
            pQuery = ParamQuery.Instance().Select("A.*,C.ClassName").From("msg_entrust as A left outer join msg_class as C on A.MsgClass=C.ClassCode").AndWhere("MsgStatus", 5).OrderBy("ApproveDate Desc").Paging(1, 6);
            model.zxwt = new msg_advisoryService().GetDynamicList(pQuery);
            //成功案例
            pQuery = ParamQuery.Instance().Select("A.*,C.ClassName,L.LawyerName").From("msg_entrust as A left outer join msg_class as C on A.MsgClass=C.ClassCode left outer join law_lawyer as L on A.LawyerCode=L.LawyerCode").AndWhere("MsgStatus", 30, Cp.GreaterEqual, new object[] { });
            model.cgal = new msg_advisoryService().GetDynamicList(pQuery);
            //找律师
            pQuery = ParamQuery.Instance().AndWhere("LawyerStatus", 1).OrderBy("LawyerOrder").Paging(1, 7);
            model.lawyer = new law_lawyerService().GetDynamicList(pQuery);
            //律师魅力榜
            pQuery = ParamQuery.Instance().Select("*").From("v_lawyerScore").AndWhere("LawyerStatus", 1).OrderBy("TotalScore desc").Paging(1, 8);
            model.llmlb = new law_lawyerService().GetDynamicList(pQuery);
            //找律所
            pQuery = ParamQuery.Instance().AndWhere("FirmStatus", 1).OrderBy("FirmOrder").Paging(1, 3);
            model.firm = new law_firmService().GetDynamicList(pQuery);

            return View(model);
        }

        public ActionResult Jwc()
        {
            ViewBag.Title = "CMS信息管理系统";

            var pQuery = ParamQuery.Instance();
            var service = new newsService();
            pQuery.Select("top (5) *").AndWhere("ClassCode", "0001").AndWhere("Signed", "1").AndWhere("IsPicNews", "0").OrderBy("SignedTime Desc");
            var jwgg = service.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().Select("top (1) *").AndWhere("ClassCode", "0001").AndWhere("Signed", "1").AndWhere("IsPicNews", "1").OrderBy("SignedTime Desc");
            var jwggpic = service.GetDynamic(pQuery);
            pQuery = ParamQuery.Instance().Select("top (4) *").AndWhere("ClassCode", "0011").AndWhere("Signed", "1").OrderBy("SignedTime Desc");
            var flash = service.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().Select("top (8) *").AndWhere("Signed", "1").AndWhere("Signedtime", DateTime.Now.AddDays(-100), Cp.DtGreaterEqual, new object[] { }).OrderBy("OnClick Desc");
            var llph = service.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().Select("top (8) *").AndWhere("Signed", "1").AndWhere("ClassCode", "'0010','0006','0007','0005','0008','0009'", Cp.In, new object[] { }).OrderBy("SignedTime Desc");
            var jwdt = service.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().Select("top (8) *").AndWhere("ClassCode", "0013").AndWhere("Signed", "1").OrderBy("SignedTime Desc");
            var jwbgxz = service.GetDynamicList(pQuery);
            var linkService = new linkService();
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0006").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var jwzy = linkService.GetDynamic(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0001").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var tplj = linkService.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0002").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var link2 = linkService.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0003").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var link3 = linkService.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0004").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var link4 = linkService.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0005").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var link5 = linkService.GetDynamicList(pQuery);
            pQuery = ParamQuery.Instance().AndWhere("ParentLinkCode", "0046").AndWhere("IsVisible", "1").AndWhere("IsEnable", "1").OrderBy("LinkOrder");
            var link46 = linkService.GetDynamicList(pQuery);

            dynamic Model = new System.Dynamic.ExpandoObject();
            Model.jwgg = jwgg;
            Model.jwggpic = jwggpic;
            Model.flash = flash;
            Model.llph = llph;
            Model.jwdt = jwdt;
            Model.jwbgxz = jwbgxz;
            Model.jwzy = jwzy;
            Model.tplj = tplj;
            Model.link2 = link2;
            Model.link3 = link3;
            Model.link4 = link4;
            Model.link5 = link5;
            Model.link46 = link46;

            //return RedirectToRoute(new { controller = "Law", action = ""});
            return View(Model);
        }
 
        public ActionResult Error() 
        {
            return View();
        }

        public void Download()
        {
            Exporter.Instance().Download();
        }
    }

    [System.Web.Http.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class HomeApiController : ApiController
    {
        #region CMS
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='A.ClassOrder,A.ClassCode'>
    <select>
        A.*, B.ClassName As ParentClassName 
    </select>
    <from>
        news_class A
        left join news_class         B on A.ParentClassCode = B.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
        <field name='A.ParentClassCode'></field>
        <field name='A.IsEnable'></field>
    </where>
</settings>
");
            var result = new news_classService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public List<dynamic> GetRootNewsClass()
        {
            var pQuery = ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("IsVisible", 1).AndWhere("ParentClassCode", DBNull.Value, Cp.IsNullOrEmpty).OrderBy("ClassOrder");
            var result = new news_classService().GetDynamicList(pQuery);
            return result;
        }

        public List<dynamic> GetNewsByOnClick()
        {
            var pQuery = ParamQuery.Instance().Select("top (8) * ").AndWhere("Signed", 1).OrderBy("OnClick Desc");
            var result = new newsService().GetDynamicList(pQuery);
            return result;
        }
        #endregion

        #region Law
        /// <summary>
        /// 获取所有留言类型:/api/home/getmsgclass
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<dynamic> GetMsgClass()
        {
            var pQuery = ParamQuery.Instance().AndWhere("IsEnable", "1");
            var result = new msg_classService().GetDynamicList(pQuery);
            return result;
        }
        #endregion
    }
}
