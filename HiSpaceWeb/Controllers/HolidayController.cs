using HiSpaceModels;
using HiSpaceWeb.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System;

namespace HiSpaceWeb.Controllers
{
    public class HolidayController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public HolidayController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<HolidayMaster> holidays = new List<HolidayMaster>();

            SetSessionVariables();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiHolidayControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiHolidayGetHolidaysByClientID + "/" + GetSessionObject().ClientID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HolidayMaster>>();
                    readTask.Wait();

                    holidays = readTask.Result.ToList();
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //holidays = Enumerable.Empty<Holiday>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(holidays);
        }

        public ActionResult Create()
        {
            HolidayMaster holiday = new HolidayMaster();

            SetSessionVariables();

            return View(holiday);
        }

        [HttpPost]
        public ActionResult Create(HolidayMaster holiday)
        {
            SetSessionVariables();
            holiday.ClientID = GetSessionObject().ClientID.Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiHolidayControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<HolidayMaster>(Common.Instance.ApiHolidayAddEditHoliday, holiday);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var rs = result.Content.ReadAsAsync<ClientMaster>();
                    ////return RedirectToAction("Index");
                    //NewClient = rs.Result;
                    var rs = result.Content.ReadAsAsync<bool>().Result;
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int HolidayID, DateTime HolidayDate, string HolidayDetails)
        {
            HolidayMaster holiday = new HolidayMaster();
            holiday.ClientID = GetSessionObject().ClientID.Value;
            holiday.HolidayID = HolidayID;
            holiday.HolidayDate = HolidayDate;
            holiday.HolidayDetails = HolidayDetails;

            SetSessionVariables();

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(Common.Instance.ApiHolidayControllerName);
            //    //HTTP GET
            //    var responseTask = client.PostAsJsonAsync<HolidayMaster>(Common.Instance.ApiHolidayAddEditHoliday, holiday);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<bool>();
            //        readTask.Wait();

            //        var rs = readTask.Result;
            //    }
            //}

            return View(holiday);
        }

        [HttpPost]
        public ActionResult Edit(HolidayMaster holiday)
        {
            SetSessionVariables();
            holiday.ClientID = GetSessionObject().ClientID.Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiHolidayControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<HolidayMaster>(Common.Instance.ApiHolidayAddEditHoliday, holiday);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var rs = result.Content.ReadAsAsync<ClientMaster>();
                    ////return RedirectToAction("Index");
                    //NewClient = rs.Result;
                    var rs = result.Content.ReadAsAsync<bool>().Result;
                }
            }

            return RedirectToAction("Index");
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