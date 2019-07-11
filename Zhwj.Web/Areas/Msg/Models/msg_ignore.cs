using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_ignoreService : ServiceBase<msg_ignore>
    {
       
    }

    public class msg_ignore : ModelBase
    {
        [PrimaryKey]   
        public string IgnoreCode { get; set; }
        public string IgnoreReason { get; set; }
        public int? IgnoreOrder { get; set; }
        public string IgnoreRemark { get; set; }
    }
}
