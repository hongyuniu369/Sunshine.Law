using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("CMS")]
    public class newsService : ServiceBase<news>
    {
        protected override bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            if (arg.dataAction == OptType.Add)
            {
                arg.dataNew["InTime"] = DateTime.Now;
                arg.dataNew["UserCode"] = FormsAuth.GetUserData().UserCode;
                arg.dataNew["UserName"] = string.IsNullOrEmpty(arg.dataNew["UserName"].ToString()) ? FormsAuth.GetUserData().UserName : arg.dataNew["UserName"];
            }
            return base.OnBeforeEditPageForm(arg);
        }

        public int OnClick(string id)
        {
            var sql = String.Format(@"
                    update news  
                        set OnClick = OnClick+1
                    where NewsCode = {0}", id);
            return db.Sql(sql).Execute();

        }
    }

    public class news : ModelBase
    {
        [PrimaryKey]   
        public string NewsCode { get; set; }
        public string ClassCode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SubDescription { get; set; }
        public string NewsContent { get; set; }
        public string Keywords { get; set; }
        public string TitlePic { get; set; }
        public bool IsPicNews { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public bool Signed { get; set; }
        public int? OnClick { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public bool? IsTop { get; set; }
        public DateTime? SignedTime { get; set; }
        public DateTime? InTime { get; set; }
    }
}
