using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class api_postService : ServiceBase<api_post>
    {
       
    }

    public class api_post : ModelBase
    {
        [PrimaryKey]   
        public string PostCode { get; set; }
        public string UrlCode { get; set; }
        public string PostContent { get; set; }
        public int? PostResult { get; set; }
        public string PostRemark { get; set; }
        public DateTime? PostDate { get; set; }
    }
}
