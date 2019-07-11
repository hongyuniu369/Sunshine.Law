
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
    public class LawyerScoreController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsScoreItem = code.GetKeyTextListByType("ScoreItem")
                },
                urls = new{
                    query = "/api/Msg/LawyerScore",
                    newkey = "/api/Msg/LawyerScore/getnewkey",
                    edit = "/api/Msg/LawyerScore/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    LawyerCode = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "ScoreCode",
                    postListFields = new string[] { "ScoreCode" ,"LawyerCode" ,"MasterKey" ,"MasterValue" ,"WebScore" ,"AnswerScore" ,"BestScore" ,"AdminScore" }
                }
            };

            return View(model);
        }
    }

    public class LawyerScoreApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ScoreCode'>
    <select>*</select>
    <from>law_lawyerScore</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='LawyerCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new law_lawyerScoreService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new law_lawyerScoreService().GetNewKey("ScoreCode", "dateplus");
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        law_lawyerScore
    </table>
    <where>
        <field name='ScoreCode' cp='equal'></field>
    </where>
</settings>");
            var service = new law_lawyerScoreService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
