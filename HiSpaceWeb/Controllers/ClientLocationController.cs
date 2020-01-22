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

namespace HiSpaceWeb.Controllers
{
    public class ClientLocationController : Controller
    {
        // GET: Table
        public ActionResult Index()
        {
            SetSessionVariables();
            
            IEnumerable<ClientLocation> clLocations = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientLocationsByClientID + GetSessionObject().ClientID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientLocation>>();
                    readTask.Wait();

                    clLocations = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    clLocations = Enumerable.Empty<ClientLocation>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clLocations);
        }

        // GET: Create
        public ActionResult Create()
        {
            SetSessionVariables();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientLocation model)
        {
            SetSessionVariables();

            if (model != null)
            {
                model.CreatedBy = GetSessionObject().UserID;
                model.CreatedDateTime = DateTime.Now;
                model.ClientID = GetSessionObject().ClientID.Value;
                model.ClientLocationStatus = true;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ClientLocation>(Common.Instance.ApiClientAddClientLocation, model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return RedirectToAction("Index");
        }

        // GET: Edit
        [HttpGet]
        public ActionResult Edit(int ClientLocationID)
        {
            SetSessionVariables();

            ClientLocation clientLocation = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientLocation + ClientLocationID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientLocation>();
                    readTask.Wait();

                    clientLocation = readTask.Result;
                }
            }
            return View(clientLocation);
        }

        [HttpPost]
        public ActionResult Edit(ClientLocation model)
        {
            SetSessionVariables();

            ClientLocation clientLocation = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.PutAsJsonAsync(Common.Instance.ApiClientUpdateClientLocation + model.ClientLocationID, model);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientLocation>();
                    readTask.Wait();

                    clientLocation = readTask.Result;
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Display
        [HttpGet]
        public ActionResult Display(int ClientLocationID)
        {
            SetSessionVariables();

            ClientLocation clientLocation = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientLocation + ClientLocationID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ClientLocation>();
                    readTask.Wait();

                    clientLocation = readTask.Result;
                }
            }
            return View(clientLocation);
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