using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class law_firmService : ServiceBase<law_firm>
    {
        /// <summary>
        /// 获取所有根msg_class 并标记改firmCode的msg_class为已选择
        /// </summary>
        /// <param name="firmCode"></param>
        /// <returns></returns>
        public dynamic GetFirmClasses(string firmCode)
        {
            var sql = String.Format(@"
select A.*,
    case when B.FirmCode is null then 0 else 1 end as Selected
from msg_class A
    left join law_firmClassMap B on B.FirmCode = '{0}' and B.ClassCode = A.ClassCode
where A.IsEnable = 1 and (A.ParentClassCode is null or A.ParentClassCode = '') 
order by ClassOrder", firmCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        /// <summary>
        /// 获取该律所的msg_class
        /// </summary>
        /// <param name="firmCode"></param>
        /// <returns></returns>
        public dynamic GetFirmClass(string firmCode)
        {
            var sql = String.Format(@"
select A.*
from msg_class A
    right join law_firmClassMap B on B.FirmCode = '{0}' and B.ClassCode = A.ClassCode
where A.IsEnable = 1  
order by ClassOrder", firmCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        /// <summary>
        /// 获取该律所的律师
        /// </summary>
        /// <param name="firmCode"></param>
        /// <returns></returns>
        public List<dynamic> GetLawyers(string firmCode)
        {
            var sql = String.Format(@"
select A.*
from law_lawyer A
where A.FirmCode = '{0}'  
order by LawyerOrder", firmCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }
        public void SaveFirmClasses(string firmCode, JToken classList)
        {
            db.UseTransaction(true);
            Logger("设置律所业务领域", () =>
            {
                db.Delete("law_firmClassMap").Where("FirmCode", firmCode).Execute();
                foreach (JToken item in classList.Children())
                {
                    var classCode = item["ClassCode"].ToString();
                    var firmClassMapCode = Zephyr.Core.NewKey.dateplus(db, "law_firmClassMap", "FirmClassMapCode", "yyyyMMdd", 4); 
                    db.Insert("law_firmClassMap").Column("FirmClassMapCode", firmClassMapCode).Column("FirmCode", firmCode).Column("ClassCode", classCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }
    }

    public class law_firm : ModelBase
    {
        [PrimaryKey]   
        public string FirmCode { get; set; }
        public string OrganizeCode { get; set; }
        public string FirmName { get; set; }
        public string FirmLogo { get; set; }
        public string FirmAddress { get; set; }
        public string FrimDetailAddress { get; set; }
        public string ZIPCode { get; set; }
        public string ContactNO { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string FirmDescription { get; set; }
        public string FirmRemark { get; set; }
        public string FirmOrder { get; set; }
        public string FirmStatus { get; set; }
    }
}
