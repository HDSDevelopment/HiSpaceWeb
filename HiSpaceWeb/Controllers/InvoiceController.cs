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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using HiSpaceWeb.ViewModel;
using Microsoft.AspNetCore.Http;
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.Controllers
{
	public class InvoiceController : Controller
	{

        // GET: Invoice
		[HttpGet]
		public ActionResult Get(int memberBookingSpaceID)
		{
			//SetSessionVariables();

			//Test();

			Invoice invoice = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiInvoiceControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiInvoiceGet + "/" + memberBookingSpaceID);
				
                responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<Invoice>();
					readTask.Wait();

					invoice = readTask.Result;
                    return View("Get", invoice);
				}
				else //web api sent error response
					//log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
			}
			return View("Error");
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
    }
}
	