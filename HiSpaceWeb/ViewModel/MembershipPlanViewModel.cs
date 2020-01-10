using HiSpaceModels;
using HiSpaceWeb.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HiSpaceWeb.ViewModel
{
	public class MembershipPlanViewModel
    {
		public MembershipPlanViewModel()
		{
            clientMemPlan = new ClientMembershipPlan();
		}

        public string MembershipDurationType { set; get; }
        public bool IsActive { set; get; }
        public bool IsRecommented { set; get; }
        public ClientMembershipPlan clientMemPlan { set; get; }
	}
}