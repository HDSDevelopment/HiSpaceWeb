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
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.Controllers
{
    public class UsersController : Controller
    {
        // GET: Client
        [HttpGet]
        public ActionResult Index()
        {
            SetSessionVariables();
            UserLogin rs = HttpContext.Session.GetObjectFromJson<UserLogin>("_user");

            if (rs.ClientID == null)
                rs.ClientID = 0;

            IEnumerable<UserLoginResponse> logins = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiUserLoginGetUserLoginsByClientID + rs.ClientID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserLoginResponse>>();
                    readTask.Wait();

                    logins = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    logins = Enumerable.Empty<UserLoginResponse>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(logins);
        }

        public ActionResult Create()
        {
            SetSessionVariables();

            ViewBag.ListOfClients = GetClientList();

            return View();
        }

        [HttpPost]
        public ActionResult Create(UserLogin newUser)
        {
            SetSessionVariables();

            newUser.ClientID = ViewBag.ClientID;
            //ViewBag.ListOfClients = GetClientList();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<UserLogin>(Common.Instance.ApiUserLoginAddUserLogin, newUser);
                postTask.Wait();

                var result = postTask.Result;
                if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username already exist");
                    return RedirectToAction("Signup", "Authendication");
                }
            }

            //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            //return RedirectToAction("Index");
        }

        public ActionResult Edit(int UserID)
        {
            SetSessionVariables();

            ViewBag.ListOfClients = GetClientList();
            UserLoginViewModel vModel = new UserLoginViewModel();

            //UserLogin user = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiUserLoginGetUserLogin + UserID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserLogin>();
                    readTask.Wait();

                    vModel.UserLogin = readTask.Result;
                    int? CID = vModel.UserLogin.ClientID;
                    int _CID = CID.Value;
                    vModel.ClientMasterID = _CID;
                }
            }
            return View(vModel);
        }

        [HttpPost]
        public ActionResult Edit(UserLogin user)
        {
            SetSessionVariables();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);

                //HTTP POST
                var putTask = client.PutAsJsonAsync<UserLogin>(Common.Instance.ApiUserLoginUpdateUserLogin + user.UserID, user);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Display(int UserID)
        {
            SetSessionVariables();

            ViewBag.ListOfClients = GetClientList();
            UserLoginViewModel vModel = new UserLoginViewModel();
            //UserLogin user = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiUserLoginGetUserLogin + UserID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserLogin>();
                    readTask.Wait();

                    vModel.UserLogin = readTask.Result;

                    int? CID = vModel.UserLogin.ClientID;
                    int _CID = CID.Value;
                    vModel.ClientMasterID = _CID;
                }
            }
            return View(vModel);
        }

        #region
        private List<ClientMaster> GetClientList()
        {
            SetSessionVariables();

            List<ClientMaster> clients = new List<ClientMaster>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClients);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientMaster>>();
                    readTask.Wait();

                    clients = readTask.Result.ToList();
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            clients.Insert(0, new ClientMaster { ClientID = 0, ClientName = "Please Select" });

            return clients;
        }
        #endregion


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

    }
}