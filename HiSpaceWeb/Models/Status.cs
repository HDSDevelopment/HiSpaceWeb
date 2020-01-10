using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.Models
{
    public class Status
    {
        public int StatusID { set; get; }
        public string StatusName { set; get; }
    }

    public class ApplyTo
    {
        public int ApplyToID { set; get; }
        public string ApplyToName { set; get; }
    }

    public class SeatStatus
    {
        public int SeatStatusID { set; get; }
        public string SeatStatusName { set; get; }
    }

    public class Floor
    {
        public int FloorNumber { set; get; }
    }
}
