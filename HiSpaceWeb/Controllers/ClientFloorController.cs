using HiSpaceModels;
using HiSpaceWeb.Utilities;
using HiSpaceWeb.ViewModel;
using HiSpaceService.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace HiSpaceWeb.Controllers
{
    public class ClientFloorController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ClientFloorController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Table
        public ActionResult Index(int ClientLocationID)
        {
            SetSessionVariables();
            var User = HttpContext.Session.GetObjectFromJson<UserLogin>("UserLoggedObject");
            var userid = HttpContext.Session.GetInt32("UserID");

            //ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];

            IEnumerable<ClientFloor> Floors = null;

            using (var client = new HttpClient())
            {
                //int ClientLocationID = 0;
                //if (ViewBag.ClientLocationID != null)
                //ClientLocationID = 1;// int.Parse(ViewBag.ClientLocationID);

                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientFloors + ClientLocationID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    if (result.Content != null)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<ClientFloor>>();
                        readTask.Wait();

                        Floors = readTask.Result;
                    }
                }
                else //web api sent error response
                {
                    //log response status here..

                    Floors = Enumerable.Empty<ClientFloor>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(Floors);
        }

        // GET: Create
        public ActionResult Create()
        {
            SetSessionVariables();
            ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ListOfFloors = Common.GetFloorNumbers();
            int? ClientLocationID = Convert.ToInt32(ViewBag.ClientLocationID);

            ClientFloorViewModel vModel = new ClientFloorViewModel
            {
                FacilityCatList = Common.GetFacilityCatList(),
                ClientFloor = new ClientFloor() { ClientLocationID = ClientLocationID }
            };
            return View(vModel);
        }

        [HttpPost]
        public ActionResult Create(ClientFloorViewModel model, IFormCollection formCollection)
        {
            SetSessionVariables();
            //ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];

            if (model != null)
            {
                model.ClientFloor.CreatedBy = GetSessionObject().UserID;
                model.ClientFloor.Active = true;
                model.ClientFloor.ClientID = GetSessionObject().ClientID;
                model.ClientFloor.ClientLocationID = model.ClientFloor.ClientLocationID;// int.Parse(ViewBag.ClientLocationID);
                model.FacilityCatList = Common.GetFacilityCatList();
                List<ClientFacility> facilityList = new List<ClientFacility>();

                string DuplicateName = "";
                string OriginalName = "";

                for (int i = 0; i < model.FacilityCatList.Count(); i++)
                {
                    for (int j = 0; j < model.FacilityCatList[i].FacilityList.Count(); j++)
                    {
                        ClientFacility faci = new ClientFacility();
                        string ck_name_Available = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].Selected";
                        string ck_name_IsPaid = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].IsPaidAmenity";
                        string txt_PaidAmenityPrice = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].PaidAmenityPrice";
                        var ckAvailable = formCollection[ck_name_Available];
                        var ckIsPaid = formCollection[ck_name_IsPaid];
                        var txtPaidAmenityPrice = formCollection[txt_PaidAmenityPrice];

                        faci.ClientID = model.ClientFloor.ClientID;
                        faci.ClientLocationID = model.ClientFloor.ClientLocationID;
                        faci.FacilityID = model.FacilityCatList[i].FacilityList[j].FacilityID;
                        faci.Available = (ckAvailable.Count == 2 ? true : false);
                        faci.IsPaidAmenity = (ckIsPaid.Count == 2 ? true : false);
                        faci.PaidAmenityPrice = (ckIsPaid.Count == 2 ? Convert.ToDouble(txtPaidAmenityPrice) : 0);
                        if (faci.Available)
                            facilityList.Add(faci);

                    }

                }

                ClientFloorRequest FloorRequest = new ClientFloorRequest();
                FloorRequest.clientFacilities = facilityList;

                string UploadRootPath = "Upload";
                string uploadsFolder = "\\client\\" + GetSessionObject().ClientID + "\\FloorPlanImages\\";
                string serverUploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadRootPath);
                serverUploadsFolder += uploadsFolder;
                if (!Directory.Exists(serverUploadsFolder))
                {
                    Directory.CreateDirectory(serverUploadsFolder);
                }

                //RCCopy image uploader
                if (model.FloorPlanFilePath != null && model.ClientFloor.FloorName != null)
                {
                    OriginalName = model.FloorPlanFilePath.FileName;
                    string extension = Path.GetExtension(OriginalName);
                    DuplicateName = Guid.NewGuid().ToString() + extension;

                    string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
                    model.FloorPlanFilePath.CopyTo(new FileStream(filePath, FileMode.Create));
                    model.ClientFloor.FloorPlanFilePath = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
                }

                model.ClientFloor.FloorNumber = model.FloorNumber;
                FloorRequest.clientFloor = model.ClientFloor;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                    //HTTP POST ClientFloor
                    var postTask = client.PostAsJsonAsync<ClientFloorRequest>(Common.Instance.ApiClientUpdateClientFloor, FloorRequest);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<bool>();
                        readTask.Wait();

                        //if(readTask.Result);                        
                    }
                }

                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return RedirectToAction("Index", new { ClientLocationID = model.ClientFloor.ClientLocationID });

        }

        public ActionResult Edit(int ClientFloorID)
        {
            SetSessionVariables();
            GetSessionObject();
            ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ListOfFloors = Common.GetFloorNumbers();
            ClientFloorViewModel vModel = new ClientFloorViewModel();
            vModel.ClientFloor = new ClientFloor();
            vModel.FacilityCatList = Common.GetFacilityCatList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST ClientFloor
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientFloorDetails + ClientFloorID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientFloor>();
                    readTask.Wait();
                    vModel.ClientFloor = readTask.Result;
                    vModel.FloorNumber = vModel.ClientFloor.FloorNumber.Value;
                }

                responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceFacilities + vModel.ClientFloor.ClientID + "/" + ClientFloorID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ClientFacility>>();
                    readTask.Wait();
                    List<ClientFacility> facs = readTask.Result;

                    foreach (var item in facs)
                    {
                        //bool found = false;
                        foreach (var grp in vModel.FacilityCatList)
                        {
                            var fac = grp.FacilityList.SingleOrDefault(g => g.FacilityID == item.FacilityID);
                            if (fac != null)
                            {
                                if (item.FacilityID == fac.FacilityID)
                                {
                                    fac.Selected = true;
                                    fac.IsPaidAmenity = item.IsPaidAmenity;
                                    fac.PaidAmenityPrice = item.PaidAmenityPrice;
                                }
                            }
                        }


                    }

                }

            }

            return View(vModel);
        }

        // GET: Edit
        [HttpPost]
        public ActionResult Edit(ClientFloorViewModel model, IFormCollection formCollection)
        {
            if (model != null)
            {
                model.ClientFloor.Active = true;
                model.ClientFloor.ClientID = GetSessionObject().ClientID;
                model.ClientFloor.ClientLocationID = model.ClientFloor.ClientLocationID;// int.Parse(ViewBag.ClientLocationID);
                model.FacilityCatList = Common.GetFacilityCatList();
                List<ClientFacility> facilityList = new List<ClientFacility>();

                string DuplicateName = "";
                string OriginalName = "";

                for (int i = 0; i < model.FacilityCatList.Count(); i++)
                {
                    for (int j = 0; j < model.FacilityCatList[i].FacilityList.Count(); j++)
                    {
                        ClientFacility faci = new ClientFacility();
                        string ck_name_Available = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].Selected";
                        string ck_name_IsPaid = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].IsPaidAmenity";
                        string txt_PaidAmenityPrice = "FacilityCatList[" + i.ToString() + "].FacilityList[" + j.ToString() + "].PaidAmenityPrice";
                        var ckAvailable = formCollection[ck_name_Available];
                        var ckIsPaid = formCollection[ck_name_IsPaid];
                        var txtPaidAmenityPrice = formCollection[txt_PaidAmenityPrice];

                        faci.ClientID = model.ClientFloor.ClientID;
                        faci.ClientLocationID = model.ClientFloor.ClientLocationID;
                        faci.FacilityID = model.FacilityCatList[i].FacilityList[j].FacilityID;
                        faci.Available = (ckAvailable.Count == 2 ? true : false);
                        faci.IsPaidAmenity = (ckIsPaid.Count == 2 ? true : false);
                        faci.PaidAmenityPrice = (ckIsPaid.Count == 2 ? Convert.ToDouble(txtPaidAmenityPrice) : 0);
                        if (faci.Available)
                            facilityList.Add(faci);

                    }

                }

                ClientFloorRequest FloorRequest = new ClientFloorRequest();
                FloorRequest.clientFacilities = facilityList;

                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Upload");
                uploadsFolder += "\\" + "client" + "\\" + GetSessionObject().ClientID + "\\FloorPlanImages\\";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                //RCCopy image uploader
                if (model.FloorPlanFilePath != null && model.ClientFloor.FloorName != null)
                {
                    OriginalName = model.FloorPlanFilePath.FileName;
                    string extension = Path.GetExtension(OriginalName);
                    DuplicateName = Guid.NewGuid() + extension;

                    string filePath = Path.Combine(uploadsFolder, DuplicateName);
                    model.FloorPlanFilePath.CopyTo(new FileStream(filePath, FileMode.Create));
                    model.ClientFloor.FloorPlanFilePath = uploadsFolder + DuplicateName;
                }

                //model.ClientFloor.FloorNumber = model.FloorNumber;
                FloorRequest.clientFloor = model.ClientFloor;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                    //HTTP POST ClientFloor
                    var postTask = client.PostAsJsonAsync<ClientFloorRequest>(Common.Instance.ApiClientUpdateClientFloor, FloorRequest);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<bool>();
                        readTask.Wait();

                        //if(readTask.Result);                        
                    }
                }

                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return RedirectToAction("Index", new { ClientLocationID = model.ClientFloor.ClientLocationID });
        }

        public ActionResult Display(int ClientFloorID)
        {
            SetSessionVariables();
            GetSessionObject();
            ViewBag.ClientLocationID = HttpContext.Request.Query["ClientLocationID"];
            ViewBag.ListOfFloors = Common.GetFloorNumbers();
            ClientFloorViewModel vModel = new ClientFloorViewModel();
            vModel.ClientFloor = new ClientFloor();
            vModel.FacilityCatList = Common.GetFacilityCatList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST ClientFloor
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientFloorDetails + ClientFloorID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientFloor>();
                    readTask.Wait();
                    vModel.ClientFloor = readTask.Result;
                    vModel.FloorNumber = vModel.ClientFloor.FloorNumber.Value;
                }

                responseTask = client.GetAsync(Common.Instance.ApiClientGetClientSpaceFacilities + vModel.ClientFloor.ClientID + "/" + ClientFloorID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ClientFacility>>();
                    readTask.Wait();
                    List<ClientFacility> facs = readTask.Result;

                    foreach (var item in facs)
                    {
                        //bool found = false;
                        foreach (var grp in vModel.FacilityCatList)
                        {
                            var fac = grp.FacilityList.SingleOrDefault(g => g.FacilityID == item.FacilityID);
                            if (fac != null)
                            {
                                if (item.FacilityID == fac.FacilityID)
                                {
                                    fac.Selected = true;
                                    fac.IsPaidAmenity = item.IsPaidAmenity;
                                    fac.PaidAmenityPrice = item.PaidAmenityPrice;
                                }
                            }
                        }


                    }

                }

            }

            return View(vModel);
        }

        public void SetSessionVariables()
        {
            #region
            ViewBag.Username = HttpContext.Session.GetString(Common.SessionUsername);
            ViewBag.Type = HttpContext.Session.GetInt32(Common.SessionType);
            ViewBag.UserID = HttpContext.Session.GetInt32(Common.SessionUserID);
            ViewBag.ClientID = HttpContext.Session.GetInt32(Common.SessionClientID);
            ViewBag.MemberID = HttpContext.Session.GetInt32(Common.SessionMemberID);
            ViewBag.ClientLocationID = Request.Query["ClientLocationID"];
            #endregion
        }

        public UserLogin GetSessionObject()
        {
            return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
        }

    }
}