using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.Models
{
    public class LoggedUser
    {        
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }        
        public bool Active { get; set; }
        public int? MemberID { get; set; }
        public int? ClientID { get; set; }
        public int? ClientLocationID { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public long? LoginCount { get; set; }
    }
}
