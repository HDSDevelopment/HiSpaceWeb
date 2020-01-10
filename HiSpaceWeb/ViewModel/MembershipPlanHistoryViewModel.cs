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
	public class MembershipPlanHistoryViewModel
    {
		public MembershipPlanHistoryViewModel()
		{
            membershipPlanHistory = new List<MembershipPlanHistoryResponse>();
            currentPlan = new MembershipPlanHistoryResponse();
        }
		
		public List<MembershipPlanHistoryResponse> membershipPlanHistory { set; get; }
        public MembershipPlanHistoryResponse currentPlan { set; get; }
    }
}