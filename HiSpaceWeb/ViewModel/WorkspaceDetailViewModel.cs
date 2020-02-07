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
	public class WorkspaceDetailViewModel
	{
		public WorkspaceDetailViewModel()
		{
			WorkSpaceDetails = new WorkSpaceDetailsResponse();
		}

		public WorkSpaceDetailsResponse WorkSpaceDetails { set; get; }

		public IFormFile FloorPlanFile { set; get; }
		public TimeSpan? OpenTime { get; set; }
		public TimeSpan? CloseTime { get; set; }
	}
}