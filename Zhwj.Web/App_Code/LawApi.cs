//======================================================================
//
//        Copyright (C) 2010-2011 HEBCCC All rights reserved   
//        Guid: 078057f8-e03a-4dfe-88a5-85f98cc262c3
//        CLR Version: 4.0.30319.17929
//        Machine Name: DELL-PC
//        Registered Organization: 
//        File Name: App
//        Discription: 
//
//        Created by Hongyuniu at  2014/9/2 14:23:23
//        http://hongyuniu.cnblogs.com
//
//======================================================================

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
namespace Zephyr.Web.Api
{

    /// <summary>
    /// Api 类
    /// </summary>
    public partial class LawApi
    {
        #region 同步数据 Synchronous

        private static System.Threading.Thread threadYglz = null;
        private static System.Threading.Thread threadYunDuan = null;
        private static System.Threading.Thread threadDsb = null;
        /// <summary>
        /// 开启独立线程执行同步操作
        /// </summary>
        public static void Synchronous()
        {
            //阳光理政回复
            threadYglz = new System.Threading.Thread(new System.Threading.ThreadStart(ToYglz.PostReply));
            threadYglz.IsBackground = true;
            threadYglz.Priority = System.Threading.ThreadPriority.Highest;
            if (!threadYglz.IsAlive)
            {
                threadYglz.Start();
            }
        }
        /// <summary>
        /// 关闭同步线程
        /// </summary>
        public static void Synchronous_Stop()
        {
            //关闭阳光理政线程
            if (threadYglz != null && threadYglz.IsAlive)
            {
                threadYglz.Abort();
            }
        }
        #endregion
    }
}