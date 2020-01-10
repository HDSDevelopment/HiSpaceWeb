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
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.Controllers
{
    public class MemberBookingController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public MemberBookingController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public ActionResult Index()
        {          
            SetSessionVariables();
            UserLogin rs = HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
            if (rs.MemberID == null)
                rs.MemberID = 0;
            IEnumerable<MemberBookingResponse> bookings = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiMemberGetMemberBookingsByMemberID + rs.MemberID + "/" + rs.ClientID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MemberBookingResponse>>();
                    readTask.Wait();

                    bookings = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bookings = Enumerable.Empty<MemberBookingResponse>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bookings);
        }

        [HttpGet]
        public ActionResult Approve(int MemberBookingSpaceID, string Status)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiMemberApproveMemberBooking+ MemberBookingSpaceID + "/" + Status);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var rs = result.Content.ReadAsAsync<bool>().Result;
                    var sr = rs;
                }
            }
            return RedirectToAction("Index");
        }

        public void SetSessionVariables()
        {
            #region
            UserLogin rs = HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
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