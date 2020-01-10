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
	public class AttendanceViewModel
	{
		public AttendanceViewModel()
		{
		
			AttendanceList = new List<Attendance>();
			//Employee = new EmployeeMaster();

		}

		
		public List<Attendance> AttendanceList { set; get; }
		public int EmpID { set; get; }
		public DateTime? FromDate { set; get; }
		public DateTime? ToDate { set; get; }

	}
}