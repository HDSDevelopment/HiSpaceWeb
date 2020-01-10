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
using HiSpaceWeb.ViewModel;
using HiSpaceService.ViewModel;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace HiSpaceWeb.Controllers
{
    public class SpaceBookingController : Controller
    {
        private object hostingEnvironment;

        // GET: Table

        public ActionResult Index()
        {
			SetSessionVariables();

            ViewBag.ListOfLocations = Common.GetLocationList();
            SpaceBookingViewModel vModel = new SpaceBookingViewModel();
            return View(vModel);
        }
        // GET: Table
        [HttpPost]
        public ActionResult Index(SpaceBookingViewModel vModel)
		{
			SetSessionVariables();

			ViewBag.ListOfLocations = Common.GetLocationList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                //HTTP GET
                int ClientLocationID = vModel.ClientLocationID;
                var responseTask = client.GetAsync(Common.Instance.ApiSpaceBookingGetBookingSpaces + ClientLocationID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<SpaceBookingResponse>>();
                    readTask.Wait();

                    vModel.BookingSpaces = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..


                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View("Index", vModel);
        }

        [HttpGet]
        public ActionResult BookSpace(int SpaceFloorPlanID)
		{
			SetSessionVariables();

			ViewBag.ListOfSpaceTypes = Common.GetWorkSpaceTypeList();
            ViewBag.ListOfChairTypes = Common.GetChairTypeList();
            ViewBag.ListOfScaleMetrics = Common.GetScaleMetricList();
            ViewBag.ListOfStatus = Common.GetAvailableStatusList();
            ViewBag.ListOfApplyTo = Common.GetApplyTo();
            ViewBag.ListOfSeatStatus = Common.GetSeatStatus();

            SpaceBookingViewModel vModel = new SpaceBookingViewModel();
            List<FacilityVM> FacilityVMList = Common.GetFacilityList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientWorkSpaceFloorPlan + SpaceFloorPlanID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    readTask.Wait();

                    vModel.SpaceFloorPlan = readTask.Result;
                    vModel.ChairTypeID = vModel.SpaceFloorPlan.ChairTypeID;
                    vModel.WSpaceTypeID = vModel.SpaceFloorPlan.WSpaceTypeID;
                    vModel.StatusName = vModel.SpaceFloorPlan.Status;
                }

                responseTask = client.GetAsync(Common.Instance.ApiGetClientSpaceFacilitiesByClientSpace + SpaceFloorPlanID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ClientFacility>>();
                    readTask.Wait();
                    var lst = readTask.Result.ToList();

                    vModel.FacilityList = new List<FacilityVM>();

                    foreach (var item in lst)
                    {
                        var fac = FacilityVMList.SingleOrDefault(d => d.FacilityID == item.FacilityID);
                        if (fac != null)
                        {
                            if (item.Available)
                                fac.Selected = true;
                            else
                                fac.Selected = false;
                        }
                    }
                    vModel.FacilityList = FacilityVMList;
                }

                responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceSeats + SpaceFloorPlanID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ClientSpaceSeat>>();
                    readTask.Wait();
                    var seats = readTask.Result;

                    SelectedSeats.SeatsSpaceBook = new List<ClientSpaceSeat>();
                    SelectedSeats.SeatsSpaceBook.Clear();
                    foreach (var item in seats)
                    {
                        if (item.SeatPrice == null)
                            item.SeatPrice = 0;
                        if (item.SeatDescription == null)
                            item.SeatDescription = "";
                        SelectedSeats.SeatsSpaceBook.Add(item);
                    }
                }
            }

            return View(vModel);
        }

        [HttpPost]
        public ActionResult BookSpace(SpaceBookingViewModel model, IFormCollection formCollection)
		{
			SetSessionVariables();

			if (model == null) return View(model);
            if (model.SpaceFloorPlan == null) return View(model);
            if (model.SpaceFloorPlan.ClientSpaceFloorPlanID == 0) return View(model);

            //var objComplex = HttpContext.Session.GetObject("ComplexObject");
            //var myComplexTestObject = HttpContext.Session.GetObjectFromJson<ClientSpaceViewModel>("seat");
            ViewBag.complex = HttpContext.Session.GetObjectFromJson<List<ClientSpaceSeat>>("Seats");

            var cnt = SelectedSeats.SeatsSpaceBook.Count();

            //ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];

            if (model != null)
            {
                model.SpaceFloorPlan.ModifyBy = 1;
                model.SpaceFloorPlan.ModifyDateTime = DateTime.Now;
            }
            model.SpaceFloorPlan.ClientID = 1;
            model.SpaceFloorPlan.ClientLocationID = 1;// ViewBag.ClientLocationID;
            //var WSpaceTypeID = HttpContext.Request.Form["WSpaceTypeID"].ToString();
            //var ChairTypeID = HttpContext.Request.Form["ChairTypeID"].ToString();
            //var ScaleMetricID = HttpContext.Request.Form["ScaleMetricID"].ToString();
            //var StatusName = HttpContext.Request.Form["StatusName"].ToString();
            //model.SpaceFloorPlan.WSpaceTypeID = int.Parse(WSpaceTypeID);
            //model.SpaceFloorPlan.ChairTypeID = int.Parse(ChairTypeID);
            //model.SpaceFloorPlan.ScaleMetricID = int.Parse(ScaleMetricID);
            //model.SpaceFloorPlan.Status = StatusName;

            ClientWorkSpaceFloorPlan clientWorkSpaceFloorPlan = model.SpaceFloorPlan;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                
                //Delete 
                //if (SelectedSeats.SeatsToRemove != null)
                //{
                //    foreach (var _seat in SelectedSeats.SeatsToRemove)
                //        _seat.ClientSpaceFloorPlanID = model.SpaceFloorPlan.ClientSpaceFloorPlanID;
                //    responseTask = client.PutAsJsonAsync(Common.Instance.ApiClientDeleteClientSpaceSeats, SelectedSeats.SeatsToRemove);
                //    responseTask.Wait();

                //    result = responseTask.Result;
                //    if (result.IsSuccessStatusCode)
                //    {
                //        //var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                //        //readTask.Wait();

                //        //clientWorkSpaceFloorPlan = readTask.Result;
                //    }
                //}

                foreach (var _seat in SelectedSeats.SeatsSpaceBook)
                    _seat.ClientSpaceFloorPlanID = model.SpaceFloorPlan.ClientSpaceFloorPlanID;
                //HTTP GET
                var responseTask = client.PutAsJsonAsync(Common.Instance.ApiClientUpdateClientSpaceSeats, SelectedSeats.SeatsSpaceBook);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    //readTask.Wait();

                    //clientWorkSpaceFloorPlan = readTask.Result;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddSeats(List<ClientSpaceSeat> SeatList)
		{
			SetSessionVariables();

			if (SelectedSeats.SeatsSpaceBook == null)
                SelectedSeats.SeatsSpaceBook = new List<ClientSpaceSeat>();

            foreach (var seat in SeatList)
            {
                var _seat = SelectedSeats.SeatsSpaceBook.SingleOrDefault(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord);
                if (_seat == null)
                    SelectedSeats.SeatsSpaceBook.Add(seat);
                else
                {
                    _seat.SeatDescription = seat.SeatDescription;
                    _seat.SeatStatus = seat.SeatStatus;
                    _seat.SeatPrice = seat.SeatPrice;
                }
            }
            return Ok(new { Name = "OK", Message = "Added" });
        }

        [HttpPost]
        public IActionResult RemoveSeats(List<ClientSpaceSeat> SeatList)
		{
			SetSessionVariables();

			if (SelectedSeats.SeatsSpaceBook == null)
                SelectedSeats.SeatsSpaceBook = new List<ClientSpaceSeat>();

            if (SelectedSeats.SeatsToRemoveSpaceBook == null)
                SelectedSeats.SeatsToRemoveSpaceBook = new List<ClientSpaceSeat>();

            //foreach (var seat in SeatList)
            //{
            //    var _seat = SelectedSeats.Seats.SingleOrDefault(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord);
            //    if (_seat != null)
            //    {
            //        _seat.ClientSpaceFloorPlanID = SeatList[0].ClientSpaceFloorPlanID;
            //        SelectedSeats.Seats.Remove(_seat);
            //        SelectedSeats.SeatsToRemove.Add(_seat);
            //    }
            //}
            return Ok(new { Name = "OK", Message = "Removed" });
        }

        [HttpGet]
        public ActionResult<List<ClientSpaceSeat>> GetClientSeats()
		{
			SetSessionVariables();

			if (SelectedSeats.SeatsSpaceBook == null)
                SelectedSeats.SeatsSpaceBook = new List<ClientSpaceSeat>();

            return SelectedSeats.SeatsSpaceBook;
        }

		public void SetSessionVariables()
		{
			#region
			ViewBag.Username = HttpContext.Session.GetString(Common.SessionUsername);
			ViewBag.Type = HttpContext.Session.GetInt32(Common.SessionType);
			ViewBag.UserID = HttpContext.Session.GetInt32(Common.SessionUserID);
			ViewBag.ClientID = HttpContext.Session.GetInt32(Common.SessionClientID);
			ViewBag.MemberID = HttpContext.Session.GetInt32(Common.SessionMemberID);
			ViewBag.ClientLocationID = HttpContext.Session.GetInt32(Common.SessionClientLocationID);
			#endregion
		}

        public UserLogin GetSessionObject()
        {
            return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
        }

    }
}