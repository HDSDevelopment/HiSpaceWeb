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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using HiSpaceWeb.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.Controllers
{
    public class ClientSpaceController : Controller
    { // GET: Table
        private readonly IHostingEnvironment hostingEnvironment;

        public ClientSpaceController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            SetSessionVariables();

            var qryCLID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ClientLocationID = qryCLID;
            int ClientLocationID = Convert.ToInt32(qryCLID);
            var qryCFID = HttpContext.Request.Query["ClientFloorID"];
            ViewBag.ClientFloorID = qryCFID;
            int ClientFloorID = Convert.ToInt32(qryCFID);

            int? ClientID = 0;
            if (string.IsNullOrEmpty(HttpContext.Request.Query["ClientID"].ToString()))
                ClientID = GetSessionObject().ClientID;

            IEnumerable<SpaceBookingResponse> spaces = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientWorkSpaceFloorPlansByFilter + ClientID + "/" + ClientLocationID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    if (result.Content != null)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<SpaceBookingResponse>>();
                        readTask.Wait();

                        spaces = readTask.Result;
                    }
                }
                else //web api sent error response
                {
                    //log response status here..

                    spaces = Enumerable.Empty<SpaceBookingResponse>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(spaces);
        }

        // GET: Create
        public ActionResult Create()
        {
            SetSessionVariables();

            var qryCLID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ClientLocationID = qryCLID;
            int ClientLocationID = Convert.ToInt32(qryCLID);

            var qryCFD = HttpContext.Request.Query["ClientFloorID"];
            ViewBag.ClientFloorID = qryCFD;
            int ClientFloorID = Convert.ToInt32(qryCFD);

            ClientSpaceViewModel vModel = new ClientSpaceViewModel();

            ViewBag.ListOfSpaceTypes = Common.GetWorkSpaceTypeList();
            ViewBag.ListOfChairTypes = Common.GetChairTypeList();
            ViewBag.ListOfScaleMetrics = Common.GetScaleMetricList();
            ViewBag.ListOfStatus = Common.GetAvailableStatusList();
            ViewBag.ListOfApplyTo = Common.GetApplyTo();
            ViewBag.ListOfSeatStatus = Common.GetSeatStatus();
            ViewBag.ListOfFloors = Common.GetClientFloors(ClientLocationID);
            ViewBag.ListOfScheduleTime = Common.GetScheduleTime();
            List<ClientSpaceSeat> sessionSeatObject = new List<ClientSpaceSeat>();
            SetSeatListObject(sessionSeatObject);
            vModel.ClientSpaceFloorPlan.ClientLocationID = ClientLocationID;
            vModel.ClientFloorID = vModel.ClientSpaceFloorPlan.ClientFloorID = ClientFloorID;

            return View(vModel);
        }

        [HttpPost]
        public ActionResult Create(ClientSpaceViewModel model, IFormCollection formCollection)
        {
            SetSessionVariables();

            if (model != null)
            {
                model.ClientSpaceFloorPlan.CreatedBy = GetSessionObject().UserID;
                model.ClientSpaceFloorPlan.CreatedDateTime = DateTime.Now;
                //model.ClientLocationStatus = "Active";
                model.ClientSpaceFloorPlan.ClientFloorID = model.ClientFloorID;
                if (model.AllTimeCheck == true)
                {
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = true;
                    model.ClientSpaceFloorPlan.SunAvail = true;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.TueOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.WedOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.ThuOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.FriOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.SatOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.SunOpen = TimeSpan.Parse("00:00:00");

                    //close
                    model.ClientSpaceFloorPlan.MonClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.TueClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.WedClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.ThuClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.FriClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.SatClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.SunClose = TimeSpan.Parse("23:59:59");
                }
                else if (model.MonToFriCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = false;
                    model.ClientSpaceFloorPlan.SunAvail = false;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.TueOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.WedOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.ThuOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.FriOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.SatOpen = null;
                    model.ClientSpaceFloorPlan.SunOpen = null;

                    //close
                    model.ClientSpaceFloorPlan.MonClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.TueClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.WedClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.ThuClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.FriClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.SatClose = null;
                    model.ClientSpaceFloorPlan.SunClose = null;
                }
                else if (model.MonToSatCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = true;
                    model.ClientSpaceFloorPlan.SunAvail = false;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.TueOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.WedOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.ThuOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.FriOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.SatOpen = model.MonToFriWithSatOpen;
                    model.ClientSpaceFloorPlan.SunOpen = null;

                    //close
                    model.ClientSpaceFloorPlan.MonClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.TueClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.WedClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.ThuClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.FriClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.SatClose = model.MonToFriWithSatClose;
                    model.ClientSpaceFloorPlan.SunClose = null;
                }
                else if (model.CustomCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                }
            }
            model.ClientSpaceFloorPlan.ClientID = GetSessionObject().ClientID;
            //model.ClientSpaceFloorPlan.ClientLocationID = ClientLocationID;

            string DuplicateName = "";
            string OriginalName = "";

            string UploadRootPath = "Upload";
            string uploadsFolder = "\\client\\" + GetSessionObject().ClientID + "\\spaces\\";
            string serverUploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadRootPath);
            serverUploadsFolder += uploadsFolder;
            if (!Directory.Exists(serverUploadsFolder))
            {
                Directory.CreateDirectory(serverUploadsFolder);
            }

            //RCCopy image uploader
            if (model.FloorPlanFile != null)
            {
                OriginalName = model.FloorPlanFile.FileName;
                string extension = Path.GetExtension(OriginalName);
                DuplicateName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
                model.FloorPlanFile.CopyTo(new FileStream(filePath, FileMode.Create));
                model.ClientSpaceFloorPlan.FloorPlanFilePath = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
            }

            var WSpaceTypeID = HttpContext.Request.Form["WSpaceTypeID"].ToString();
            var ChairTypeID = HttpContext.Request.Form["ChairTypeID"].ToString();
            var ScaleMetricID = HttpContext.Request.Form["ScaleMetricID"].ToString();
            var StatusName = HttpContext.Request.Form["StatusName"].ToString();

            model.ClientSpaceFloorPlan.WSpaceTypeID = int.Parse(WSpaceTypeID);
            model.ClientSpaceFloorPlan.ChairTypeID = int.Parse(ChairTypeID);
            model.ClientSpaceFloorPlan.ScaleMetricID = int.Parse(ScaleMetricID);
            model.ClientSpaceFloorPlan.Status = StatusName;

            //var ClientFloorID = HttpContext.Request.Form["ClientFloorID"].ToString();
            //model.ClientSpaceFloorPlan.ClientFloorID = int.Parse(ClientFloorID);
            model.ClientSpaceFloorPlan.Verification = "Pending";

            ClientWorkSpaceFloorPlan NewSpace = new ClientWorkSpaceFloorPlan();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST ClientWorkSpaceFloorPlan
                var postTask = client.PostAsJsonAsync<ClientWorkSpaceFloorPlan>(Common.Instance.ApiClientAddClientWorkSpaceFloorPlan, model.ClientSpaceFloorPlan);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    readTask.Wait();

                    NewSpace = readTask.Result;

                    //return RedirectToAction("Index");
                }

                List<ClientSpaceSeat> seats = GetSeatListObject();
                foreach (var _seat in seats)
                    _seat.ClientSpaceFloorPlanID = NewSpace.ClientSpaceFloorPlanID;
                //HTTP GET
                postTask = client.PostAsJsonAsync<List<ClientSpaceSeat>>(Common.Instance.ApiClientUpdateClientSpaceSeats, seats);
                postTask.Wait();

                result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    //readTask.Wait();

                    //clientWorkSpaceFloorPlan = readTask.Result;
                }
            }

            //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return RedirectToAction("Index");
        }

        // GET: Edit
        [HttpGet]
        public ActionResult Edit(int ClientSpaceFloorPlanID)
        {
            SetSessionVariables();

            ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];

            var qryCLID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ClientLocationID = qryCLID;
            int ClientLocationID = Convert.ToInt32(qryCLID);

            var qryCFD = HttpContext.Request.Query["ClientFloorID"];
            ViewBag.ClientFloorID = qryCFD;
            int ClientFloorID = Convert.ToInt32(qryCFD);

            //var qryCWSID = HttpContext.Request.Query["ClientSpaceFloorPlanID"];
            //ViewBag.ClientSpaceFloorPlanID = qryCWSID;
            //int ClientSpaceFloorPlanID = Convert.ToInt32(qryCWSID);

            ViewBag.ListOfSpaceTypes = Common.GetWorkSpaceTypeList();
            ViewBag.ListOfChairTypes = Common.GetChairTypeList();
            ViewBag.ListOfScaleMetrics = Common.GetScaleMetricList();
            ViewBag.ListOfStatus = Common.GetAvailableStatusList();
            ViewBag.ListOfApplyTo = Common.GetApplyTo();
            ViewBag.ListOfSeatStatus = Common.GetSeatStatus();
            ViewBag.ListOfFloors = Common.GetClientFloors(ClientLocationID);

            ViewBag.ListOfScheduleTime = Common.GetScheduleTime();

            ClientSpaceViewModel vModel = new ClientSpaceViewModel();

            vModel.ClientSpaceFloorPlan.ClientLocationID = ClientLocationID;
            vModel.ClientFloorID = vModel.ClientSpaceFloorPlan.ClientFloorID = ClientFloorID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientWorkSpaceFloorPlan + ClientSpaceFloorPlanID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    readTask.Wait();

                    vModel.ClientSpaceFloorPlan = readTask.Result;
                    vModel.ChairTypeID = vModel.ClientSpaceFloorPlan.ChairTypeID;
                    vModel.WSpaceTypeID = vModel.ClientSpaceFloorPlan.WSpaceTypeID;
                    vModel.StatusName = vModel.ClientSpaceFloorPlan.Status;
                    vModel.ClientFloorID = vModel.ClientSpaceFloorPlan.ClientFloorID;

                    if (vModel.ClientSpaceFloorPlan.Is24 == true)
                    {
                        vModel.AllTimeCheck = true;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = false;
                    }
                    else if ((vModel.ClientSpaceFloorPlan.Is24 == false) && (vModel.ClientSpaceFloorPlan.MonAvail == true) && (vModel.ClientSpaceFloorPlan.TueAvail == true) && (vModel.ClientSpaceFloorPlan.WedAvail == true) && (vModel.ClientSpaceFloorPlan.ThuAvail == true) && (vModel.ClientSpaceFloorPlan.FriAvail == true) && (vModel.ClientSpaceFloorPlan.SatAvail == false) && (vModel.ClientSpaceFloorPlan.SunAvail == false) && (new[] { vModel.ClientSpaceFloorPlan.MonOpen, vModel.ClientSpaceFloorPlan.TueOpen, vModel.ClientSpaceFloorPlan.WedOpen, vModel.ClientSpaceFloorPlan.ThuOpen, vModel.ClientSpaceFloorPlan.FriOpen }.Contains(vModel.ClientSpaceFloorPlan.MonOpen)) && (new[] { vModel.ClientSpaceFloorPlan.MonClose, vModel.ClientSpaceFloorPlan.TueClose, vModel.ClientSpaceFloorPlan.WedClose, vModel.ClientSpaceFloorPlan.ThuClose, vModel.ClientSpaceFloorPlan.FriClose }.Contains(vModel.ClientSpaceFloorPlan.MonClose)))
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = true;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = false;
                        vModel.MonToFriOpen = vModel.ClientSpaceFloorPlan.MonOpen;
                        vModel.MonToFriClose = vModel.ClientSpaceFloorPlan.MonClose;
                    }
                    else if ((vModel.ClientSpaceFloorPlan.Is24 == false) && (vModel.ClientSpaceFloorPlan.MonAvail == true) && (vModel.ClientSpaceFloorPlan.TueAvail == true) && (vModel.ClientSpaceFloorPlan.WedAvail == true) && (vModel.ClientSpaceFloorPlan.ThuAvail == true) && (vModel.ClientSpaceFloorPlan.FriAvail == true) && (vModel.ClientSpaceFloorPlan.SatAvail == true) && (vModel.ClientSpaceFloorPlan.SunAvail == false) && (new[] { vModel.ClientSpaceFloorPlan.MonOpen, vModel.ClientSpaceFloorPlan.TueOpen, vModel.ClientSpaceFloorPlan.WedOpen, vModel.ClientSpaceFloorPlan.ThuOpen, vModel.ClientSpaceFloorPlan.FriOpen }.Contains(vModel.ClientSpaceFloorPlan.MonOpen)) && (new[] { vModel.ClientSpaceFloorPlan.MonClose, vModel.ClientSpaceFloorPlan.TueClose, vModel.ClientSpaceFloorPlan.WedClose, vModel.ClientSpaceFloorPlan.ThuClose, vModel.ClientSpaceFloorPlan.FriClose }.Contains(vModel.ClientSpaceFloorPlan.MonClose)))
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = true;
                        vModel.CustomCheck = false;
                        vModel.MonToFriNotSatOpen = vModel.ClientSpaceFloorPlan.MonOpen;
                        vModel.MonToFriNotSatClose = vModel.ClientSpaceFloorPlan.MonClose;
                        vModel.MonToFriWithSatOpen = vModel.ClientSpaceFloorPlan.SatOpen;
                        vModel.MonToFriWithSatClose = vModel.ClientSpaceFloorPlan.SatClose;
                    }
                    else
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = true;
                    }
                }

                responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceSeats + ClientSpaceFloorPlanID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ClientSpaceSeat>>();
                    readTask.Wait();
                    var seats = readTask.Result;
                    SetSeatListObject(seats);
                }
            }

            return View(vModel);
        }

        //[HttpPost]
        public IActionResult Edit(ClientSpaceViewModel model, IFormCollection formCollection)
        {
            SetSessionVariables();

            if (model != null)
            {
                model.ClientSpaceFloorPlan.ModifyBy = GetSessionObject().UserID;
                model.ClientSpaceFloorPlan.ModifyDateTime = DateTime.Now;
				

                if (model.AllTimeCheck == true)
                {
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = true;
                    model.ClientSpaceFloorPlan.SunAvail = true;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.TueOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.WedOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.ThuOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.FriOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.SatOpen = TimeSpan.Parse("00:00:00");
                    model.ClientSpaceFloorPlan.SunOpen = TimeSpan.Parse("00:00:00");

                    //close
                    model.ClientSpaceFloorPlan.MonClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.TueClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.WedClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.ThuClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.FriClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.SatClose = TimeSpan.Parse("23:59:59");
                    model.ClientSpaceFloorPlan.SunClose = TimeSpan.Parse("23:59:59");
                }
                else if (model.MonToFriCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = false;
                    model.ClientSpaceFloorPlan.SunAvail = false;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.TueOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.WedOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.ThuOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.FriOpen = model.MonToFriOpen;
                    model.ClientSpaceFloorPlan.SatOpen = null;
                    model.ClientSpaceFloorPlan.SunOpen = null;

                    //close
                    model.ClientSpaceFloorPlan.MonClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.TueClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.WedClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.ThuClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.FriClose = model.MonToFriClose;
                    model.ClientSpaceFloorPlan.SatClose = null;
                    model.ClientSpaceFloorPlan.SunClose = null;
                }
                else if (model.MonToSatCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                    model.ClientSpaceFloorPlan.MonAvail = true;
                    model.ClientSpaceFloorPlan.TueAvail = true;
                    model.ClientSpaceFloorPlan.WedAvail = true;
                    model.ClientSpaceFloorPlan.ThuAvail = true;
                    model.ClientSpaceFloorPlan.FriAvail = true;
                    model.ClientSpaceFloorPlan.SatAvail = true;
                    model.ClientSpaceFloorPlan.SunAvail = false;

                    //open
                    model.ClientSpaceFloorPlan.MonOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.TueOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.WedOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.ThuOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.FriOpen = model.MonToFriNotSatOpen;
                    model.ClientSpaceFloorPlan.SatOpen = model.MonToFriWithSatOpen;
                    model.ClientSpaceFloorPlan.SunOpen = null;

                    //close
                    model.ClientSpaceFloorPlan.MonClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.TueClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.WedClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.ThuClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.FriClose = model.MonToFriNotSatClose;
                    model.ClientSpaceFloorPlan.SatClose = model.MonToFriWithSatClose;
                    model.ClientSpaceFloorPlan.SunClose = null;
                }
                else if (model.CustomCheck == true)
                {
                    model.ClientSpaceFloorPlan.Is24 = false;
                }
            }
            model.ClientSpaceFloorPlan.ClientID = GetSessionObject().ClientID;
            //model.ClientSpaceFloorPlan.ClientLocationID = GetSessionObject().ClientLocationID;
            var WSpaceTypeID = HttpContext.Request.Form["WSpaceTypeID"].ToString();
            var ChairTypeID = HttpContext.Request.Form["ChairTypeID"].ToString();
            var ScaleMetricID = HttpContext.Request.Form["ScaleMetricID"].ToString();
            var StatusName = HttpContext.Request.Form["StatusName"].ToString();
            //var ClientFloorID = HttpContext.Request.Form["ClientFloorID"].ToString();
            model.ClientSpaceFloorPlan.WSpaceTypeID = int.Parse(WSpaceTypeID);
            model.ClientSpaceFloorPlan.ChairTypeID = int.Parse(ChairTypeID);
            model.ClientSpaceFloorPlan.ScaleMetricID = int.Parse(ScaleMetricID);
            model.ClientSpaceFloorPlan.Status = StatusName;
            //model.ClientSpaceFloorPlan.ClientFloorID = int.Parse(ClientFloorID);
            model.ClientSpaceFloorPlan.Verification = "Pending";

            string DuplicateName = "";
            string OriginalName = "";

            string UploadRootPath = "Upload";
            string uploadsFolder = "\\client\\" + GetSessionObject().ClientID + "\\spaces\\";
            string serverUploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadRootPath);
            serverUploadsFolder += uploadsFolder;
            if (!Directory.Exists(serverUploadsFolder))
            {
                Directory.CreateDirectory(serverUploadsFolder);
            }

            if (model.FloorPlanFile != null)
            {
                OriginalName = model.FloorPlanFile.FileName;
                string extension = Path.GetExtension(OriginalName);
                DuplicateName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
                try
                {
                    model.FloorPlanFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                catch (Exception ex) { }

                model.ClientSpaceFloorPlan.FloorPlanFilePath = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
            }

            ClientWorkSpaceFloorPlan clientWorkSpaceFloorPlan = new ClientWorkSpaceFloorPlan();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.PostAsJsonAsync<ClientWorkSpaceFloorPlan>(Common.Instance.ApiClientUpdateClientWorkSpaceFloorPlan, model.ClientSpaceFloorPlan);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    readTask.Wait();

                    clientWorkSpaceFloorPlan = readTask.Result;
                }

                List<ClientSpaceSeat> seats = GetSeatListObject();
                foreach (var _seat in seats)
                    _seat.ClientSpaceFloorPlanID = model.ClientSpaceFloorPlan.ClientSpaceFloorPlanID;
                //HTTP GET
                responseTask = client.PostAsJsonAsync<List<ClientSpaceSeat>>(Common.Instance.ApiClientUpdateClientSpaceSeats, seats);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    //readTask.Wait();

                    //clientWorkSpaceFloorPlan = readTask.Result;
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Display
        [HttpGet]
        public ActionResult Display(int ClientSpaceFloorPlanID)
        {
            SetSessionVariables();

            ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];

            var qryCLID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ClientLocationID = qryCLID;
            int ClientLocationID = Convert.ToInt32(qryCLID);

            var qryCFD = HttpContext.Request.Query["ClientFloorID"];
            ViewBag.ClientFloorID = qryCFD;
            int ClientFloorID = Convert.ToInt32(qryCFD);

            //var qryCWSID = HttpContext.Request.Query["ClientSpaceFloorPlanID"];
            //ViewBag.ClientSpaceFloorPlanID = qryCWSID;
            //int ClientSpaceFloorPlanID = Convert.ToInt32(qryCWSID);

            ViewBag.ListOfSpaceTypes = Common.GetWorkSpaceTypeList();
            ViewBag.ListOfChairTypes = Common.GetChairTypeList();
            ViewBag.ListOfScaleMetrics = Common.GetScaleMetricList();
            ViewBag.ListOfStatus = Common.GetAvailableStatusList();
            ViewBag.ListOfApplyTo = Common.GetApplyTo();
            ViewBag.ListOfSeatStatus = Common.GetSeatStatus();
            ViewBag.ListOfFloors = Common.GetClientFloors(ClientLocationID);
            ViewBag.ListOfScheduleTime = Common.GetScheduleTime();

            ClientSpaceViewModel vModel = new ClientSpaceViewModel();

            vModel.ClientSpaceFloorPlan.ClientLocationID = ClientLocationID;
            vModel.ClientFloorID = vModel.ClientSpaceFloorPlan.ClientFloorID = ClientFloorID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientWorkSpaceFloorPlan + ClientSpaceFloorPlanID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientWorkSpaceFloorPlan>();
                    readTask.Wait();

                    vModel.ClientSpaceFloorPlan = readTask.Result;
                    vModel.ChairTypeID = vModel.ClientSpaceFloorPlan.ChairTypeID;
                    vModel.WSpaceTypeID = vModel.ClientSpaceFloorPlan.WSpaceTypeID;
                    vModel.StatusName = vModel.ClientSpaceFloorPlan.Status;
                    vModel.ClientFloorID = vModel.ClientSpaceFloorPlan.ClientFloorID;

                    if (vModel.ClientSpaceFloorPlan.Is24 == true)
                    {
                        vModel.AllTimeCheck = true;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = false;
                    }
                    else if ((vModel.ClientSpaceFloorPlan.Is24 == false) && (vModel.ClientSpaceFloorPlan.MonAvail == true) && (vModel.ClientSpaceFloorPlan.TueAvail == true) && (vModel.ClientSpaceFloorPlan.WedAvail == true) && (vModel.ClientSpaceFloorPlan.ThuAvail == true) && (vModel.ClientSpaceFloorPlan.FriAvail == true) && (vModel.ClientSpaceFloorPlan.SatAvail == false) && (vModel.ClientSpaceFloorPlan.SunAvail == false) && (new[] { vModel.ClientSpaceFloorPlan.MonOpen, vModel.ClientSpaceFloorPlan.TueOpen, vModel.ClientSpaceFloorPlan.WedOpen, vModel.ClientSpaceFloorPlan.ThuOpen, vModel.ClientSpaceFloorPlan.FriOpen }.Contains(vModel.ClientSpaceFloorPlan.MonOpen)) && (new[] { vModel.ClientSpaceFloorPlan.MonClose, vModel.ClientSpaceFloorPlan.TueClose, vModel.ClientSpaceFloorPlan.WedClose, vModel.ClientSpaceFloorPlan.ThuClose, vModel.ClientSpaceFloorPlan.FriClose }.Contains(vModel.ClientSpaceFloorPlan.MonClose)))
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = true;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = false;
                        vModel.MonToFriOpen = vModel.ClientSpaceFloorPlan.MonOpen;
                        vModel.MonToFriClose = vModel.ClientSpaceFloorPlan.MonClose;
                    }
                    else if ((vModel.ClientSpaceFloorPlan.Is24 == false) && (vModel.ClientSpaceFloorPlan.MonAvail == true) && (vModel.ClientSpaceFloorPlan.TueAvail == true) && (vModel.ClientSpaceFloorPlan.WedAvail == true) && (vModel.ClientSpaceFloorPlan.ThuAvail == true) && (vModel.ClientSpaceFloorPlan.FriAvail == true) && (vModel.ClientSpaceFloorPlan.SatAvail == true) && (vModel.ClientSpaceFloorPlan.SunAvail == false) && (new[] { vModel.ClientSpaceFloorPlan.MonOpen, vModel.ClientSpaceFloorPlan.TueOpen, vModel.ClientSpaceFloorPlan.WedOpen, vModel.ClientSpaceFloorPlan.ThuOpen, vModel.ClientSpaceFloorPlan.FriOpen }.Contains(vModel.ClientSpaceFloorPlan.MonOpen)) && (new[] { vModel.ClientSpaceFloorPlan.MonClose, vModel.ClientSpaceFloorPlan.TueClose, vModel.ClientSpaceFloorPlan.WedClose, vModel.ClientSpaceFloorPlan.ThuClose, vModel.ClientSpaceFloorPlan.FriClose }.Contains(vModel.ClientSpaceFloorPlan.MonClose)))
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = true;
                        vModel.CustomCheck = false;
                        vModel.MonToFriNotSatOpen = vModel.ClientSpaceFloorPlan.MonOpen;
                        vModel.MonToFriNotSatClose = vModel.ClientSpaceFloorPlan.MonClose;
                        vModel.MonToFriWithSatOpen = vModel.ClientSpaceFloorPlan.SatOpen;
                        vModel.MonToFriWithSatClose = vModel.ClientSpaceFloorPlan.SatClose;
                    }
                    else
                    {
                        vModel.AllTimeCheck = false;
                        vModel.MonToFriCheck = false;
                        vModel.MonToSatCheck = false;
                        vModel.CustomCheck = true;
                    }
                }

                responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceSeats + ClientSpaceFloorPlanID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ClientSpaceSeat>>();
                    readTask.Wait();
                    var seats = readTask.Result;
                    SetSeatListObject(seats);
                }
            }

            return View(vModel);
        }

        [HttpGet]
        public ActionResult Approve(int ClientSpaceFloorPlanID, string Status)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientApproveSpace + ClientSpaceFloorPlanID + "/" + Status);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rs = result.Content.ReadAsAsync<bool>().Result;
                    var sr = rs;
                }
            }
            return RedirectToAction("Index", new { ClientID = 0, ClientLocationID = 0 });
        }

        [HttpPost]
        public IActionResult AddSeats(List<ClientSpaceSeat> SeatList, string SelectedSeatId)
        {
            SetSessionVariables();

            List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
            if (sessionSeatObject == null) sessionSeatObject = new List<ClientSpaceSeat>();
            foreach (var seat in SeatList)
            {
                if (!sessionSeatObject.Any(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord))
                    sessionSeatObject.Add(seat);
                //int selectedX = Convert.ToInt32(SelectedSeatId.Split(':')[0]);
                //int selectedY = Convert.ToInt32(SelectedSeatId.Split(':')[1]);
                //var selectedSeat = SeatList.SingleOrDefault(d => d.SeatXCoord == selectedX && d.SeatYCoord == selectedY);
                //if (selectedSeat != null)
                //    sessionSeatObject.Add(selectedSeat);
            }

            SetSeatListObject(sessionSeatObject);

            return Ok(new { Name = "OK", Message = "Added" });
        }

        [HttpPost]
        public IActionResult RemoveSeats(List<ClientSpaceSeat> SeatList, string SelectedSeatId)
        {
            SetSessionVariables();

            List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
            if (sessionSeatObject == null) sessionSeatObject = new List<ClientSpaceSeat>();

            List<ClientSpaceSeat> tempSeatObject = new List<ClientSpaceSeat>();

            foreach (var seat in sessionSeatObject)
            {
                var isSeatExist = SeatList.SingleOrDefault(d => d.SeatXCoord == seat.SeatXCoord && d.SeatYCoord == seat.SeatYCoord);
                if (isSeatExist == null)
                    tempSeatObject.Add(seat);
            }

            SetSeatListObject(tempSeatObject);

            return Ok(new { Name = "OK", Message = "Removed" });
        }

        public List<ClientSpaceSeat> GetClientSeats()
        {
            SetSessionVariables();

            List<ClientSpaceSeat> sessionSeatObject = GetSeatListObject();
            if (sessionSeatObject == null) sessionSeatObject = new List<ClientSpaceSeat>();

            return sessionSeatObject;
        }

        [HttpGet]
        public ActionResult ReadClientSeats()
        {
            SetSessionVariables();

            List<ClientSpaceSeat> sessionSeatObject = GetClientSeats();

            return Json(sessionSeatObject);
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
            HttpContext.Session.SetObjectAsJson("_seatList", seats);
        }

        public List<ClientSpaceSeat> GetSeatListObject()
        {
            return HttpContext.Session.GetObjectFromJson<List<ClientSpaceSeat>>("_seatList");
        }
    }
}