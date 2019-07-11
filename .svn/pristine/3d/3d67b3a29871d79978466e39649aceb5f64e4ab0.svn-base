using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class law_lawyerScoreService : ServiceBase<law_lawyerScore>
    {
        public string GetScoreCode(string masterKey, string masterValue)
        {
            var pQuery = ParamQuery.Instance()
                .Select("ScoreCode")
                .AndWhere("MasterKey", masterKey)
                .AndWhere("MasterValue", masterValue);

            return base.GetField<string>(pQuery);
            //var sql = string.Format(@"Select ScoreCode " + 
            //    "From law_lawyerScore " + 
            //    "Where MasterKey='{0}' And MasterValue='{1}'", masterKey, masterValue);
            //return db.Sql(sql).QuerySingle<law_lawyerScore>().LawyerCode;
        }

        public int EditScore(string lawyerCode, string masterKey, string masterValue, string fieldName, decimal score)
        {
            var scoreCode = GetScoreCode(masterKey, masterValue);
            if (!string.IsNullOrEmpty(scoreCode))
            {
                var pQuery = ParamUpdate.Instance()
                    .Column(fieldName, score)
                    .AndWhere("ScoreCode", scoreCode);
                return base.Update(pQuery);
            }
            else
            {
                var pQuery = ParamInsert.Instance()
                    .Column("ScoreCode", base.GetNewKey("ScoreCode", "dateplus"))
                    .Column("LawyerCode", lawyerCode)
                    .Column("MasterKey", masterKey)
                    .Column("MasterValue", masterValue)
                    .Column(fieldName, score);
                return base.Insert(pQuery);
            }
        }
    }

    public class law_lawyerScore : ModelBase
    {
        [PrimaryKey]   
        public string ScoreCode { get; set; }
        public string LawyerCode { get; set; }
        public string MasterKey { get; set; }
        public string MasterValue { get; set; }
        public decimal? WebScore { get; set; }
        public decimal? AnswerScore { get; set; }
        public decimal? BestScore { get; set; }
        public decimal? AdminScore { get; set; }
        public string ScoreRemark { get; set; }
    }
}
