using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Msg.Controllers
{
    public class MsgBaseApi<TMasterModel, TMasterService> : ApiController
        where TMasterModel : ModelBase, new()
        where TMasterService : ServiceBase<TMasterModel>, new()
    {
        #region 属性
        private TMasterService _masterService;
        public TMasterService masterService
        {
            get
            {
                if (_masterService == null)
                    _masterService = new TMasterService();
                return _masterService;
            }
        }
        public string projectCode { get { return MsgHelper.GetCurrentProject(); } }
        public string userName { get { return MsgHelper.GetUserName(); } }
        string keyField = ModelBase.GetAttributeFields<TMasterModel, PrimaryKeyAttribute>()[0];
        #endregion

        #region 采播
        // 取得新的主表Bill GET api/mms/send/getnewbillno
        public virtual string GetNewCode()
        {
            return masterService.GetNewKey(keyField, "dateplus");
        }
        #endregion

        #region 查询
        // 查询主表数据列表 GET api/mms/send 
        public virtual dynamic Get(RequestWrapper query)
        {
            if (!query.IsLoadedSettings)
                query.LoadSettingXmlString(@"
<settings defaultOrderBy='{1}'>
    <select>*</select>
    <from>{0}</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", new string[]{typeof(TMasterModel).Name, keyField});
            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        // 取得编辑页面中的主表数据及上一页下一页主键 GET api/mms/send/geteditmaster 
        public virtual dynamic GetEditMaster(string id)
        {
            return new
            {
                form = masterService.GetModel(ParamQuery.Instance().AndWhere(keyField, id)),
                scrollKeys = masterService.ScrollKeys(keyField, id, ParamQuery.Instance())
            };
        }
        #endregion

        #region 删除
        // 删除 DELETE api/mms/send
        public virtual void Delete(string id)
        {
            var result = masterService.Delete(ParamDelete.Instance().AndWhere(keyField, id));
            MsgHelper.ThrowHttpExceptionWhen(result <= 0, "单据删除失败[" + keyField + "={0}]，请重试或联系管理员！", id);
        }
        #endregion

        #region 保存
        // 保存 POST api/mms/send
        [HttpPost]
        public virtual void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='{1}' cp='equal'></field>
    </where>
</settings>", typeof(TMasterModel).Name, keyField);

            var tabsWrapper = new List<RequestWrapper>();
            var result = masterService.EditPage(data, formWrapper, tabsWrapper);
        }
        #endregion
    }
}