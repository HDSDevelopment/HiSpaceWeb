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

	public class MemberBookingSpaceSeatViewModel
	{
		public MemberBookingSpaceSeatViewModel()
		{
			
		}
		
		public int MemberBookingSpaceSeatID { get; set; }
		public int? MemberBookingSpaceID { get; set; }
		public int? ClientSpaceSeatID { get; set; }
		public double? SeatPrice { get; set; }
		public string SeatStatus { get; set; }
		public int? CreatedBy { get; set; }		
		public string SeatName { set; get; }
		public string SeatDesc { set; get; }
		public string SeatId { set; get; }
		public string SeatMatrix { set; get; }
	}

}
