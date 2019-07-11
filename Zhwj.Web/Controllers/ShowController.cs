using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Web.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class ShowController : Controller
    {
        public ActionResult Index(string id)
        {
            new newsService().OnClick(id);
            var news = new newsService().GetModel(ParamQuery.Instance().AndWhere("NewsCode", id));
            if (news.ApproveState != "passed" || news.Signed == false)
                news = null;
            var newsClass = news == null ? null : new news_classService().GetModel(ParamQuery.Instance().AndWhere("ClassCode", news.ClassCode));
            dynamic Model = new System.Dynamic.ExpandoObject();
            Model.newsClass = newsClass ?? new news_class();
            Model.news = news ?? new news();
            //var model = new
            //{
            //    newsClass = newsClass ?? new news_class(),
            //    news = news ?? new news()
            //};
            return View(Model);
        }
    }
}
