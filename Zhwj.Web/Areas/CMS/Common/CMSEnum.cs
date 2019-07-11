using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zephyr.Areas.CMS
{
    public static class CMSEnum
    {
        public enum AdvisoryStatus {
            忽略 = -9,
            撤销审核 = -1,
            新咨询 = 0,
            待分发 = 3,
            已审核 = 5,
            已回复 = 30,
            已办结 = 99
        }
    }

    public static class CMSData
    {
        static Dictionary<string, string> _audit = new Dictionary<string, string>();
        public static Dictionary<string, string> Audit
        {
            get {
                if (_audit.Count < 1)
                {
                    _audit = new Dictionary<string, string>();
                    _audit.Add("", "未审核");
                    _audit.Add("passed", "通过");
                    _audit.Add("reject", "未通过");
                }
                return _audit;
            }
        }

        static Dictionary<bool, string> _bit = new Dictionary<bool, string>();
        public static Dictionary<bool, string> Bit
        {
            get
            {
                if (_bit.Count < 1)
                {
                    _bit = new Dictionary<bool, string>();
                    _bit.Add(true, "是");
                    _bit.Add(false, "否");
                }
                return _bit;
            }
        }
    }
}