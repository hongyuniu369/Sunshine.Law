using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class com_regionService : ServiceBase<com_region>
    {
    }

    public class com_region : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int RegionID { get; set; }
        public int ParentId { get; set; }
        public string RegionName { get; set; }
        public int DisplaySequence { get; set; }
        public string Path { get; set; }
        public int Depth { get; set; }
    }
}
