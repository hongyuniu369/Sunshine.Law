using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_classService : ServiceBase<msg_class>
    {
    }

    public class msg_class : ModelBase
    {
        [PrimaryKey]
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string ParentClassCode { get; set; }
        public bool? IsEnable { get; set; }
        public int? ClassOrder { get; set; }
        public string ClassRemark { get; set; }
    }
}