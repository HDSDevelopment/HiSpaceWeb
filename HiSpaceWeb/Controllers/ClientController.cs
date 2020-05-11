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
	public class ClientController : Controller
	{
		public static string ApiAddress = "http://localhost:52886/api/";
		public static string Base = ApiAddress + "Client/";

		private readonly IHostingEnvironment hostingEnvironment;

		private void Test()
		{
			List<ClientFacility> facilityLst = new List<ClientFacility>();
			facilityLst.Add(
				new ClientFacility
				{
					ClientFacilityID = 0,
					ClientID = 2,
					FacilityID = 5,
					Available = true
				});

			using (var client = new HttpClient())
			{
				//HTTP GET
				var responseTask = client.PutAsJsonAsync(Base + "UpdateClientFacility/2", facilityLst);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
				}
			}
		}

		public ClientController(IHostingEnvironment hostingEnvironment)
		{
			this.hostingEnvironment = hostingEnvironment;
		}

		// GET: Client
		[HttpGet]
		public ActionResult Index()
		{
			SetSessionVariables();

			//Test();

			IEnumerable<ClientMaster> clients = null;

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

					clients = readTask.Result;
				}
				else //web api sent error response
				{
					//log response status here..

					clients = Enumerable.Empty<ClientMaster>();

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
		public ActionResult Create(ClientMasterViewModel model)
		{
			SetSessionVariables();

			if (model != null)
			{
				ClientMaster NewClient = new ClientMaster();
				UserLogin userLogin = new UserLogin();
				model.ClientMaster.CreatedBy = GetSessionObject().UserID;

				string DuplicateName = "";
				string OriginalName = "";
				string UploadRootPath = "Upload";
				string uploadsFolder = "\\client\\" + GetSessionObject().ClientID + "\\documents\\";
				string serverUploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadRootPath);
				serverUploadsFolder += uploadsFolder;
				if (!Directory.Exists(serverUploadsFolder))
				{
					Directory.CreateDirectory(serverUploadsFolder);
				}

				//RCCopy image uploader
				if (model.RCCopy != null)
				{
					OriginalName = model.RCCopy.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_RCCopy" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.RCCopy.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_RCCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//PANCopy image uploader
				if (model.PANCopy != null)
				{
					OriginalName = model.PANCopy.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_PANCopy" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.PANCopy.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_PANCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//GSTCopy image uploader
				if (model.GSTCopy != null)
				{
					OriginalName = model.GSTCopy.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_GSTCopy" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.GSTCopy.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_GSTCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//MembershipAgreementCopy image uploader
				if (model.MembershipAgreementCopy != null)
				{
					OriginalName = model.MembershipAgreementCopy.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_MembershipAgreementCopy" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.MembershipAgreementCopy.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_MembershipAgreementCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//ContactPersonAadhaar image uploader
				if (model.ContactPersonAadhaar != null)
				{
					OriginalName = model.ContactPersonAadhaar.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_ContactPersonAadhaar" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.ContactPersonAadhaar.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_ContactPersonAadhaar = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//ContactPersonPAN image uploader
				if (model.ContactPersonPAN != null)
				{
					OriginalName = model.ContactPersonPAN.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_ContactPersonPAN" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.ContactPersonPAN.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.Doc_ContactPersonPAN = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				//Logo image uploader
				if (model.Logo != null)
				{
					OriginalName = model.Logo.FileName;
					string extension = Path.GetExtension(OriginalName);
					DuplicateName = "_Logo" + extension;

					string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
					model.Logo.CopyTo(new FileStream(filePath, FileMode.Create));
					model.ClientMaster.ClientLogo = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
				}

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);

					//HTTP POST
					var postTask = client.PostAsJsonAsync<ClientMaster>(Common.Instance.ApiClientAddClientMaster, model.ClientMaster);
					postTask.Wait();

					var result = postTask.Result;
					if (result.IsSuccessStatusCode)
					{
						//var rs = result.Content.ReadAsAsync<ClientMaster>();
						////return RedirectToAction("Index");
						//NewClient = rs.Result;
						var rs = result.Content.ReadAsAsync<int>().Result;
						model.UserLogin.ClientID = rs;
					}
					model.UserLogin.Username = model.ClientMaster.ClientUsername;
					model.UserLogin.Password = model.ClientMaster.ClientPassword;
					model.UserLogin.Active = model.ClientMaster.Active;
					model.UserLogin.UserType = 2;

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

				//           string uniqueRCCopy = "";
				//           string OriginalName = "";
				//           if (model.RCCopy != null)
				//           {
				//               string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
				//               uploadsFolder += "\\" + NewClient.ClientID.ToString() + "_" + NewClient.ClientName+"\\";
				//               if (!Directory.Exists(uploadsFolder))
				//               {
				//                   Directory.CreateDirectory(uploadsFolder);
				//               }
				//OriginalName = model.RCCopy.FileName;
				//string extension = Path.GetExtension(OriginalName);
				//uniqueRCCopy = "_RCCopy"+extension;

				//               string filePath = Path.Combine(uploadsFolder, uniqueRCCopy);
				//               model.RCCopy.CopyTo(new FileStream(filePath, FileMode.Create));
				//               model.ClientMaster.Doc_RCCopy = uploadsFolder + uniqueRCCopy;
				//           }

				ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int ClientID)
		{
			SetSessionVariables();

			ClientMaster clientMaster = null;
			ClientMasterViewModel clientMasterViewModel = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClient + ClientID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<ClientMaster>();
					readTask.Wait();

					clientMaster = readTask.Result;
				}
			}
			clientMasterViewModel = new ClientMasterViewModel
			{
				ClientMaster = clientMaster
			};

			return View(clientMasterViewModel);
		}

		[HttpPost]
		public ActionResult Edit(ClientMasterViewModel model)
		{
			SetSessionVariables();

			ClientMaster clientMaster = null;

			string DuplicateName = "";
			string OriginalName = "";
			string UploadRootPath = "Upload";
			string uploadsFolder = "\\client\\" + GetSessionObject().ClientID + "\\documents\\";
			string serverUploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, UploadRootPath);
			serverUploadsFolder += uploadsFolder;
			if (!Directory.Exists(serverUploadsFolder))
			{
				Directory.CreateDirectory(serverUploadsFolder);
			}

			//RCCopy image uploader
			if (model.RCCopy != null)
			{
				OriginalName = model.RCCopy.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_RCCopy" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.RCCopy.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_RCCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//PANCopy image uploader
			if (model.PANCopy != null)
			{
				OriginalName = model.PANCopy.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_PANCopy" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.PANCopy.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_PANCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//GSTCopy image uploader
			if (model.GSTCopy != null)
			{
				OriginalName = model.GSTCopy.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_GSTCopy" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.GSTCopy.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_GSTCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//MembershipAgreementCopy image uploader
			if (model.MembershipAgreementCopy != null)
			{
				OriginalName = model.MembershipAgreementCopy.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_MembershipAgreementCopy" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.MembershipAgreementCopy.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_MembershipAgreementCopy = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//ContactPersonAadhaar image uploader
			if (model.ContactPersonAadhaar != null)
			{
				OriginalName = model.ContactPersonAadhaar.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_ContactPersonAadhaar" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.ContactPersonAadhaar.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_ContactPersonAadhaar = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//ContactPersonPAN image uploader
			if (model.ContactPersonPAN != null)
			{
				OriginalName = model.ContactPersonPAN.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_ContactPersonPAN" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.ContactPersonPAN.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.Doc_ContactPersonPAN = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			//Logo image uploader
			if (model.Logo != null)
			{
				OriginalName = model.Logo.FileName;
				string extension = Path.GetExtension(OriginalName);
				DuplicateName = "_Logo" + extension;

				string filePath = Path.Combine(serverUploadsFolder, DuplicateName);
				model.Logo.CopyTo(new FileStream(filePath, FileMode.Create));
				model.ClientMaster.ClientLogo = "\\" + UploadRootPath + uploadsFolder + DuplicateName;
			}

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.PutAsJsonAsync(Common.Instance.ApiClientUpdateClientMaster + model.ClientMaster.ClientID, model.ClientMaster);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<ClientMaster>();
					readTask.Wait();

					clientMaster = readTask.Result;
				}
			}
			return RedirectToAction("Edit", new { ClientID = model.ClientMaster.ClientID });
		}

		[HttpGet]
		public ActionResult Display(int ClientID)
		{
			SetSessionVariables();

			ClientMaster clientMaster = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClient + ClientID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<ClientMaster>();
					readTask.Wait();

					clientMaster = readTask.Result;
				}
			}
			return View(clientMaster);
		}

		[HttpPost]
		public ActionResult Display(ClientMaster model)
		{
			SetSessionVariables();

			ClientMaster clientMaster = null;

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Approve(int ClientID, string Status)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientApproveClient + ClientID + "/" + Status);
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