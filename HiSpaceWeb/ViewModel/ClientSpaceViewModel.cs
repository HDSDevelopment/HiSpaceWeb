using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using HiSpaceModels;
using HiSpaceWeb.Utilities;
using HiSpaceWeb.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HiSpaceWeb.ViewModel
{

    public class ClientSpaceViewModel
    {
        private SessionUtility session;

        public ClientSpaceViewModel()
        {
            ClientSpaceFloorPlan = new ClientWorkSpaceFloorPlan();
            FacilityList = new List<FacilityVM>();
            SelectedSeats = new List<ClientSpaceSeat>();
        }
        public ClientWorkSpaceFloorPlan ClientSpaceFloorPlan { set; get; }
              
        public int? ClientFloorID { set; get; }
        public int? WSpaceTypeID { set; get; }
        public int? ChairTypeID { get; set; }
        public int? ScaleMetricID { get; set; }
        public string StatusName { get; set; }
        public int? ApplyToID { set; get; }
        public int? SeatStatusID { set; get; }
        public double? SeatPrice { set; get; }
        public string SeatDescription { set; get; }
        public List<ClientSpaceSeat> SelectedSeats { set; get; }

        public List<FacilityVM> FacilityList { set; get; }

        public int SeatXCoord { set; get; }
        public int SeatYCoord { set; get; }
        //public string SeatDescription { set; get; }
        //public double? SeatPrice { set; get; }
        public string SeatStatus { set; get; }

        public IFormFile FloorPlanFile { set; get; }
    }
   
}
