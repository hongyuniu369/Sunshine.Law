using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_commentService : ServiceBase<msg_comment>
    {
        protected override bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            if (arg.dataAction == OptType.Add)
            {
                arg.dataNew["CommentDate"] = DateTime.Now;
            }
            return base.OnBeforeEditPageForm(arg);
        }
    }

    public class msg_comment : ModelBase
    {
        [PrimaryKey]   
        public string CommentCode { get; set; }
        public string MasterKey { get; set; }
        public string MasterValue { get; set; }
        public string CommentContent { get; set; }
        public string UserCode { get; set; }
        public string CommentorName { get; set; }
        public string ContactWay { get; set; }
        public DateTime CommentDate { get; set; }
        public string ParentCommentId { get; set; }
        public int? CommentOrder { get; set; }
        public int? PraiseCount { get; set; }
        public string CommentRemark { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
    }
}
