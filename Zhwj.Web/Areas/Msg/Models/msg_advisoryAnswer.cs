using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_advisoryAnswerService : ServiceBase<msg_advisoryAnswer>
    {
        protected override bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            if (arg.dataAction == OptType.Add)
            {
                arg.dataNew["AnswerCode"] = GetNewKey("AnswerCode", "dateplus");
                arg.dataNew["LawyerCode"] = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", Zephyr.Areas.Msg.MsgHelper.GetUserCode())).LawyerCode;
                arg.dataNew["AnswerDate"] = DateTime.Now;
                //更新咨询状态为已回复
                var sql = String.Format(@"
                    update msg_advisory 
                        set MsgStatus = '30',ReplyDate = '{1}'
                    where AdvisoryCode = {0}", arg.dataNew["AdvisoryCode"],DateTime.Now.ToString());
                arg.db.Sql(sql).Execute();
            }
            return base.OnBeforeEditPageForm(arg);
        }

        /// <summary>
        /// 设置最佳答复
        /// </summary>
        /// <param name="answerCode"></param>
        public void Best(string answerCode)
        {
            var advisoryCode = GetModel(ParamQuery.Instance().AndWhere("AnswerCode", answerCode)).AdvisoryCode;
            //评分数据
                var advisoryAnswer = base.GetModel(ParamQuery.Instance().AndWhere("AnswerCode", answerCode));
                var score = new sys_parameterService().GetModel(ParamQuery.Instance().AndWhere("ParamCode", "Score_Best"));

            db.UseTransaction(true);
            Logger("设置最佳答复", () =>
            {
                db.Update("msg_advisoryAnswer")
                    .Column("IsBestAnswer", 0)
                    .Column("SelectDate", DBNull.Value)
                    .Column("SelectUserCode", DBNull.Value)
                    .Where("AdvisoryCode", advisoryCode)
                    .Execute();
                db.Update("msg_advisoryAnswer")
                    .Column("IsBestAnswer", 1)
                    .Column("SelectDate", DateTime.Now)
                    .Column("SelectUserCode", Zephyr.Areas.Msg.MsgHelper.GetUserName())
                    .Where("AnswerCode", answerCode)
                    .Execute();
                //更新咨询状态为已办结
                var sql = String.Format(@"
                    update msg_advisory 
                        set MsgStatus = '99',BestDate = '{1}'
                    where AdvisoryCode = {0}", advisoryCode, DateTime.Now.ToString());
                db.Sql(sql).Execute();
                //评分处理
                sql = string.Format(@"
                    update law_lawyerScore 
                    set BestScore = 0
                    where MasterKey='msg_advisoryAnswer' 
                    and MasterValue in (select AnswerCode from msg_advisoryAnswer where AdvisoryCode = '{0}')
                    ", advisoryCode);
                db.Sql(sql).Execute();//清空该咨询下所有答复的择优评分
                db.Commit();

                new law_lawyerScoreService().EditScore(advisoryAnswer.LawyerCode, "msg_advisoryAnswer", answerCode, "BestScore", Convert.ToDecimal(score.ParamValue));

            }, e => db.Rollback());
        }
    }

    public class msg_advisoryAnswer : ModelBase
    {
        [PrimaryKey]
        public string AnswerCode { get; set; }
        public string AdvisoryCode { get; set; }
        public string LawyerCode { get; set; }
        public string AnswerContent { get; set; }
        public bool IsSecret { get; set; }
        public DateTime AnswerDate { get; set; }
        public bool IsBestAnswer { get; set; }
        public DateTime? SelectDate { get; set; }
        public string SelectUserCode { get; set; }
        public int? PraiseCount { get; set; }
        public int? AnswerOrder { get; set; }
        public string AnswerRemark { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
    }
}
