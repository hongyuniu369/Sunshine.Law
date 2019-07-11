using System.Collections.Specialized;
using System.IO;
using System.Web.Mvc;
using Zephyr.Core;

namespace Zephyr.Web
{
    public class RequestWrapperModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Set the binding result here
            var request = System.Web.HttpUtility.ParseQueryString(controllerContext.RequestContext.HttpContext.Request.Url.Query);
            var requestWrapper = new RequestWrapper(new NameValueCollection(request));
            if (!string.IsNullOrEmpty(request["_xml"]))
            {
                var xmlType = request["_xml"].Split('.');
                var xmlPath = string.Format("~/Views/Shared/Xml/{0}.xml", xmlType[xmlType.Length - 1]);
                if (xmlType.Length > 1)
                    xmlPath = string.Format("~/Areas/{0}/Views/Shared/Xml/{1}.xml", xmlType);

                requestWrapper.LoadSettingXml(xmlPath);
            }
            return requestWrapper;
        }
    }
}