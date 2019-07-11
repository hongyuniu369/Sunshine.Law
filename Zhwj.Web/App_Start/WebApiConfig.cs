﻿using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Zephyr.Core;

namespace Zephyr.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Change ControllerSuffix from default string "Controller" to "ApiController"
            var suffix = typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null) suffix.SetValue(null, "ApiController");

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                "HomeApi",
                "api/{controller}/{action}/{id}",
                new {action = RouteParameter.Optional, id = RouteParameter.Optional, namespaceName = new string[] {"Zephyr.Controllers"} },
                new {action = new StartWithConstraint() }
            );

            //支持命名空间
            config.Services.Replace(typeof(IHttpControllerSelector),
                new NamespaceHttpControllerSelector(config));

            config.ParameterBindingRules.Insert(0, param =>
            {
                if (param.ParameterType == typeof(RequestWrapper))
                    return new RequestWrapperParameterBinding(param);

                return null;
            });

            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new WebApiExceptionFilter());
            config.Filters.Add(new WebApiDisposeFilter());
        }
    }
}
