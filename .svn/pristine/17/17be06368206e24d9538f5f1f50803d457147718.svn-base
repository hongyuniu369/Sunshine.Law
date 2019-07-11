
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Msg.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = MsgHelper.GetIndexUrls("Post"),
                resx = MsgHelper.GetIndexResx("实时发送"),
                form = new
                {
                    UrlCode = "" ,
                    PostContent = "" ,
                    PostResult = "" ,
                    PostDate = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "PostCode",
                    postListFields = new string[] { "PostCode" ,"UrlCode" ,"PostContent" ,"PostResult" ,"PostRemark" ,"PostDate" }
                }
            };
            model.urls.edit = "/api/Msg/Post/edit/";

            return View(model);
        }
    }

    public class PostApiController : MsgBaseApi<api_post, api_postService>
    {
        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='PostCode'>
    <select>*</select>
    <from>api_post</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UrlCode'		cp='equal'></field>   
        <field name='PostContent'		cp='like'></field>   
        <field name='PostResult'		cp='equal'></field>   
        <field name='PostDate'		cp='daterange'></field>   
    </where>
</settings>");
            var service = new api_postService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public override void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        api_post
    </table>
    <where>
        <field name='PostCode' cp='equal'></field>
    </where>
</settings>");
            var service = new api_postService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
