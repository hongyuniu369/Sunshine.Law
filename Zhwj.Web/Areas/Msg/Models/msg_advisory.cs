using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_advisoryService : ServiceBase<msg_advisory>
    {
        protected override bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            if (arg.dataAction == OptType.Add)
            {
                if (!string.IsNullOrEmpty(FormsAuth.GetUserData().UserCode))
                    arg.dataNew["UserCode"] = FormsAuth.GetUserData().UserCode;
                arg.dataNew["MsgIP"] = System.Web.HttpContext.Current.Request.UserHostAddress;
                arg.dataNew["MsgInDate"] = DateTime.Now;
                arg.dataNew["MsgStatus"] = 0;
            }
            return base.OnBeforeEditPageForm(arg);
        }

        public int getReplyCount(string advisoryCode)
        {
            int replyCount = new Int32();
            var sql = String.Format(@"
select ReplyCount=count(*) 
    from msg_advisoryAnswer
where AdvisoryCode='{0}' and ApproveState='passed'", advisoryCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            replyCount = result[0].ReplyCount;
            return replyCount;
        }
    }

    public class msg_advisory : ModelBase
    {
        [PrimaryKey]
        public string AdvisoryCode { get; set; }
        public string MsgTitle { get; set; }
        public string MsgContent { get; set; }
        public string LawyerCode { get; set; }
        public string Address { get; set; }
        public string DetailAddress { get; set; }
        public string Keyword { get; set; }
        public string MsgClass { get; set; }
        public bool IsOpen { get; set; }
        public bool? IsInternalReference { get; set; }
        public string ContactNO { get; set; }
        public bool? PhoneIsOpen { get; set; }
        public string Email { get; set; }
        public string MsgIP { get; set; }
        public string IgnoreCode { get; set; }
        public string MsgStatus { get; set; }
        public int HitCount { get; set; }
        public bool IsPublicity { get; set; }
        public bool IsClassic { get; set; }
        public DateTime MsgInDate { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public DateTime? ReplyDate { get; set; }
        public DateTime? BestDate { get; set; }
        public string ReferenceSource { get; set; }
        public string MsgRemark { get; set; }
        public string UserCode { get; set; }
    }
}
