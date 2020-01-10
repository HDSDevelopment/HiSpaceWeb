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
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.ViewModel
{
    public class ListWorkSpaceViewModel
    {
        public ListWorkSpaceViewModel()
        {
            ClientLocationSearchList = new List<ClientLocationSearchResponse>();
            AmenitySearchList = new List<AmenitiesSearchResponse>();
            WorkSpaceTypeSearchList = new List<WorkSpaceTypeSearchResponse>();
        }
        
        public List<ClientLocationSearchResponse> ClientLocationSearchList { set; get; }
        public List<AmenitiesSearchResponse> AmenitySearchList { set; get; }        
        public List<WorkSpaceTypeSearchResponse> WorkSpaceTypeSearchList { set; get; }
        public List<SpaceDetailsListSearchResponse> WorkSpaceSearchList { set; get; }


        public IFormFile FloorPlanFile { set; get; }

    }
}
