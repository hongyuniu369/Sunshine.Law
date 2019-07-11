using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_userService : ServiceBase<sys_user>
    {
       
    }

    public class sys_user : ModelBase
    {
        [PrimaryKey]   
        public string UserCode { get; set; }
        public string UserSeq { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string OrganizeName { get; set; }
        public string ConfigJSON { get; set; }
        public bool? IsEnable { get; set; }
        public int? LoginCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Email { get; set; }
        public string IDCard { get; set; }
        public string ContactNO { get; set; }
        public string MobilePhone { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string DetailedAddress { get; set; }
        public string QQ { get; set; }
        public string MSN { get; set; }
        public string ZIPCode { get; set; }
        public string RegIP { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public int? FailedPasswordAttemptCount { get; set; }
        public DateTime? FailedPasswordAttemptWindowStart { get; set; }
        public int? FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
    }
}
