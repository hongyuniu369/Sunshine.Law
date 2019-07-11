using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class law_lawyerService : ServiceBase<law_lawyer>
    {
        /// <summary>
        /// 获取所有msg_class 并标记改LawyerCode的msg_class为已选择
        /// </summary>
        /// <param name="LawyerCode"></param>
        /// <returns></returns>
        public dynamic GetLawyerClasses(string lawyerCode)
        {
            var sql = String.Format(@"
select A.*,
    case when B.LawyerCode is null then 0 else 1 end as Selected
from msg_class A
    left join law_lawyerClassMap B on B.lawyerCode = '{0}' and B.ClassCode = A.ClassCode
where A.IsEnable = 1 and (A.ParentClassCode is not null and A.ParentClassCode != '') 
order by ClassOrder", lawyerCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        /// <summary>
        /// 获取该律所的msg_class
        /// </summary>
        /// <param name="lawyerCode"></param>
        /// <returns></returns>
        public dynamic GetLawyerClass(string lawyerCode)
        {
            var sql = String.Format(@"
select A.*
from msg_class A
    right join law_lawyerClassMap B on B.lawyerCode = '{0}' and B.ClassCode = A.ClassCode
where A.IsEnable = 1  
order by ClassOrder", lawyerCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        public void SavelawyerClasses(string lawyerCode, JToken classList)
        {
            db.UseTransaction(true);
            Logger("设置律师业务专长", () =>
            {
                db.Delete("law_lawyerClassMap").Where("lawyerCode", lawyerCode).Execute();
                foreach (JToken item in classList.Children())
                {
                    var classCode = item["ClassCode"].ToString();
                    var lawyerClassMapCode = Zephyr.Core.NewKey.dateplus(db, "law_lawyerClassMap", "lawyerClassMapCode", "yyyyMMdd", 4);
                    db.Insert("law_lawyerClassMap").Column("lawyerClassMapCode", lawyerClassMapCode).Column("LawyerCode", lawyerCode).Column("ClassCode", classCode).Execute();
                }
                db.Commit();
            }, e => db.Rollback());
        }
        
        /// <summary>
        /// 获取律师，及擅长领域，所属机构等所有资料
        /// </summary>
        /// <param name="lawyerCode"></param>
        /// <returns></returns>
        public dynamic GetLawyer(string lawyerCode){
            dynamic Lawyer = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("LawyerCode", lawyerCode));

            var sql = String.Format(@"
select A.*
from    v_lawyerScore A
where   A.LawyerCode = '{0}'", lawyerCode);
            var result = db.Sql(sql).QuerySingle<dynamic>();

            Lawyer=Lawyer.Extend(new { MsgClasses = "",FirmName="", TotalScore=result.TotalScore});
            dynamic classes = new law_lawyerService().GetLawyerClass(lawyerCode);
            string msgClasses = "";
            foreach (var presentClass in classes)
            {
                msgClasses = msgClasses + presentClass.ClassName + ',';

            }
            if (msgClasses.Substring(msgClasses.Length - 1, 1) == ",")
            {
                msgClasses = msgClasses.Substring(0, msgClasses.Length - 1);
            }
            Lawyer.MsgClasses = msgClasses;
            Lawyer.FirmName = new law_firmService().GetModel(ParamQuery.Instance().AndWhere("FirmCode", Lawyer.FirmCode)).FirmName;
            return Lawyer;
        }
        /// <summary>
        /// 获取该律师下的代理，前20个
        /// </summary>
        /// <param name="lawyerCode"></param>
        /// <returns></returns>
        public dynamic GetAdvisorys(string lawyerCode) {
            var sql = String.Format(@"
select top 20 AdvisoryCode,MsgTitle,MsgInDate
    from msg_advisory
where LawyerCode='{0}'
order by AdvisoryCode desc", lawyerCode);
            var result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }
    }

    public class law_lawyer : ModelBase
    {
        [PrimaryKey]   
        public string LawyerCode { get; set; }
        public string UserCode { get; set; }
        public string FirmCode { get; set; }
        public string LawyerName { get; set; }
        public string LicenseNumber { get; set; }
        public string LawyerPic { get; set; }
        public string ContactNO { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string ProfessionalTitle { get; set; }
        public string Address { get; set; }
        public string DetailAddress { get; set; }
        public string LawyerDescription { get; set; }
        public string LawyerRemark { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? LawyerOrder { get; set; }
        public string LawyerStatus { get; set; }
    }
}
