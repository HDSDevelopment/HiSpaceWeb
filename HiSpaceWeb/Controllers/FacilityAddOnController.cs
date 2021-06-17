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
	public class FacilityAddOnController : Controller
	{
        // GET: Client
		[HttpGet]
		public ActionResult List(int memberBookingSpaceID)
		{
			SetSessionVariables();

			//Test();

			IEnumerable<FacilityAddOn> facilityAddOns = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiFacilityAddOnControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiFacilityAddOnList + memberBookingSpaceID);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<List<FacilityAddOn>>();
					readTask.Wait();

					facilityAddOns = readTask.Result;
				}
				else //web api sent error response
				{
					//log response status here..
					
					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}
			return View(facilityAddOns);
		}

		[HttpGet, ActionName("Add")]
		public ActionResult AddGet(int memberBookingSpaceID)
		{
			SetSessionVariables();
			FacilityAddOn facilityAddOn = new FacilityAddOn()
													{MemberBookingSpaceID = memberBookingSpaceID };	
			return View(facilityAddOn);
		}

		[HttpPost]
        public ActionResult Add(FacilityAddOn facilityAddOn)
        {
            SetSessionVariables();            

            if (!ModelState.IsValid)
            	return View("Error");                                

                using (var client = new HttpClient())
                {
					facilityAddOn.CreatedBy = GetSessionObject().UserID;
                    client.BaseAddress = new Uri(Common.Instance.ApiFacilityAddOnControllerName);		

                    //HTTP POST FacilityAddOn
                    var postTask = client.PostAsJsonAsync<FacilityAddOn>(Common.Instance.ApiFacilityAddOnAdd, facilityAddOn);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<bool>();
                        readTask.Wait();                                                
                    }
					else
						return View("Error");
                }
				
				return RedirectToAction("List", new { memberBookingSpaceID = 																	facilityAddOn.MemberBookingSpaceID});
        }

		[HttpGet]
		public ActionResult Delete(int facilityAddOnID, int memberBookingSpaceID)
        {
			int MBSID = memberBookingSpaceID;
                using (var client = new HttpClient())
                {					
                    client.BaseAddress = new Uri(Common.Instance.ApiFacilityAddOnControllerName);		

                    var postTask = client.GetAsync(Common.Instance.ApiFacilityAddOnDelete + "/" + facilityAddOnID);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<bool>();
                        readTask.Wait();
						return RedirectToAction("List", new { memberBookingSpaceID = 																	MBSID});
                    }						
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

		public UserLogin GetSessionObject()
        {
            return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
        }
    }
}