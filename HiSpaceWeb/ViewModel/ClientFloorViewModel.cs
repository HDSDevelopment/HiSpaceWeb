using HiSpaceModels;
using HiSpaceWeb.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HiSpaceWeb.ViewModel
{
	public class ClientFloorViewModel
	{
		public ClientFloorViewModel()
		{
			ClientFloor = new ClientFloor();
			FacilityCatList = new List<FacilityCatVM>();
		}

        public int FloorNumber { set; get; }
        public ClientFloor ClientFloor { set; get; }
		public IFormFile FloorPlanFilePath { set; get; }
		public List<FacilityCatVM> FacilityCatList { set; get; }
	}
}