
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
    public class RegionController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = new{
                    query = "/api/Msg/Region",
                    newkey = "/api/Msg/Region/getnewkey",
                    edit = "/api/Msg/Region/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    RegionName = "" ,
                    RegionID = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "RegionID",
                    postListFields = new string[] { "RegionID" ,"ParentId" ,"RegionName" ,"DisplaySequence" ,"Path" ,"Depth" }
                }
            };

            return View(model);
        }
    }

    public class RegionApiController : MsgBaseApi<com_region, com_regionService>
    {
        /// <summary>
        /// 根据ParentId获取子项目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        public List<com_region> GetByPid(int id)
        {
            var service = new com_regionService();
            var pQuery = ParamQuery.Instance()
                .From(@"com_region")
                .AndWhere("ParentId", id)
                .OrderBy("DisplaySequence");
            var result = service.GetModelList(pQuery);
            return result;
        }

        public override dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='RegionID'>
    <select>*</select>
    <from>com_region</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='RegionName'		cp='equal'></field>   
        <field name='RegionID'		cp='equal'></field>   
    </where>
</settings>");
            var service = new com_regionService();
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
        com_region
    </table>
    <where>
        <field name='RegionID' cp='equal'></field>
    </where>
</settings>");
            var service = new com_regionService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
