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
	public class LeadController : Controller
	{
		// GET: Table
		public ActionResult Index()
		{
			SetSessionVariables();

			IEnumerable<Lead> lead = null;
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiLeadControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiLeadGetLeadsByClientID + GetSessionObject().ClientID);
				responseTask.Wait();
				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<Lead>>();
					readTask.Wait();

					lead = readTask.Result;
				}
				else //web api sent error response
				{
					//log response status here..

					lead = Enumerable.Empty<Lead>();

					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}
			return View(lead);
		}

		// GET: Create
		public ActionResult Create()
		{
			SetSessionVariables();
			return View();
		}

		//Post: create model
		[HttpPost]
		public ActionResult Create(Lead model)
		{
			SetSessionVariables();
			if (model != null)
			{
				model.CreatedBy = GetSessionObject().UserID;
				model.CreatedDateTime = DateTime.Now;
				model.ClientID = GetSessionObject().ClientID.Value;
				model.LeadGenerationCode = model.LeadName + "_" + Guid.NewGuid().ToString();
			}

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiLeadControllerName);

				//HTTP POST
				var postTask = client.PostAsJsonAsync<Lead>(Common.Instance.ApiLeadAddLead, model);
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
		public ActionResult Edit(int LeadID)
		{
			SetSessionVariables();

			Lead lead = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiLeadControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiLeadGetLead + LeadID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<Lead>();
					readTask.Wait();

					lead = readTask.Result;
				}
			}
			return View(lead);
		}

		//Post: Edit model
		[HttpPost]
		public ActionResult Edit(Lead model)
		{
			SetSessionVariables();

			Lead lead = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiLeadControllerName);
				//HTTP GET
				var responseTask = client.PutAsJsonAsync(Common.Instance.ApiLeadUpdateLead + model.LeadID, model);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<Lead>();
					readTask.Wait();

					lead = readTask.Result;
				}
			}
			return RedirectToAction("Index");
		}

		// GET: Display
		[HttpGet]
		public ActionResult Display(int LeadID)
		{
			SetSessionVariables();

			Lead lead = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiLeadControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiLeadGetLead + LeadID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<Lead>();
					readTask.Wait();

					lead = readTask.Result;
				}
			}
			return View(lead);
		}

		//Session Variables
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