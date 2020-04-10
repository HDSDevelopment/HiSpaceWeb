using HiSpaceModels;
using HiSpaceService.ViewModel;
using HiSpaceWeb.Utilities;
using HiSpaceWeb.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HiSpaceWeb.Controllers
{
	public class WebsiteController : Controller
	{
		public ActionResult Index()
		{
			SetSessionVariables();

			return View();
		}

		public ActionResult About()
		{
			SetSessionVariables();

			return View();
		}

		public ActionResult Logout()
		{
			ApplicationState.Instance.CartSpaces.Clear();
			ApplicationState.Instance.CartSeats.Clear();
			UserLogin session = null;
			HttpContext.Session.SetObjectAsJson("_user", session);
			return View();
		}

		public ActionResult Payment()
		{
			SetSessionVariables();

			var id = Request.HttpContext.Request.Query["ClientMemberPlanID"];

			int _id = Convert.ToInt32(id);

			return View();
		}

		public ActionResult AddWorkspace()
		{
			SetSessionVariables();

			return View();
		}

		public ActionResult ListWorkspace()
		{
			SetSessionVariables();

			ListWorkSpaceViewModel vModel = new ListWorkSpaceViewModel();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetAllClientLocationSearch);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<ClientLocationSearchResponse>>();
					readTask.Wait();
					vModel.ClientLocationSearchList = readTask.Result;
				}

				responseTask = client.GetAsync(Common.Instance.ApiCommonGetAllAmenitiesSearch);
				responseTask.Wait();

				result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<AmenitiesSearchResponse>>();
					readTask.Wait();
					vModel.AmenitySearchList = readTask.Result;
				}

				responseTask = client.GetAsync(Common.Instance.ApiCommonGetAllWorkSpaceTypesSearch);
				responseTask.Wait();

				result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<WorkSpaceTypeSearchResponse>>();
					readTask.Wait();
					vModel.WorkSpaceTypeSearchList = readTask.Result;
				}
			}

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientWorkSpaceFloorPlanList);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<SpaceDetailsListSearchResponse>>();
					readTask.Wait();
					vModel.WorkSpaceSearchList = readTask.Result;
				}
			}

			return View(vModel);
		}

		[HttpGet]
		public ActionResult WorkspaceDetail(int ClientSpaceFloorPlanID)
		{
			SetSessionVariables();

			ApplicationState.Instance.CartSeats.Clear();
			ApplicationState.Instance.CartSpaces.Clear();
			ApplicationState.Instance.ReadClientSeats.Clear();
			ApplicationState.Instance.ClientID = 0;
			ApplicationState.Instance.ClientLocationID = 0;

			WorkspaceDetailViewModel vModel = new WorkspaceDetailViewModel();
			ViewBag.ListOfScheduleTime = Common.GetScheduleTime();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetWorkSpaceDetails + ClientSpaceFloorPlanID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<WorkSpaceDetailsResponse>();
					readTask.Wait();
					vModel.WorkSpaceDetails = readTask.Result;
					ViewBag.ClientID = vModel.WorkSpaceDetails.selectedSpace.ClientID;
					ViewBag.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID;
					ApplicationState.Instance.ClientID = vModel.WorkSpaceDetails.selectedSpace.ClientID;
					ApplicationState.Instance.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID;
				}

				responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceSeats + ClientSpaceFloorPlanID.ToString());
				responseTask.Wait();

				result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<ClientSpaceSeat>>();
					readTask.Wait();
					var seats = readTask.Result;
					//SetSeatListObject(seats);
					ApplicationState.Instance.ReadClientSeats = seats;
				}
			}

			return View(vModel);
		}

		public ActionResult ViewCartWorkspace()
		{
			SetSessionVariables();

			return View();
		}

		[HttpGet]
		public ActionResult GetFacilitiesBySpace(int ClientSpaceFloorPlanID)
		{
			//if (GetSessionObject() == null)
			//    return NoContent();
			//SetSessionVariables();

			List<FacilityMaster> facilities = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetFacilitiesBySpace + ClientSpaceFloorPlanID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<FacilityMaster>>();
					readTask.Wait();

					facilities = readTask.Result;
				}
			}
			return Json(facilities);
		}

		[HttpPost]
		public IActionResult AddSeats(List<MemberBookingSpaceSeatViewModel> SeatList)
		{
			SetSessionVariables();

			//List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
			List<MemberBookingSpace> sessionSpaceObject = ApplicationState.Instance.CartSpaces;
			List<MemberBookingSpaceSeat> sessionSeatObject = ApplicationState.Instance.CartSeats;
			List<MemberBookingSpaceSeatViewModel> sessionSeatObjectViewModel = ApplicationState.Instance.CartSeatsViewModel;

			if (sessionSpaceObject == null) sessionSpaceObject = new List<MemberBookingSpace>();
			if (sessionSeatObject == null) sessionSeatObject = new List<MemberBookingSpaceSeat>();
			if (sessionSeatObjectViewModel == null) sessionSeatObjectViewModel = new List<MemberBookingSpaceSeatViewModel>();

			//SeatList.GroupBy(d => d.)

			foreach (var seat in SeatList)
			{
				//if (!sessionSeatObject.Any(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord))
				if (!sessionSpaceObject.Any(d => d.ClientSpaceFloorPlanID == seat.MemberBookingSpaceID))
				{
					sessionSpaceObject.Add(new MemberBookingSpace()
					{
						ClientSpaceFloorPlanID = seat.MemberBookingSpaceID
						//MemberBookingSpaceID=seat.MemberBookingSpaceID.Value
					});
				}
				//if (!sessionSeatObject.Any(d => d.ClientSpaceSeatID == seat.ClientSpaceSeatID && d.MemberBookingSpaceID == seat.MemberBookingSpaceID))
				if (!sessionSeatObject.Any(d => d.ClientSpaceSeatID == seat.ClientSpaceSeatID))
				{
					sessionSeatObject.Add(new MemberBookingSpaceSeat()
					{
						MemberBookingSpaceID = seat.MemberBookingSpaceID,
						ClientSpaceSeatID = seat.ClientSpaceSeatID,
						SeatPrice = seat.SeatPrice,
						SeatStatus = seat.SeatStatus,
						FromDateTime = seat.FromDateTime,
						ToDateTime = seat.ToDateTime
					});
					//sessionSeatObject.Add(seat);
				}

				if (!sessionSeatObjectViewModel.Any(d => d.ClientSpaceSeatID == seat.ClientSpaceSeatID))
				{
					//sessionSeatObjectViewModel.Add(new MemberBookingSpaceSeat()
					//{
					//	MemberBookingSpaceID = seat.MemberBookingSpaceID,
					//	ClientSpaceSeatID = seat.ClientSpaceSeatID,
					//	SeatPrice = seat.SeatPrice,
					//	SeatStatus = seat.SeatStatus
					//});
					sessionSeatObjectViewModel.Add(seat);
				}
				//int selectedX = Convert.ToInt32(SelectedSeatId.Split(':')[0]);
				//int selectedY = Convert.ToInt32(SelectedSeatId.Split(':')[1]);
				//var selectedSeat = SeatList.SingleOrDefault(d => d.SeatXCoord == selectedX && d.SeatYCoord == selectedY);
				//if (selectedSeat != null)
				//    sessionSeatObject.Add(selectedSeat);
			}

			//SetSeatListObject(sessionSeatObject);
			ApplicationState.Instance.CartSpaces = sessionSpaceObject;
			ApplicationState.Instance.CartSeats = sessionSeatObject;
			ApplicationState.Instance.CartSeatsViewModel = sessionSeatObjectViewModel;

			return Ok(new { Name = "OK", Message = "Added" });
		}

		[HttpPost]
		public IActionResult RemoveSeats(List<MemberBookingSpaceSeatViewModel> SeatList)
		{
			SetSessionVariables();

			//List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
			List<MemberBookingSpaceSeat> sessionSeatObject = ApplicationState.Instance.CartSeats;
			if (sessionSeatObject == null) sessionSeatObject = new List<MemberBookingSpaceSeat>();

			List<MemberBookingSpaceSeat> tempSeatObject = new List<MemberBookingSpaceSeat>();

			foreach (var seat in sessionSeatObject)
			{
				//var isSeatExist = SeatList.SingleOrDefault(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord);
				var isSeatExist = SeatList.SingleOrDefault(d => d.ClientSpaceSeatID == seat.ClientSpaceSeatID);
				if (isSeatExist == null)
					tempSeatObject.Add(seat);
			}

			//SetSeatListObject(tempSeatObject);
			ApplicationState.Instance.CartSeats = tempSeatObject;

			return Ok(new { Name = "OK", Message = "Removed" });
		}

		[HttpGet]
		public ActionResult GetSelectedSeats()
		{
			SetSessionVariables();

			List<MemberBookingSpaceSeatViewModel> sessionSeatObjectViewModel = ApplicationState.Instance.CartSeatsViewModel;

			return Json(sessionSeatObjectViewModel);
		}

		public List<ClientSpaceSeat> GetClientSeats()
		{
			SetSessionVariables();

			//List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
			List<ClientSpaceSeat> sessionSeatObject = ApplicationState.Instance.ReadClientSeats;
			if (sessionSeatObject == null) sessionSeatObject = new List<ClientSpaceSeat>();

			return sessionSeatObject;
		}

		[HttpGet]
		public ActionResult ReadClientSeats(int ClientSpaceFloorPlanID)
		{
			SetSessionVariables();

			//List<ClientSpaceSeat> sessionSeatObject = GetClientSeats();
			List<ClientSpaceSeat> sessionSeatObject = new List<ClientSpaceSeat>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceSeats + ClientSpaceFloorPlanID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<ClientSpaceSeat>>();
					readTask.Wait();
					var seats = readTask.Result;
					//SetSeatListObject(seats);
					sessionSeatObject = ApplicationState.Instance.ReadClientSeats = seats;
				}
			}
			return Json(sessionSeatObject);
		}

		[HttpGet]
		public ActionResult GetClientSpaceFloorPlanByID(int ClientSpaceFloorPlanID)
		{
			List<ClientWorkSpaceFloorPlan> clientWorkSpaceFloorPlan = new List<ClientWorkSpaceFloorPlan>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiGetClientSpaceFloorPlanByID + ClientSpaceFloorPlanID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<ClientWorkSpaceFloorPlan>>();
					readTask.Wait();
					clientWorkSpaceFloorPlan = readTask.Result;
				}
			}

			return Json(clientWorkSpaceFloorPlan);
		}

		public void SetSessionVariables()
		{
			#region
			ViewBag.Username = HttpContext.Session.GetString(Common.SessionUsername);
			ViewBag.Type = HttpContext.Session.GetInt32(Common.SessionType);
			ViewBag.UserID = HttpContext.Session.GetInt32(Common.SessionUserID);
			ViewBag.ClientID = HttpContext.Session.GetInt32(Common.SessionClientID);
			ViewBag.MemberID = HttpContext.Session.GetInt32(Common.SessionMemberID);
			//ViewBag.ClientLocationID = HttpContext.Session.GetInt32(Common.SessionClientLocationID);
			#endregion
		}

		public UserLogin GetSessionObject()
		{
			return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
		}

		public void SetSeatListObject(List<ClientSpaceSeat> seats)
		{
			//HttpContext.Session.SetObjectAsJson("_seatListWebsite", seats);
			ApplicationState.Instance.ReadClientSeats = seats;
		}

		public List<ClientSpaceSeat> GetSeatListObject()
		{
			//return HttpContext.Session.GetObjectFromJson<List<ClientSpaceSeat>>("_seatListWebsite");
			return ApplicationState.Instance.ReadClientSeats;
		}
	}
}