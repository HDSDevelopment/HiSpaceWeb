using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.Models
{
    [Serializable]
    public class SingleSeat
    {        
        //public int ClientSpaceSeatID { set; get; }
        //public int ClientSpaceFloorPlanID { set; get; }
        public int SeatXCoord { set; get; }
        public int SeatYCoord { set; get; }
        public string SeatDescription { set; get; }
        public double SeatPrice { set; get; }
        public string SeatStatus { set; get; }        
    }
}
