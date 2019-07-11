using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Law")]
    public class msg_entrustAnswerService : ServiceBase<msg_entrustAnswer>
    {
        protected override bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            if (arg.dataAction == OptType.Add)
            {
                arg.dataNew["AnswerCode"] = GetNewKey("AnswerCode", "dateplus");
                arg.dataNew["LawyerCode"] = new law_lawyerService().GetModel(ParamQuery.Instance().AndWhere("UserCode", Zephyr.Areas.Msg.MsgHelper.GetUserCode())).LawyerCode;
                arg.dataNew["AnswerDate"] = DateTime.Now;
                //更新委托状态为已回复
                var sql = String.Format(@"
                    update msg_entrust 
                        set MsgStatus = '30',ReplyDate = '{1}'
                    where EntrustCode = {0}", arg.dataNew["EntrustCode"], DateTime.Now.ToString());
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
            var entrustCode = GetModel(ParamQuery.Instance().AndWhere("AnswerCode", answerCode)).EntrustCode;
            //评分数据
            var entrustAnswer = base.GetModel(ParamQuery.Instance().AndWhere("AnswerCode", answerCode));
            var score = new sys_parameterService().GetModel(ParamQuery.Instance().AndWhere("ParamCode", "Score_Best"));

            db.UseTransaction(true);
            Logger("设置最佳答复", () =>
            {
                db.Update("msg_entrustAnswer")
                    .Column("IsBestAnswer", 0)
                    .Column("SelectDate", DateTime.Now)
                    .Column("SelectUserCode", Zephyr.Areas.Msg.MsgHelper.GetUserName())
                    .Where("EntrustCode", entrustCode)
                    .Execute();
                db.Update("msg_entrustAnswer")
                    .Column("IsBestAnswer", 1)
                    .Column("SelectDate", DateTime.Now)
                    .Column("SelectUserCode", Zephyr.Areas.Msg.MsgHelper.GetUserName())
                    .Where("AnswerCode", answerCode)
                    .Execute();
                //更新咨询状态为已办结
                var sql = String.Format(@"
                    update msg_entrust 
                        set MsgStatus = '99',ReplyDate = '{1}'
                    where EntrustCode = {0}", entrustCode, DateTime.Now.ToString());
                db.Sql(sql).Execute();
                //评分处理  评分暂时不使用
                /*
                sql = string.Format(@"
                    update law_lawyerScore 
                    set BestScore = 0
                    where MasterKey='msg_entrustAnswer' 
                    and MasterValue in (select AnswerCode from msg_entrustAnswer where EntrustCode = '{0}')
                    ", entrustCode);
                db.Sql(sql).Execute();//清空该委托下所有答复的择优评分
                //经测试事务有效
                new law_lawyerScoreService().EditScore(entrustAnswer.LawyerCode, "msg_entrustAnswer", answerCode, "BestScore", Convert.ToDecimal(score.ParamValue));
                */
                db.Commit();
            }, e => db.Rollback());
        }
    }

    public class msg_entrustAnswer : ModelBase
    {
        [PrimaryKey]
        public string AnswerCode { get; set; }
        public string EntrustCode { get; set; }
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
