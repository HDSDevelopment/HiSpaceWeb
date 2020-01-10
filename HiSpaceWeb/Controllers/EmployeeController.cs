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
    public class EmployeeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public EmployeeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            SetSessionVariables();

            IEnumerable<EmployeeMaster> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiEmployeeGetEmployees + "/" + GetSessionObject().MemberID.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EmployeeMaster>>();
                    readTask.Wait();

                    employees = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    employees = Enumerable.Empty<EmployeeMaster>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(employees);
        }

        public ActionResult Create()
        {
            SetSessionVariables();

            EmployeeMaster model = new EmployeeMaster();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmployeeMaster employee)
        {
            SetSessionVariables();

            employee.CreatedBy = GetSessionObject().UserID;
            employee.CreatedDateTime = DateTime.Now;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<EmployeeMaster>(Common.Instance.ApiEmployeeAddEditEmployee, employee);
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

        public ActionResult Edit(string EmpCode)
        {
            SetSessionVariables();
            EmployeeMaster emp = new EmployeeMaster();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);
                //HTTP GET
                var responseTask = client.GetAsync(Common.Instance.ApiEmployeeGetEmployeeDetails + "/" + EmpCode);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EmployeeMaster>();
                    readTask.Wait();

                    emp = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeMaster employee)
        {
            SetSessionVariables();

            employee.ModifyBy = GetSessionObject().UserID;
            employee.ModifiedDateTime = DateTime.Now;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<EmployeeMaster>(Common.Instance.ApiEmployeeAddEditEmployee, employee);
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

        public ActionResult Display()
        {
            SetSessionVariables();
            return View();
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

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var files = Request.Form.Files;
            return Ok();
        }
		//[HttpPost]
		//public ActionResult UploadFile(HttpPostedFileBase postedFile)
		//{
		//    return View();
		//}

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

					List<EmployeeMaster> emplist = new List<EmployeeMaster>();

					for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
					{
						EmployeeMaster emp = new EmployeeMaster();
						emp.MemberID = GetSessionObject().MemberID.Value;
						emp.CreatedBy = GetSessionObject().MemberID.Value;
						emp.ModifyBy = GetSessionObject().MemberID.Value;

						DateTime dn = DateTime.Now;
						emp.CreatedDateTime = dn;
						emp.ModifiedDateTime = dn;

						//DateTime dt = DateTime.Now;
						//string s2 = dt.ToString("dd-MM-yyyy hh:mm:ss");
						//DateTime dtnew = DateTime.Parse(s2);
						//emp.DOJ = dtnew;

						IRow row = sheet.GetRow(i);
						if (row == null) continue;
						if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
						{
							emp.EmpCode = row.GetCell(GetCellIndex(headerRow, cellCount, "EmpCode")).ToString();
							emp.Name = row.GetCell(GetCellIndex(headerRow, cellCount, "Name")).ToString();
							emp.Designation = row.GetCell(GetCellIndex(headerRow, cellCount, "Designation")).ToString();
							emp.PAN = row.GetCell(GetCellIndex(headerRow, cellCount, "PAN")).ToString();
							emp.Identification = row.GetCell(GetCellIndex(headerRow, cellCount, "Identification")).ToString();
							
							//string dt1 = row.GetCell(GetCellIndex(headerRow, cellCount, "DOJ")).DateCellValue.ToString();
							//DateTime date1 = DateTime.Parse(dt1);
							//emp.DOJ = date1;

							//Date of join
							var dt1 = row.GetCell(GetCellIndex(headerRow, cellCount, "DOJ"));
							if (dt1 != null)
							{
								DateTime date1 = DateTime.Parse(dt1.DateCellValue.ToString());
								emp.DOJ = date1;
							}
							else
								emp.DOJ = null;

							//Date of Releave
							var dt2 = row.GetCell(GetCellIndex(headerRow, cellCount, "DOR"));
							if (dt2 != null)
							{
								DateTime date2 = DateTime.Parse(dt2.DateCellValue.ToString());
								emp.DOR = date2;
							}
							else
								emp.DOR = null;
						}
						emplist.Add(emp);
					}

					using (var client = new HttpClient())
					{
						client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);

						//HTTP POST
						var postTask = client.PostAsJsonAsync<List<EmployeeMaster>>(Common.Instance.ApiEmployeeUploadEmployees, emplist);
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
			return this.Content("Employee update");
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

		//public async Task<IActionResult> OnPostExport()
		//{
		//    string sWebRootFolder = _hostingEnvironment.WebRootPath;
		//    string sFileName = @"demo.xlsx";
		//    string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
		//    FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
		//    var memory = new MemoryStream();
		//    using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
		//    {
		//        IWorkbook workbook;
		//        workbook = new XSSFWorkbook();
		//        ISheet excelSheet = workbook.CreateSheet("Demo");
		//        IRow row = excelSheet.CreateRow(0);

		//        row.CreateCell(0).SetCellValue("ID");
		//        row.CreateCell(1).SetCellValue("Name");
		//        row.CreateCell(2).SetCellValue("Age");

		//        row = excelSheet.CreateRow(1);
		//        row.CreateCell(0).SetCellValue(1);
		//        row.CreateCell(1).SetCellValue("Kane Williamson");
		//        row.CreateCell(2).SetCellValue(29);

		//        row = excelSheet.CreateRow(2);
		//        row.CreateCell(0).SetCellValue(2);
		//        row.CreateCell(1).SetCellValue("Martin Guptil");
		//        row.CreateCell(2).SetCellValue(33);

		//        row = excelSheet.CreateRow(3);
		//        row.CreateCell(0).SetCellValue(3);
		//        row.CreateCell(1).SetCellValue("Colin Munro");
		//        row.CreateCell(2).SetCellValue(23);

		//        workbook.Write(fs);
		//    }
		//    using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
		//    {
		//        await stream.CopyToAsync(memory);
		//    }
		//    memory.Position = 0;
		//    return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
		//}

	}
}