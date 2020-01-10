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
	public class BulkUploadController : Controller
	{
		private IHostingEnvironment _hostingEnvironment;
		public BulkUploadController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}
		public ActionResult EmployeeUpload()
		{
			SetSessionVariables();
			return View();
		}

		public ActionResult AttendanceUpload()
		{
			SetSessionVariables();
			return View();
		}

		//public ActionResult OnPostImport()
		//{
		//	IFormFile file = Request.Form.Files[0];
		//	string folderName = "Upload";
		//	string webRootPath = _hostingEnvironment.WebRootPath;
		//	string newPath = Path.Combine(webRootPath, folderName);
		//	StringBuilder sb = new StringBuilder();
		//	if (!Directory.Exists(newPath))
		//	{
		//		Directory.CreateDirectory(newPath);
		//	}
		//	if (file.Length > 0)
		//	{
		//		string sFileExtension = Path.GetExtension(file.FileName).ToLower();
		//		ISheet sheet;
		//		string fullPath = Path.Combine(newPath, file.FileName);
		//		using (var stream = new FileStream(fullPath, FileMode.Create))
		//		{
		//			file.CopyTo(stream);
		//			stream.Position = 0;
		//			if (sFileExtension == ".xls")
		//			{
		//				HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
		//				sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
		//			}
		//			else
		//			{
		//				XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
		//				sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
		//			}
		//			IRow headerRow = sheet.GetRow(0); //Get Header Row
		//			int cellCount = headerRow.LastCellNum;

		//			List<EmployeeMaster> emplist = new List<EmployeeMaster>();

		//			for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
		//			{
		//				EmployeeMaster emp = new EmployeeMaster();
		//				emp.MemberID = GetSessionObject().MemberID.Value;
		//				IRow row = sheet.GetRow(i);
		//				if (row == null) continue;
		//				if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
		//				{
		//					emp.EmpCode = row.GetCell(GetCellIndex(headerRow, cellCount, "EmpCode")).ToString();
		//					emp.Name = row.GetCell(GetCellIndex(headerRow, cellCount, "Name")).ToString();
		//					emp.Designation = row.GetCell(GetCellIndex(headerRow, cellCount, "Designation")).ToString();
		//					emp.PAN = row.GetCell(GetCellIndex(headerRow, cellCount, "PAN")).ToString();
		//					emp.Identification = row.GetCell(GetCellIndex(headerRow, cellCount, "Identification")).ToString();
		//				}
		//				emplist.Add(emp);

		//			}

		//			using (var client = new HttpClient())
		//			{
		//				client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);

		//				//HTTP POST
		//				var postTask = client.PostAsJsonAsync<List<EmployeeMaster>>(Common.Instance.ApiEmployeeUploadEmployees, emplist);
		//				postTask.Wait();

		//				var result = postTask.Result;
		//				if (result.IsSuccessStatusCode)
		//				{
		//					//var rs = result.Content.ReadAsAsync<ClientMaster>();
		//					////return RedirectToAction("Index");
		//					//NewClient = rs.Result;
		//					var rs = result.Content.ReadAsAsync<bool>().Result;
		//				}
		//			}

		//		}
		//	}
		//	return this.Content("test");
		//	//return this.Content(sb.ToString());
		//}

		////To get the index values
		//public int GetCellIndex(IRow headerRow, int cellCount, string cellName)
		//{
		//	int index = 0;
		//	bool found = false;

		//	for (int c = 0; c < cellCount; c++)
		//	{
		//		if (!found)
		//		{
		//			if (headerRow.Cells[c].ToString() == cellName)
		//			{
		//				index = c;
		//				found = true;
		//				return index;
		//			}
		//		}
		//	}
		//	return index;
		//}


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