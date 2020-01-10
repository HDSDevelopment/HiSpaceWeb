using HiSpaceModels;
using HiSpaceService.ViewModel;
using HiSpaceWeb.Utilities;
using HiSpaceWeb.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HiSpaceWeb.Controllers
{
    public class MembershipPlanController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public MembershipPlanController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            SetSessionVariables();

            IEnumerable<ClientMembershipPlan> clientMemPlans = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientMembershipPlans + "/" + GetSessionObject().ClientID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientMembershipPlan>>();
                    readTask.Wait();

                    clientMemPlans = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    clientMemPlans = Enumerable.Empty<ClientMembershipPlan>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clientMemPlans);
        }

        public ActionResult Create()
        {
            SetSessionVariables();
            ViewBag.ListOfMembershipDurationTypes = Common.GetMembershipDurationTypes();
            MembershipPlanViewModel model = new MembershipPlanViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MembershipPlanViewModel model)
        {
            SetSessionVariables();

            if (model.MembershipDurationType == "Select Plan")
            {
                return RedirectToAction("Index");
            }

            model.clientMemPlan.ClientID = GetSessionObject().ClientID;
            model.clientMemPlan.CreatedBy = GetSessionObject().UserID;
            model.clientMemPlan.CreatedDateTime = DateTime.Now;
            model.clientMemPlan.MembershipDurationType = model.MembershipDurationType;
            model.clientMemPlan.IsActive = model.IsActive;
            model.clientMemPlan.IsRecommented = model.IsRecommented;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ClientMembershipPlan>(Common.Instance.ApiClientAddEditClientMembershipPlan, model.clientMemPlan);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rs = result.Content.ReadAsAsync<ClientMembershipPlan>().Result;

                    ClientMembershipPlanHistory history = new ClientMembershipPlanHistory();
                    history.ClientMembershipPlanID = rs.ClientMembershipPlanID;
                    history.ClientID = rs.ClientID;
                    history.MembershipName = rs.MembershipName;
                    history.MembershipDuration = rs.MembershipDuration;
                    history.MembershipDurationType = rs.MembershipDurationType;
                    history.Price = rs.Price;
                    history.CreatedBy = rs.CreatedBy;
                    history.CreatedDateTime = rs.CreatedDateTime;

                    postTask = client.PostAsJsonAsync<ClientMembershipPlanHistory>(Common.Instance.ApiClientAddClientMembershipPlanHistory, history);
                    postTask.Wait();
                    result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rs1 = result.Content.ReadAsAsync<bool>().Result;
                    }

                }
            }

            return RedirectToAction("Index");

        }

        public ActionResult Edit(int ClientMembershipPlanID)
        {
            ViewBag.ListOfMembershipDurationTypes = Common.GetMembershipDurationTypes();
            SetSessionVariables();
            MembershipPlanViewModel model = new MembershipPlanViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientMembershipPlanDetails + "/" + ClientMembershipPlanID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientMembershipPlan>();
                    readTask.Wait();

                    model.clientMemPlan = readTask.Result;
                    model.MembershipDurationType = model.clientMemPlan.MembershipDurationType;
                    model.IsActive = model.clientMemPlan.IsActive.Value;
                    model.IsRecommented = model.clientMemPlan.IsRecommented.Value;
                }
                else //web api sent error response 
                {
                    //log response status here..



                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MembershipPlanViewModel model)
        {
            SetSessionVariables();

            if (model.MembershipDurationType == "Select Plan")
            {
                return RedirectToAction("Index");
            }

            model.clientMemPlan.ClientID = GetSessionObject().ClientID;
            model.clientMemPlan.MembershipDurationType = model.MembershipDurationType;
            model.clientMemPlan.ModifyBy = GetSessionObject().UserID;
            model.clientMemPlan.ModifyDateTime = DateTime.Now;
            model.clientMemPlan.IsActive = model.IsActive;
            model.clientMemPlan.IsRecommented = model.IsRecommented;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ClientMembershipPlan>(Common.Instance.ApiClientAddEditClientMembershipPlan, model.clientMemPlan);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rs = result.Content.ReadAsAsync<ClientMembershipPlan>().Result;

                    ClientMembershipPlanHistory history = new ClientMembershipPlanHistory();
                    history.ClientMembershipPlanID = rs.ClientMembershipPlanID;
                    history.ClientID = rs.ClientID;
                    history.MembershipName = rs.MembershipName;
                    history.MembershipDuration = rs.MembershipDuration;
                    history.MembershipDurationType = rs.MembershipDurationType;
                    history.Price = rs.Price;
                    history.CreatedBy = rs.CreatedBy;
                    history.CreatedDateTime = rs.CreatedDateTime;

                    postTask = client.PostAsJsonAsync<ClientMembershipPlanHistory>(Common.Instance.ApiClientAddClientMembershipPlanHistory, history);
                    postTask.Wait();
                    result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var rs1 = result.Content.ReadAsAsync<bool>().Result;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Display(int ClientMembershipPlanID)
        {
            SetSessionVariables();
            ViewBag.ListOfMembershipDurationTypes = Common.GetMembershipDurationTypes();
            MembershipPlanViewModel model = new MembershipPlanViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientMembershipPlanDetails + "/" + ClientMembershipPlanID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientMembershipPlan>();
                    readTask.Wait();

                    model.clientMemPlan = readTask.Result;
                    model.MembershipDurationType = model.clientMemPlan.MembershipDurationType;
                    model.IsActive = model.clientMemPlan.IsActive.Value;
                    model.IsRecommented = model.clientMemPlan.IsRecommented.Value;
                }
                else //web api sent error response 
                {
                    //log response status here..



                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(model);
        }

        public ActionResult PlanList()
        {
            SetSessionVariables();

            IEnumerable<ClientMembershipPlan> clientMemPlans = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientMembershipPlans + "/" + GetSessionObject().ClientID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientMembershipPlan>>();
                    readTask.Wait();

                    clientMemPlans = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    clientMemPlans = Enumerable.Empty<ClientMembershipPlan>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clientMemPlans);
        }

        public ActionResult PlanHistory()
        {
            SetSessionVariables();

            MembershipPlanHistoryViewModel vModel = new MembershipPlanHistoryViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetMembershipPlanHistories + "/" + GetSessionObject().MemberID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<MembershipPlanHistoryResponse>>();
                    readTask.Wait();
                    vModel.membershipPlanHistory = readTask.Result;
                    if (vModel.membershipPlanHistory.Count > 0)
                    {
                        vModel.currentPlan = vModel.membershipPlanHistory[0];
                    }
                }
                else //web api sent error response 
                {
                    //log response status here..
                  
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
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
            ViewBag.ClientLocationID = HttpContext.Session.GetInt32(Common.SessionClientLocationID);
            #endregion
        }

        public UserLogin GetSessionObject()
        {
            return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
        }

        public string GetEndDate()
        {
            return "";
        }

        [HttpPost]
        public ActionResult UpdateMembershipPlan(int ClientMembershipPlanID)
        {
            //GetSessionObject().MemberID
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientAddMembershipPlanHistory + "/" + GetSessionObject().MemberID + "/" + ClientMembershipPlanID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<bool>();
                    readTask.Wait();

                    var rs = readTask.Result;
                }
            }
            return RedirectToAction("Index");
        }

    }
}