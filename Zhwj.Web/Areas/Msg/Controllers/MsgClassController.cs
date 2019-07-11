using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Zephyr.Models;
using Zephyr.Core;
using Newtonsoft.Json.Linq;

namespace Zephyr.Areas.Msg.Controllers
{
    public class MsgClassController : Controller
    {
        //
        // GET: /Msg/MsgClass/

        public ActionResult Index()
        {
            return View();
        }
    }

    public class MsgClassApiController : ApiController
    {
        public dynamic Get(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='A.ClassOrder,A.ClassCode'>
    <select>
        A.*, B.ClassName As ParentClassName 
    </select>
    <from>
        msg_class A
        left join msg_class         B on A.ParentClassCode = B.ClassCode
    </from>
    <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
        <field name='A.ParentClassCode'></field>
        <field name='A.IsEnable'></field>
    </where>
</settings>
");
            var result = new msg_classService().GetDynamicListWithPaging(request.ToParamQuery());
            return result;
        }

        public dynamic GetRootMsgClass(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='ClassOrder, ClassCode'>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='IsEnable' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var paramQuery = request.ToParamQuery();
            paramQuery.OrWhere("ParentClassCode", DBNull.Value, Cp.IsNullOrEmpty);
            var result = new msg_classService().GetDynamicListWithPaging(paramQuery);
            return result;
        }

        /// <summary>
        /// 获取下拉列表数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        public List<dynamic> GetValueTextListRoot(RequestWrapper request)
        {
            request.LoadSettingXmlString(@"
<settings defaultOrderBy='ClassOrder, ClassCode'>
    <select>
        ClassCode as value, ClassName as text 
    </select>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='IsEnable' cp='equal' ignoreEmpty='true'></field>
    </where>
</settings>");
            var paramQuery = request.ToParamQuery();
            paramQuery.OrWhere("ParentClassCode", DBNull.Value, Cp.IsNullOrEmpty);
            var result = new msg_classService().GetDynamicList(paramQuery);
            return result;
        }

        /// <summary>
        /// 获取下拉列表数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        public List<dynamic> GetValueTextListByPid(string id)
        {
            var pQuery = ParamQuery.Instance().Select("ClassCode As value, ClassName As text")
                .AndWhere("IsEnable", 1)
                .AndWhere("ParentClassCode", id);
            var result = new msg_classService().GetDynamicList(pQuery);
            return result;
        }

        public string GetNewCode(RequestWrapper request)
        {
            var service = new msg_classService();
            return service.GetNewKey("ClassCode", "maxplus").PadLeft(3, '0');
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings defaultOrderBy='ClassOrder, ClassCode'>
    <table>msg_class</table>
    <where defaultForAll='false' defaultCp='equal' defaultIgnorEmpty='true'>
        <field name='ClassCode'></field>
    </where>
</settings>
");

            var service = new msg_classService();
            service.Edit(null, listWrapper, data);
        }
    }
}
