
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

namespace Zephyr.Areas.CMS.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = CMSHelper.GetIndexUrls("News"),
                resx = CMSHelper.GetIndexResx("新闻主表"),
                dataSource = new{
                    dsAudit = CMSData.Audit.Select(s => new { value = s.Key, text = s.Value }),
                    dsBit = CMSData.Bit.Select(s => new { value = s.Key, text = s.Value })
                },
                form = new{
                    NewsCode = "" ,
                    ClassCode = "" ,
                    Title = "" ,
                    ApproveState = "",
                    Signed = "" ,
                    InTime = "" 
                },
                idField="NewsCode"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            //设置上传路径
            var response = System.Web.HttpContext.Current.Response;
            var cookieCKFinder_Path = new System.Web.HttpCookie("CKFinder_RelativePath") { Value = "~/files/news/", Expires = DateTime.MaxValue };
            response.Cookies.Remove("CKFinder_RelativePath");
            response.Cookies.Add(cookieCKFinder_Path);
            var model = new
            {
                urls = CMSHelper.GetEditUrls("News"),
                resx = CMSHelper.GetEditResx("新闻主表"),
                dataSource = new
                {
                    pageData = new NewsApiController().GetEditMaster(id),
                    //dsAudit = CMSData.Audit.Select(s => new { value = s.Key, text = s.Value }),
                    dsBit = CMSData.Bit.Select(s => new { value = s.Key, text = s.Value })
                },
                form = new
                {
                    defaults = new news().Extend(new { NewsCode = id, ClassCode = "", Title = "", SubTitle = "", SubDescription = "", NewsContent = "", Keywords = "", TitlePic = "", IsPicNews = false, Signed = false, SignedTime="", OnClick = "", UserName = CMSHelper.GetUserName(), IsTop = false, InTime= "" }),
                    primaryKeys = new string[] { "NewsCode" }
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

    public class NewsApiController : CMSBaseApi<news, newsService>
    {
        
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='NewsCode desc'>
    <select>A.*,C.ClassName</select>
    <from>news A left outer join news_class C on A.ClassCode=C.ClassCode</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='NewsCode'		cp='equal'></field>   
        <field name='A.ClassCode'		cp='equal'></field>   
        <field name='Title'		cp='like'></field>   
        <field name='Checked'		cp='equal'></field>   
        <field name='Signed'		cp='equal'></field>   
        <field name='InTime'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new newsService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("news")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("NewsCode", id);

            var service = new newsService();
            var rowsAffected = service.Update(pUpdate);
            CMSHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }

        #region 自动完成
        // GET api/msg/advisory/getadvisorycode
        public virtual List<dynamic> GetNewsCode(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 NewsCode").AndWhere("NewsCode", q, Cp.Like);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion
    }
}
