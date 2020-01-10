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
using Microsoft.AspNetCore.Http;
using HiSpaceWeb.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HiSpaceWeb.Controllers
{
    public class MemberController : Controller
    {

        private readonly IHostingEnvironment hostingEnvironment;

        public MemberController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Client
        [HttpGet]
        public ActionResult Index()
        {
            SetSessionVariables();
           
            IEnumerable<MemberMaster> clients = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiMemberGetMembersByClientID + GetSessionObject().ClientID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MemberMaster>>();
                    readTask.Wait();

                    clients = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    clients = Enumerable.Empty<MemberMaster>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clients);
        }

        public ActionResult Create()
        {
            SetSessionVariables();

            return View();
        }

        [HttpPost]
        public ActionResult Create(MemberViewModel model)
        {
            SetSessionVariables();

            if (model != null)
            {
                MemberMaster NewMember = new MemberMaster();
                UserLogin userLogin = new UserLogin();

                if (model.MemberMaster.ClientMembershipPlanID == null)
                {
                    model.MemberMaster.ClientMembershipPlanID = 0;
                }
                int? Exdate = model.MemberMaster.ClientMembershipPlanID;
                var ExDateValue = Exdate.Value;
                model.MemberMaster.CreatedBy = GetSessionObject().UserID;
                model.MemberMaster.MembershipStartedDate = DateTime.Now;
                model.MemberMaster.MembershipExpiryDate = DateTime.Now.AddMonths(ExDateValue);
                model.MemberMaster.MembershipPriceOnDate = 0;
                var RenewalAlertDate = DateTime.Now.AddMonths(ExDateValue);
                model.MemberMaster.RenewalAlertDate = RenewalAlertDate.AddDays(-1);

                model.MemberMaster.ClientID = ViewBag.ClientID;

                string DuplicateName = "";
                string OriginalName = "";

                //RCCopy image uploader
                if (model.RCCopy != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    OriginalName = model.RCCopy.FileName;
                    string extension = Path.GetExtension(OriginalName);
                    DuplicateName = "_RCCopy" + extension;

                    string filePath = Path.Combine(uploadsFolder, DuplicateName);
                    model.RCCopy.CopyTo(new FileStream(filePath, FileMode.Create));
                    model.MemberMaster.Doc_RCCopy = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
                }

                //ContactPersonAadhaar image uploader
                if (model.ContactPersonAadhaar != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    OriginalName = model.ContactPersonAadhaar.FileName;
                    string extension = Path.GetExtension(OriginalName);
                    DuplicateName = "_ContactPersonAadhaar" + extension;

                    string filePath = Path.Combine(uploadsFolder, DuplicateName);
                    model.ContactPersonAadhaar.CopyTo(new FileStream(filePath, FileMode.Create));
                    model.MemberMaster.Doc_ContactPersonAadhaar = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
                }

                //ContactPersonPAN image uploader
                if (model.ContactPersonPAN != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    OriginalName = model.ContactPersonPAN.FileName;
                    string extension = Path.GetExtension(OriginalName);
                    DuplicateName = "_ContactPersonPAN" + extension;

                    string filePath = Path.Combine(uploadsFolder, DuplicateName);
                    model.ContactPersonPAN.CopyTo(new FileStream(filePath, FileMode.Create));
                    model.MemberMaster.Doc_ContactPersonPAN = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<MemberMaster>(Common.Instance.ApiMemberAddMemberMaster, model.MemberMaster);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rs = result.Content.ReadAsAsync<MemberMaster>().Result;
                        //var rs = result.Content.ReadAsAsync<MemberMaster>();
                        ////return RedirectToAction("Index");
                        //NewMember = rs.Result;
                        model.UserLogin.MemberID = rs.MemberID;

                    }

                    model.UserLogin.Username = model.MemberMaster.MemberUsername;
                    model.UserLogin.Password = model.MemberMaster.MemberPassword;
                    model.UserLogin.Active = model.MemberMaster.MemberStatus;
                    model.UserLogin.UserType = 4;
                    model.UserLogin.ClientID = ViewBag.ClientID;

                    //userLogin section added
                    //HTTP POST
                    client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                    postTask = client.PostAsJsonAsync<UserLogin>(Common.Instance.ApiUserLoginAddUserLogin, model.UserLogin);
                    postTask.Wait();

                    result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rs = result.Content.ReadAsAsync<UserLogin>();
                        //return RedirectToAction("Index");
                        userLogin = rs.Result;
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int MemberID)
        {
            SetSessionVariables();

            MemberViewModel vModel = new MemberViewModel();
            List<MembershipVM> MembershipVMList = GetMembershipList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiMemberGetMember + MemberID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<MemberMaster>();
                    readTask.Wait();
                    vModel.MemberMaster = readTask.Result;
                }
                var membershipID = vModel.MemberMaster.ClientMembershipPlanID;
                responseTask = client.GetAsync(Common.Instance.ApiMemberGetMembershipByMemberID + membershipID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ClientMembershipPlan>>();
                    readTask.Wait();
                    var lst = readTask.Result.ToList();

                    vModel.MembershipList = new List<MembershipVM>();

                    foreach (var item in lst)
                    {
                        var fac = MembershipVMList.SingleOrDefault(d => d.ClientMembershipPlanID == item.ClientMembershipPlanID);
                    }
                }
            }

            vModel.MembershipList = MembershipVMList;
            return View(vModel);
        }

        [HttpPost]
        public ActionResult Edit(MemberViewModel model)
        {
            SetSessionVariables();


            MemberMaster MemberMaster = null;
            string DuplicateName = "";
            string OriginalName = "";

            //RCCopy image uploader
            if (model.RCCopy != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                OriginalName = model.RCCopy.FileName;
                string extension = Path.GetExtension(OriginalName);
                DuplicateName = "_RCCopy" + extension;

                string filePath = Path.Combine(uploadsFolder, DuplicateName);
                model.RCCopy.CopyTo(new FileStream(filePath, FileMode.Create));
                model.MemberMaster.Doc_RCCopy = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
            }

            //ContactPersonAadhaar image uploader
            if (model.ContactPersonAadhaar != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                OriginalName = model.ContactPersonAadhaar.FileName;
                string extension = Path.GetExtension(OriginalName);
                DuplicateName = "_ContactPersonAadhaar" + extension;

                string filePath = Path.Combine(uploadsFolder, DuplicateName);
                model.ContactPersonAadhaar.CopyTo(new FileStream(filePath, FileMode.Create));
                model.MemberMaster.Doc_ContactPersonAadhaar = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
            }

            //ContactPersonPAN image uploader
            if (model.ContactPersonPAN != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                uploadsFolder += "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                OriginalName = model.ContactPersonPAN.FileName;
                string extension = Path.GetExtension(OriginalName);
                DuplicateName = "_ContactPersonPAN" + extension;

                string filePath = Path.Combine(uploadsFolder, DuplicateName);
                model.ContactPersonPAN.CopyTo(new FileStream(filePath, FileMode.Create));
                model.MemberMaster.Doc_ContactPersonPAN = "\\" + "img" + "\\" + "member" + "\\" + model.MemberMaster.MemberName + "\\" + DuplicateName;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.PutAsJsonAsync(Common.Instance.ApiMemberUpdateMemberMaster + model.MemberMaster.MemberID, model.MemberMaster);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<MemberMaster>();
                    readTask.Wait();

                    MemberMaster = readTask.Result;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Display(int MemberID)
        {

            SetSessionVariables();

            MemberViewModel vModel = new MemberViewModel();
            List<MembershipVM> MembershipVMList = GetMembershipList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiMemberGetMember + MemberID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<MemberMaster>();
                    readTask.Wait();
                    vModel.MemberMaster = readTask.Result;

                }
                var membershipID = vModel.MemberMaster.ClientMembershipPlanID;
                responseTask = client.GetAsync(Common.Instance.ApiMemberGetMembershipByMemberID + membershipID.ToString());
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ClientMembershipPlan>>();
                    readTask.Wait();
                    var lst = readTask.Result.ToList();

                    vModel.MembershipList = new List<MembershipVM>();

                    foreach (var item in lst)
                    {
                        var fac = MembershipVMList.SingleOrDefault(d => d.ClientMembershipPlanID == item.ClientMembershipPlanID);
                    }
                }
            }

            vModel.MembershipList = MembershipVMList;
            return View(vModel);
        }

        private List<MembershipVM> GetMembershipList()
        {
            SetSessionVariables();

            List<MembershipVM> membershipPlans = new List<MembershipVM>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiCommonGetMembershipPlans);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientMembershipPlan>>();
                    readTask.Wait();

                    foreach (var item in readTask.Result.ToList())
                        membershipPlans.Add(new MembershipVM() { ClientMembershipPlanID = item.ClientMembershipPlanID, MembershipName = item.MembershipName, MembershipDuration = item.MembershipDuration, MemberShipDurationType = item.MembershipDurationType, Price = item.Price, Selected = false });
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //spaces = Enumerable.Empty<WSpaceType>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return membershipPlans;
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