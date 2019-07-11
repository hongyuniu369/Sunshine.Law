using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("CMS")]
    public class linkService : ServiceBase<link>
    {
    }

    public class link : ModelBase
    {
        [PrimaryKey]
        public string LinkCode { get; set; }
        public string LinkName { get; set; }
        public string LinkPic { get; set; }
        public string LinkUrl { get; set; }
        public string ParentLinkCode { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnable { get; set; }
        public int? LinkOrder { get; set; }
        public string LinkRemark { get; set; }
    }
}