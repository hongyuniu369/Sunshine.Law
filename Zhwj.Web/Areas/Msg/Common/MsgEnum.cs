using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zephyr.Areas.Msg.Common
{
    public static class MsgEnum
    {
        /// <summary>
        /// 咨询状态
        /// </summary>
        public enum AdvisoryStatus {
            忽略 = -9,
            撤销审核 = -1,
            新提交 = 0,
            新咨询 = 5,
            已回复 = 30,
            已办结 = 99
        }
        /// <summary>
        /// 委托状态
        /// </summary>
        public enum EntrustStatus
        {
            忽略 = -9,
            撤销审核 = -1,
            新提交 = 0,
            新委托 = 5,
            接洽中 = 30,
            已择优 = 80,
            已完成 = 99
        }
    }

    public static class MsgData
    {
        static Dictionary<string, string> _audit = new Dictionary<string, string>();
        public static Dictionary<string, string> Audit
        {
            get {
                if (_audit.Count < 1)
                {
                    _audit = new Dictionary<string, string>();
                    _audit.Add("wait", "待审核");
                    _audit.Add("passed", "通过");
                    _audit.Add("reject", "未通过");
                }
                return _audit;
            }
        }

        static Dictionary<int, string> _bit = new Dictionary<int, string>();
        public static Dictionary<int, string> Bit
        {
            get
            {
                if (_bit.Count < 1)
                {
                    _bit = new Dictionary<int, string>();
                    _bit.Add(1, "是");
                    _bit.Add(0, "否");
                }
                return _bit;
            }
        }
    }
}