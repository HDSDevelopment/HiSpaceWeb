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
using HiSpaceService.ViewModel;
using HiSpaceService.Contracts;

namespace HiSpaceWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SetSessionVariables();
            return View();
        }

        public ActionResult About()
        {

            SetSessionVariables();
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            SetSessionVariables();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[ChildActionOnly]
        [HttpGet]
        public ActionResult GetNotifications()
        {
            if (GetSessionObject() == null)
                return NoContent();
            SetSessionVariables();

            List<PendingNotificationResponse> notifications = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiCommonGetPendingNotification + GetSessionObject().UserID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<PendingNotificationResponse>>();
                    readTask.Wait();

                    notifications = readTask.Result;
                }
            }
            return Json(notifications);
        }

        [HttpPost]
        public ActionResult GoNotificationAction(string NotificationAction)
        {
            if (NotificationAction == Notifications.NewSpaceVerificationPending)
                return Json(Url.Action("Index", "ClientSpace", new { ClientID = 0, ClientLocationID = 0 }));
            else if (NotificationAction == Notifications.UserLogin)
                return Json(Url.Action("Index", "Users"));
            else if (NotificationAction == Notifications.SpaceBookingRequest)
                return Json(Url.Action("Index", "MemberBooking"));
            else if (NotificationAction == Notifications.ClientProfilePending)
                return Json(Url.Action("Edit", "Client", new { ClientID = GetSessionObject().ClientID }));
            else if (NotificationAction == Notifications.ClientVerification)
                return Json(Url.Action("Index", "Client"));
            else if (NotificationAction == Notifications.ClientFloor)
                return Json(Url.Action("Index", "ClientLocation"));
            else if (NotificationAction == Notifications.ClientLocation)
                return Json(Url.Action("Index", "ClientLocation"));
            else if (NotificationAction == Notifications.ClientSpace)
                return Json(Url.Action("Index", "ClientLocation"));
            else if (NotificationAction == Notifications.MemberProfilePending)
                return Json(Url.Action("Edit", "Member", new { MemberID = GetSessionObject().MemberID }));
            else if (NotificationAction == Notifications.MemberNotActivated)
                return Json(Url.Action("Index", "Member"));

            return Json(Url.Action("Index"));
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