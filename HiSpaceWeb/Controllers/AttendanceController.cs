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
using HiSpaceWeb.ViewModel;

namespace HiSpaceWeb.Controllers
{
    public class AttendanceController : Controller
    {

		private IHostingEnvironment _hostingEnvironment;

		public AttendanceController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpGet]
        public ActionResult Index()
        {
			ViewBag.ListOfEmployee = Common.GetEmployeeList(GetSessionObject().MemberID.Value);

			AttendanceViewModel vModel = new AttendanceViewModel();

            SetSessionVariables();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiAttendanceControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiAttendanceGetAttendanceByMember+ "/" + GetSessionObject().MemberID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Attendance>>();
                    readTask.Wait();

                    vModel.AttendanceList = readTask.Result.ToList();

					
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //holidays = Enumerable.Empty<Holiday>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(vModel);
        }


		public ActionResult Index(AttendanceViewModel model)
		{
			ViewBag.ListOfEmployee = Common.GetEmployeeList(GetSessionObject().MemberID.Value);

			AttendanceViewModel vModel = new AttendanceViewModel();

			SetSessionVariables();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiAttendanceControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiAttendanceGetAttendanceByMember + "/" + GetSessionObject().MemberID.ToString()+"/"+model.EmpID + "/" +model.FromDate + "/" + model.ToDate);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<Attendance>>();
					readTask.Wait();

					vModel.AttendanceList = readTask.Result.ToList();


				}
				else //web api sent error response 
				{
					//log response status here..

					//holidays = Enumerable.Empty<Holiday>();

					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			return View(vModel);
		}

		public ActionResult OnPostImport()
		{
			IFormFile file = Request.Form.Files[0];
			string folderName = "Upload";
			string webRootPath = _hostingEnvironment.WebRootPath;
			string newPath = Path.Combine(webRootPath, folderName);
			StringBuilder sb = new StringBuilder();
			if (!Directory.Exists(newPath))
			{
				Directory.CreateDirectory(newPath);
			}
			if (file.Length > 0)
			{
				string sFileExtension = Path.GetExtension(file.FileName).ToLower();
				ISheet sheet;
				string fullPath = Path.Combine(newPath, file.FileName);
				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					file.CopyTo(stream);
					stream.Position = 0;
					if (sFileExtension == ".xls")
					{
						HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
						sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
					}
					else
					{
						XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
						sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
					}
					IRow headerRow = sheet.GetRow(0); //Get Header Row
					int cellCount = headerRow.LastCellNum;

					List<Attendance> attendencelist = new List<Attendance>();

					for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
					{
						Attendance atten = new Attendance();
						atten.MemberID = GetSessionObject().MemberID.Value;
						atten.CreatedBy = GetSessionObject().MemberID.Value;

						DateTime dn = DateTime.Now;
						atten.CreatedDateTime = dn;

						IRow row = sheet.GetRow(i);
						if (row == null) continue;
						if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
						{
							atten.EmpCode = row.GetCell(GetCellIndex(headerRow, cellCount, "EmpCode")).ToString();

							//Attendance Date
							var dt1 = row.GetCell(GetCellIndex(headerRow, cellCount, "AttendanceDate"));
							if (dt1 != null)
							{
								DateTime date1 = DateTime.Parse(dt1.DateCellValue.ToString());
								atten.AttendanceDate = date1;
							}
							else
								atten.AttendanceDate = null;

							//Index Time
							var t1 = row.GetCell(GetCellIndex(headerRow, cellCount, "InTime"));
							if (t1 !=null)
							{
								TimeSpan inTime = TimeSpan.Parse( t1.DateCellValue.TimeOfDay.ToString());
								atten.InTime = inTime;
							}
							else
								atten.InTime = null;

							//Out Time
							var t2 = row.GetCell(GetCellIndex(headerRow, cellCount, "OutTime"));
							if (t2!=null)
							{
								TimeSpan outTime = TimeSpan.Parse( t2.DateCellValue.TimeOfDay.ToString());
								atten.OutTime = outTime;
							}
							else
								atten.OutTime = null;

						}
						attendencelist.Add(atten);

					}
					var memID = GetSessionObject().MemberID.Value;
					using (var client = new HttpClient())
					{
						client.BaseAddress = new Uri(Common.Instance.ApiAttendanceControllerName);
						

						//HTTP POST
						var postTask = client.PostAsJsonAsync<List<Attendance>>(Common.Instance.ApiAttendanceUploadAttendance+"/"+memID, attendencelist);
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

				}
			}
			return this.Content("Attendance update");
			//return this.Content(sb.ToString());
		}

		//To get the index values
		public int GetCellIndex(IRow headerRow, int cellCount, string cellName)
		{
			int index = 0;
			bool found = false;

			for (int c = 0; c < cellCount; c++)
			{
				if (!found)
				{
					if (headerRow.Cells[c].ToString() == cellName)
					{
						index = c;
						found = true;
						return index;
					}
				}
			}
			return index;
		}

		//get employee list
		#region
		
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

		public UserLogin GetSessionObject()
		{
			return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
		}
	}
}