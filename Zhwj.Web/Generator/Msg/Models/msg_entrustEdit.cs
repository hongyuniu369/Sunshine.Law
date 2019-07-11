using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_entrustService : ServiceBase<msg_entrust>
    {
       
    }

    public class msg_entrust : ModelBase
    {
        [PrimaryKey]   
        public string EntrustCode { get; set; }
        public string MsgTitle { get; set; }
        public string MsgSubdescription { get; set; }
        public string MsgContent { get; set; }
        public string MsgRequirement { get; set; }
        public string InvolvedMoney { get; set; }
        public DateTime? ActiveTime { get; set; }
        public string Address { get; set; }
        public string DetailAddress { get; set; }
        public string TrueName { get; set; }
        public string MsgClass { get; set; }
        public string Keyword { get; set; }
        public bool? IsInternalReference { get; set; }
        public string ContactNO { get; set; }
        public string Email { get; set; }
        public string MsgIP { get; set; }
        public string IgnoreCode { get; set; }
        public int MsgStatus { get; set; }
        public int HitCount { get; set; }
        public bool IsPublicity { get; set; }
        public bool IsClassic { get; set; }
        public DateTime MsgInDate { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public DateTime? MsgCheckDate { get; set; }
        public string ReferenceSource { get; set; }
        public string MsgRemark { get; set; }
        public string UserCode { get; set; }
        public string LawyerCode { get; set; }
        public string AgreeTime { get; set; }
    }
}
