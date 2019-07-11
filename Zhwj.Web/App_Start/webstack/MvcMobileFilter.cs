using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zephyr.Web
{
    public class MvcMobileFilter : ActionFilterAttribute
    {
        private bool _isEnable = false;

        public MvcMobileFilter()
        {
            _isEnable = false;
        }

        public MvcMobileFilter(bool IsEnable) 
        {
            _isEnable = IsEnable;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_isEnable && filterContext.HttpContext.Request.Browser.IsMobileDevice)
            {
                var list = new List<string>() {"home","firm", "lawyer", "zx" };
                var route = filterContext.RouteData.Values;
                ////配置参数
                //HttpRequestBase bases = (HttpRequestBase)filterContext.HttpContext.Request;
                //string url = bases.RawUrl.ToString().ToLower();
                ////获取url中的参数
                //string queryString = bases.QueryString.ToString();

                if (route.ContainsKey("area") == false && list.Contains(route["controller"].ToString().ToLower()))
                {
                    var controller = route["controller"].ToString().ToLower();
                    var action = route["action"].ToString().ToLower();
                    //string id = route.ContainsKey("id") ? route["id"].ToString().ToLower() : "";
                    //if (!string.IsNullOrEmpty(queryString))
                    //{
                    //    if (string.IsNullOrEmpty(id))
                    //        id = "?" + queryString;
                    //    else
                    //        id = id + "?" + queryString;
                    //}

                    if (controller == "home" && action == "index")
                    {
                        filterContext.Result = new RedirectResult("http://tousu.hebnews.cn/node_117284.htm");
                    }
                    if (controller == "zx")
                    {
                        if (action == "index")
                        {
                            route["controller"] = "h5";
                            route["action"] = "list";
                        }
                        if (action == "wyzx")
                        {
                            route["controller"] = "h5";
                            route["action"] = "wyzx";
                        }
                        if (action == "show")
                        {
                            route["controller"] = "h5";
                            route["action"] = "show";
                        }
                    }
                    if (controller == "firm")
                    {
                        if (action == "index")
                        {
                            route["controller"] = "h5";
                            route["action"] = "firm";
                        }
                        if (action == "show")
                        {
                            route["controller"] = "h5";
                            route["action"] = "firmdetail";
                        }
                    }
                    if (controller == "lawyer")
                    {
                        if (action == "index")
                        {
                            route["controller"] = "h5";
                            route["action"] = "lawyer";
                        }
                        if (action == "show")
                        {
                            route["controller"] = "h5";
                            route["action"] = "lawyerdetail";
                        }
                    }
                    //filterContext.Result = new RedirectResult("/" + controller + "/" + action + "/" + id);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}