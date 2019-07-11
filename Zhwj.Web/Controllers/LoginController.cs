using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;
using Zephyr.Web;
using Zephyr.Areas.Msg;

namespace Zephyr.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.CnName = "企业管理系统";
            //ViewBag.EnName = "Enterprise Mangange System";
            //return View();
            return Mms();
        }

        public ActionResult Mms()
        {
            ViewBag.CnName = "河北新闻网阳光问法";
            ViewBag.EnName = "";
            return View("Index");
        }

        public ActionResult Psi() 
        {
            ViewBag.CnName = "企业进销存管理系统";
            ViewBag.EnName = "Purchase-Sales-Inventory Management System";
            ViewBag.EnNameStyle = "left:298px;";
            return View("Index");
        }

        public JsonResult DoAction(JObject request)
        {
            var message = new sys_userService().Login(request);
            return Json(message, JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <returns></returns>
        public JsonResult checkLogin() {
            bool state = false;
            string usercode = "";
            if (String.IsNullOrEmpty(MsgHelper.GetUserCode()))
             {
             }
             else {
                state = true;
                usercode = MsgHelper.GetUserCode();
             }
             var message = new { state = state, usercode = usercode };
             return  Json(message, JsonRequestBehavior.DenyGet);

        }
        public ActionResult Logout()
        {
            FormsAuth.SingOut();
            return Redirect("~/Login");
        }
        public ActionResult LogoutIndex()
        {
            FormsAuth.SingOut();
            return Redirect("~/");
        }
    }
}
