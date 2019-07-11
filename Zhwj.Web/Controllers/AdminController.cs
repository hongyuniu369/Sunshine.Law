﻿using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web;

namespace Zephyr.Web.Controllers
{
    [MvcMenuFilter(false)]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            ViewBag.Title = "河北新闻网阳光问法平台";
            ViewBag.UserName = loginer.UserName;
            ViewBag.Settings = new sys_userService().GetCurrentUserSettings();

            return View();
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
}