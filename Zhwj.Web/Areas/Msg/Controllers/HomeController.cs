using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Areas.Msg.Controllers
{
    [MvcMenuFilter(false)]
    public class HomeController : Controller
    {
        //
        // GET: /MMS/Home/
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("~/Login/mms");

            return View();
        }
    }
}
