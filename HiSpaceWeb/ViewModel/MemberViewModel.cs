using HiSpaceModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.ViewModel
{
	public class MemberViewModel
	{
		public MemberViewModel()
		{
			MemberMaster = new MemberMaster();
			MembershipList = new List<MembershipVM>();
			UserLogin = new UserLogin();
		}
		public MemberMaster MemberMaster { get; set; }
		public IFormFile RCCopy { set; get; }
		public IFormFile ContactPersonAadhaar { set; get; }
		public IFormFile ContactPersonPAN { set; get; }
		public List<MembershipVM> MembershipList { set; get; }
		public UserLogin UserLogin { set; get; }

	}
	public class MembershipVM
	{
		public int ClientMembershipPlanID { get; set; }
		public string MembershipName { get; set; }
		public int? MembershipDuration { get; set; }
		public string MemberShipDurationType { get; set; }
		public double? Price { get; set; }
		public bool Selected { get; set; }
	}
}
