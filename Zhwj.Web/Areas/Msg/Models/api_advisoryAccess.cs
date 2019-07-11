using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class api_advisoryAccessService : ServiceBase<api_advisoryAccess>
    {
        public api_advisoryAccess Insert(api_advisoryAccess access, msg_advisory advisory)
        {
            api_advisoryAccess aaa = GetModel(ParamQuery.Instance().AndWhere("UrlCode", access.UrlCode).AndWhere("MasterCode", access.MasterCode).AndWhere("AccessResult", 1));
            db.UseTransaction(true);
            Logger("接收接口数据", () =>
            {
                if (aaa == null)
                {
                    string advisoryCode = Zephyr.Core.NewKey.dateplus(db, "msg_advisory", "AdvisoryCode", "yyyyMMdd", 4);
                    db.Insert("msg_advisory")
                        .Column("AdvisoryCode", advisoryCode)
                        .Column("MsgTitle", advisory.MsgTitle)
                        .Column("MsgContent", advisory.MsgContent)
                        .Column("IsOpen", advisory.IsOpen)
                        .Column("MsgIP", advisory.MsgIP)
                        .Column("MsgInDate", DateTime.Now)
                        .Column("Address", advisory.Address)
                        .Column("DetailAddress", advisory.DetailAddress)
                        .Column("Email", advisory.Email)
                        .Column("ContactNO", advisory.ContactNO)
                        .Column("MsgClass", advisory.MsgClass)
                        .Execute();
                    access.AccessCode = GetNewKey("AccessCode", "dateplus");
                    access.AdvisoryCode = advisoryCode;
                    db.Insert("api_advisoryAccess")
                        .Column("AccessCode", access.AccessCode)
                        .Column("UrlCode", access.UrlCode)
                        .Column("AccessDate", DateTime.Now)
                        .Column("AdvisoryCode", access.AdvisoryCode)
                        .Column("MasterCode", access.MasterCode)
                        .Execute();
                }
                else
                    access.AccessCode = aaa.AccessCode;

                db.Commit();
           }, e => db.Rollback());

            return access;
        }
    }

    public class api_advisoryAccess : ModelBase
    {
        [PrimaryKey]   
        public string AccessCode { get; set; }
        public string UrlCode { get; set; }
        public string MasterCode { get; set; }
        public string AdvisoryCode { get; set; }
        public int AccessResult { get; set; }
        public string AccessRemark { get; set; }
        public DateTime? AccessDate { get; set; }
    }
}
