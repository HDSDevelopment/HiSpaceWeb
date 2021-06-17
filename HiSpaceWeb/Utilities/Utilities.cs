using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiSpaceWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using HiSpaceModels;
using System.Net.Http;
using HiSpaceWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace HiSpaceWeb.Utilities
{
	public class Common
	{
		private string _ApiAddress = "";

		public string ApiAddress
		{
			set
			{
				_ApiAddress = value;
			}
			get
			{
				return _ApiAddress;
			}
		}

		#region Constructor

		public Common()
		{
			//ApiAddress = ConfigurationManager.AppSettings["HiSpaceServiceURL"].ToString();
			ApiAddress = "https://localhost:5001/api/";
		}

		#endregion Constructor

		#region Singleton Object

		private static readonly object padlock = new object();
		private static Common instance = null;

		public static Common Instance
		{
			get
			{
				if (instance == null)
				{
					lock (padlock)
					{
						if (instance == null)
						{
							instance = new Common();
						}
					}
				}
				return instance;
			}
		}

		#endregion Singleton Object

		#region Session Variables

		public const string SessionUsername = "_Username";
		public const string SessionType = "_Type";
		public const string SessionUserID = "_UserID";
		public const string SessionClientID = "_ClientID";
		public const string SessionMemberID = "_MemberID";
		public const string SessionClientLocationID = "_ClientLocationID";

		#endregion Session Variables

		#region API Methods

		#region UserLogin Controller

		public string ApiUserLoginUserLoginControllerName { get { return ApiAddress + "UserLogin/"; } }

		public string ApiUserLoginGetUserLogins = "GetUserLogins/";
		public string ApiUserLoginGetUserLoginsByClientID = "GetUserLoginsByClientID/";
		public string ApiUserLoginGetUserLogin = "GetUserLogin/";
		public string ApiUserLoginUpdateUserLogin = "UpdateUserLogin/";
		public string ApiUserLoginAddUserLogin = "AddUserLogin/";
		public string ApiUserLoginAuthenticateUser = "AuthenticateUser/";
		public string ApiUserLoginSignupUser = "SignupUser/";

		#endregion UserLogin Controller

		#region Client Controller

		public string ApiClientControllerName
		{
			get
			{
				return ApiAddress + "Client/";
			}
		}

		public string ApiClientGetClients = "GetClients/";
		public string ApiClientGetClient = "GetClient/";
		public string ApiClientAddClientMaster = "AddClientMaster/";
		public string ApiClientUpdateClientMaster = "UpdateClientMaster/";
		public string ApiClientApproveClient = "ApproveClient/";

		public string ApiClientGetClientLocations = "GetClientLocations/";
		public string ApiClientGetClientLocationsByClientID = "GetClientLocationsByClientID/";
		public string ApiClientGetClientLocation = "GetClientLocation/";
		public string ApiClientAddClientLocation = "AddClientLocation/";
		public string ApiClientUpdateClientLocation = "UpdateClientLocation/";

		public string ApiClientGetClientFloors = "GetClientFloors/";
		public string ApiClientGetClientFloorDetails = "GetClientFloorDetails/";
		public string ApiClientUpdateClientFloor = "UpdateClientFloor/";

		public string ApiClientGetClientSpaceFacilities = "GetClientSpaceFacilities/";
		public string ApiGetClientSpaceFacilitiesByClientSpace = "GetClientSpaceFacilitiesByClientSpace/";
		public string ApiClientGetClientFacility = "GetClientFacility/";
		public string ApiClientAddClientFacility = "AddClientFacility/";
		public string ApiClientUpdateClientFacility = "UpdateClientFacility/";

		public string ApiClientGetClientWorkSpaceFloorPlanList = "GetClientWorkSpaceFloorPlanList/";
		public string ApiClientGetClientWorkSpaceFloorPlans = "GetClientWorkSpaceFloorPlans/";
		public string ApiClientGetClientWorkSpaceFloorPlansByFilter = "GetClientWorkSpaceFloorPlansByFilter/";
		public string ApiClientGetClientWorkSpaceFloorPlan = "GetClientWorkSpaceFloorPlan/";
		public string ApiClientAddClientWorkSpaceFloorPlan = "AddClientWorkSpaceFloorPlan/";
		public string ApiClientUpdateClientWorkSpaceFloorPlan = "UpdateClientWorkSpaceFloorPlan/";
		public string ApiClientApproveSpace = "ApproveSpace/";
		public string ApiClientGetFacilitiesBySpace = "GetFacilitiesBySpace/";
		public string ApiClientGetWorkSpaceDetails = "GetWorkSpaceDetails/";
		public string ApiGetClientSpaceFloorPlanByID = "GetClientSpaceFloorPlanByID/";

		public string ApiClientGetClientSpaceSeats = "GetClientSpaceSeats/";
		public string ApiClientGetClientSpaceSeat = "GetClientSpaceSeat/";
		public string ApiClientAddClientSpaceSeat = "AddClientSpaceSeat/";
		public string ApiClientUpdateClientSpaceSeats = "UpdateClientSpaceSeats/";
		public string ApiClientDeleteClientSpaceSeats = "DeleteClientSpaceSeats/";

		public string ApiClientGetClientMembershipPlans = "GetClientMembershipPlans";
		public string ApiClientGetClientMembershipPlanDetails = "GetClientMembershipPlanDetails/";
		public string ApiClientAddEditClientMembershipPlan = "AddEditClientMembershipPlan/";
		public string ApiClientAddClientMembershipPlanHistory = "AddClientMembershipPlanHistory/";
		public string ApiClientAddMembershipPlanHistory = "AddMembershipPlanHistory/";
		public string ApiClientGetMembershipPlanHistories = "GetMembershipPlanHistories/";

		#endregion Client Controller

		#region Common Controller

		public string ApiCommonControllerName
		{
			get
			{
				return ApiAddress + "Common/";
			}
		}

		public string ApiCommonGetUserTypes = "GetUserTypes/";
		public string ApiCommonGetWorkSpaceTypes = "GetAllWorkSpaceTypes/";
		public string ApiCommonGetFacilities = "GetAllAmenities/";
		public string ApiCommonGetChairTypes = "GetAllChairTypes/";
		public string ApiCommonGetMembershipPlans = "GetMembershipPlans";
		public string ApiCommonGetScaleMetrics = "GetAllScaleMetrics/";
		public string ApiCommonGetPendingNotification = "GetPendingNotification/";
		public string ApiCommonGetAllClientLocationSearch = "GetAllClientLocationSearch";
		public string ApiCommonGetAllAmenitiesSearch = "GetAllAmenitiesSearch";
		public string ApiCommonGetAllWorkSpaceTypesSearch = "GetAllWorkSpaceTypesSearch";

		#endregion Common Controller

		#region Member Controller

		public string ApiMemberControllerName
		{
			get
			{
				return ApiAddress + "Member/";
			}
		}

		public string ApiMemberGetMembers = "GetMembers/";
		public string ApiMemberGetMembersByClientID = "GetMembersByClientID/";
		public string ApiMemberGetMember = "GetMember/";
		public string ApiMemberAddMemberMaster = "AddMemberMaster/";
		public string ApiMemberUpdateMemberMaster = "UpdateMemberMaster/";
		public string ApiMemberGetMembershipByMemberID = "GetMembershipByMemberID/";
		public string ApiMemberGetMemberBookingsByMemberID = "GetMemberBookingsByMemberID/";
		public string ApiMemberApproveMemberBooking = "ApproveMemberBooking/";

		#endregion Member Controller

		#region BookingSpace Controller

		public string ApiSpaceBookingControllerName
		{
			get
			{
				return ApiAddress + "SpaceBooking/";
			}
		}

		public string ApiSpaceBookingGetBookingSpaces = "GetBookingSpaces/";
		public string ApiSpaceBookingBookSpaces = "BookSpaces/";
		public string ApiSpaceBookingRequestSpace = "RequestSpace/";
		public string ApiSpaceBookingRequestSpaceSeats = "RequestSpaceSeats/";

		#endregion BookingSpace Controller

		#region Employee Controller

		public string ApiEmployeeControllerName
		{
			get
			{
				return ApiAddress + "Employee/";
			}
		}

		public string ApiEmployeeGetEmployees = "GetEmployees/";
		public string ApiEmployeeGetEmployeeDetails = "GetEmployeeDetails/";
		public string ApiEmployeeAddEditEmployee = "AddEditEmployee/";
		public string ApiEmployeeUploadEmployees = "UploadEmployees/";

		#endregion Employee Controller

		#region Holiday Controller

		public string ApiHolidayControllerName
		{
			get
			{
				return ApiAddress + "Holiday/";
			}
		}

		public string ApiHolidayGetHolidaysByClientID = "GetHolidaysByClientID/";
		public string ApiHolidayAddEditHoliday = "AddEditHoliday/";
		public string ApiHolidayUploadHolidayList = "UploadHolidayList/";

		#endregion Holiday Controller

		#region Attendance Controller

		public string ApiAttendanceControllerName
		{
			get
			{
				return ApiAddress + "Attendance/";
			}
		}

		public string ApiAttendanceGetAttendanceByMember = "GetAttendanceByMember/";
		public string ApiAttendanceUploadAttendance = "UploadAttendance/";

		#endregion Attendance Controller

		#region Lead Controller

		public string ApiLeadControllerName
		{
			get
			{
				return ApiAddress + "Lead/";
			}
		}
		public string ApiLeadGetLeadsByClientID = "GetLeadsByClientID/";
		public string ApiLeadGetLead = "GetLead/";
		public string ApiLeadAddLead = "AddLead/";
		public string ApiLeadUpdateLead = "UpdateLead/";

		#endregion Lead Controller

		#region Facility AddOn Controller

		public string ApiFacilityAddOnControllerName
		{
			get
			{
				return ApiAddress + "FacilityAddOn/";
			}
		}

		public string ApiFacilityAddOnList = "List/";
		public string ApiFacilityAddOnAdd = "Add/";
		public string ApiFacilityAddOnDelete = "Delete/";

		#endregion Facility AddOn Controller

		
		#region Invoice Controller

		public string ApiInvoiceControllerName
		{
			get
			{
				return ApiAddress + "Invoice/";
			}
		}

		public string ApiInvoiceGet = "GetInvoice";
		#endregion Invoice Controller

		#endregion API Methods

		public static List<WSpaceType> GetWorkSpaceTypeList()
		{
			List<WSpaceType> spaces = new List<WSpaceType>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetWorkSpaceTypes);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<WSpaceType>>();
					readTask.Wait();

					spaces = readTask.Result.ToList();
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			spaces.Insert(0, new WSpaceType { WSpaceTypeID = 0, WSpaceTypeName = "Select" });

			return spaces;
		}

		public static List<ChairType> GetChairTypeList()
		{
			List<ChairType> chairTypes = new List<ChairType>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetChairTypes);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<ChairType>>();
					readTask.Wait();

					chairTypes = readTask.Result.ToList();
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			chairTypes.Insert(0, new ChairType { ChairTypeID = 0, ChairTypeName = "Select" });

			return chairTypes;
		}

		public static List<FacilityVM> GetFacilityList()
		{
			List<FacilityVM> facilities = new List<FacilityVM>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetFacilities);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<FacilityMaster>>();
					readTask.Wait();

					foreach (var item in readTask.Result.ToList())
						facilities.Add(new FacilityVM() { FacilityID = item.FacilityID, FacilityName = item.FacilityName, Selected = false });
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			return facilities;
		}

		public static List<FacilityCatVM> GetFacilityCatList()
		{
			List<FacilityCatVM> facilitiesCat = new List<FacilityCatVM>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetFacilities);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<FacilityMaster>>();
					readTask.Wait();

					var facList = readTask.Result.ToList();
					var catList = facList.GroupBy(d => d.CategoryName);

					foreach (var catItem in catList)
					{
						FacilityCatVM cat = new FacilityCatVM();
						cat.CatName = catItem.Key;
						List<FacilityVM> _facVMList = new List<FacilityVM>();
						foreach (var item in facList.Where(d => d.CategoryName == cat.CatName))
						{
							_facVMList.Add(new FacilityVM() { FacilityID = item.FacilityID, FacilityName = item.FacilityName, Selected = false, IsPaidAmenity = false, PaidAmenityPrice = 0 });
						}
						cat.FacilityList = _facVMList;
						facilitiesCat.Add(cat);
					}

					//foreach (var item in readTask.Result.ToList())
					//    facilities.Add(new FacilityVM() { FacilityID = item.FacilityID, FacilityName = item.FacilityName, Selected = false, IsPaidAmenity = true });
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			return facilitiesCat;
		}

		public static List<ScaleMetric> GetScaleMetricList()
		{
			List<ScaleMetric> scales = new List<ScaleMetric>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiCommonControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiCommonGetScaleMetrics);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<ScaleMetric>>();
					readTask.Wait();
					scales = readTask.Result.ToList();
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					// ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			return scales;
		}

		public static List<Status> GetAvailableStatusList()
		{
			List<Status> lst = new List<Status>();

			lst.Add(new Status() { StatusID = 1, StatusName = "Available" });
			lst.Add(new Status() { StatusID = 2, StatusName = "Not Available" });

			return lst;
		}

		public static List<ApplyTo> GetApplyTo()
		{
			List<ApplyTo> lst = new List<ApplyTo>();

			lst.Add(new ApplyTo() { ApplyToID = 0, ApplyToName = "Select" });
			lst.Add(new ApplyTo() { ApplyToID = 1, ApplyToName = "Single Seat" });
			lst.Add(new ApplyTo() { ApplyToID = 2, ApplyToName = "Entire Row" });
			lst.Add(new ApplyTo() { ApplyToID = 3, ApplyToName = "Entire Column" });
			lst.Add(new ApplyTo() { ApplyToID = 4, ApplyToName = "All Seats" });

			return lst;
		}

		public static List<SeatStatus> GetSeatStatus()
		{
			List<SeatStatus> lst = new List<SeatStatus>();

			lst.Add(new SeatStatus() { SeatStatusID = 0, SeatStatusName = "Select" });
			lst.Add(new SeatStatus() { SeatStatusID = 1, SeatStatusName = "Available" });
			lst.Add(new SeatStatus() { SeatStatusID = 2, SeatStatusName = "Occupied" });
			lst.Add(new SeatStatus() { SeatStatusID = 3, SeatStatusName = "Blocked" });

			return lst;
		}

		public static List<ClientLocation> GetLocationList()
		{
			List<ClientLocation> locations = new List<ClientLocation>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientLocations);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<ClientLocation>>();
					readTask.Wait();

					locations = readTask.Result.ToList();
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			locations.Insert(0, new ClientLocation { ClientLocationID = 0, ClientLocationName = "Select" });

			return locations;
		}

		public static List<Floor> GetFloorNumbers()
		{
			List<Floor> floors = new List<Floor>();

			for (int i = 0; i <= 10; i++)
				floors.Add(new Floor() { FloorNumber = i });

			return floors;
		}

		public static List<ClientFloor> GetClientFloors(int ClientLocationID)
		{
			List<ClientFloor> floors = new List<ClientFloor>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiClientGetClientFloors + ClientLocationID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<ClientFloor>>();
					readTask.Wait();

					floors = readTask.Result.ToList();
				}
				else //web api sent error response
				{
					//log response status here..

					//spaces = Enumerable.Empty<WSpaceType>();

					//ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}

			//floors.Insert(0, new ClientFloor { ClientFloorID = 0, FloorNumber = 0 });

			return floors;
		}

		public static List<DurationType> GetMembershipDurationTypes()
		{
			List<DurationType> types = new List<DurationType>();
			types.Add(new DurationType() { MembershipDurationType = "Select Plan" });
			types.Add(new DurationType() { MembershipDurationType = "Hour" });
			types.Add(new DurationType() { MembershipDurationType = "Day" });
			types.Add(new DurationType() { MembershipDurationType = "Month" });
			types.Add(new DurationType() { MembershipDurationType = "Year" });
			types.Add(new DurationType() { MembershipDurationType = "Unlimited" });

			return types;
		}

		public static List<EmployeeMaster> GetEmployeeList(int MemberID)
		{
			List<EmployeeMaster> empLst = new List<EmployeeMaster>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Common.Instance.ApiEmployeeControllerName);
				//HTTP GET
				var responseTask = client.GetAsync(Common.Instance.ApiEmployeeGetEmployees + "/" + MemberID.ToString());
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsAsync<IList<EmployeeMaster>>();
					readTask.Wait();

					empLst = readTask.Result.ToList();
				}
			}

			empLst.Insert(0, new EmployeeMaster { EmpID = 0, Name = "Please Select", EmpCode = "0" });

			return empLst;
		}

		public static List<ScheduleTime> GetScheduleTime()
		{
			List<ScheduleTime> time = new List<ScheduleTime>();
			time.Add(new ScheduleTime() { ScheduleTimeID = 0, ScheduleTimeView = "-select-" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 1, ScheduleTimeSpan = TimeSpan.Parse("00:00:00"), ScheduleTimeView = "12:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 2, ScheduleTimeSpan = TimeSpan.Parse("00:30:00"), ScheduleTimeView = "12:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 3, ScheduleTimeSpan = TimeSpan.Parse("01:00:00"), ScheduleTimeView = "01:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 4, ScheduleTimeSpan = TimeSpan.Parse("01:30:00"), ScheduleTimeView = "01:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 5, ScheduleTimeSpan = TimeSpan.Parse("02:00:00"), ScheduleTimeView = "02:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 6, ScheduleTimeSpan = TimeSpan.Parse("02:30:00"), ScheduleTimeView = "02:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 7, ScheduleTimeSpan = TimeSpan.Parse("03:00:00"), ScheduleTimeView = "03:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 8, ScheduleTimeSpan = TimeSpan.Parse("03:30:00"), ScheduleTimeView = "03:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 9, ScheduleTimeSpan = TimeSpan.Parse("04:00:00"), ScheduleTimeView = "04:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 10, ScheduleTimeSpan = TimeSpan.Parse("04:30:00"), ScheduleTimeView = "04:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 11, ScheduleTimeSpan = TimeSpan.Parse("05:00:00"), ScheduleTimeView = "05:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 12, ScheduleTimeSpan = TimeSpan.Parse("05:30:00"), ScheduleTimeView = "05:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 13, ScheduleTimeSpan = TimeSpan.Parse("06:00:00"), ScheduleTimeView = "06:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 14, ScheduleTimeSpan = TimeSpan.Parse("06:30:00"), ScheduleTimeView = "06:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 15, ScheduleTimeSpan = TimeSpan.Parse("07:00:00"), ScheduleTimeView = "07:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 16, ScheduleTimeSpan = TimeSpan.Parse("07:30:00"), ScheduleTimeView = "07:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 17, ScheduleTimeSpan = TimeSpan.Parse("08:00:00"), ScheduleTimeView = "08:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 18, ScheduleTimeSpan = TimeSpan.Parse("08:30:00"), ScheduleTimeView = "08:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 19, ScheduleTimeSpan = TimeSpan.Parse("09:00:00"), ScheduleTimeView = "09:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 20, ScheduleTimeSpan = TimeSpan.Parse("09:30:00"), ScheduleTimeView = "09:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 21, ScheduleTimeSpan = TimeSpan.Parse("10:00:00"), ScheduleTimeView = "10:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 22, ScheduleTimeSpan = TimeSpan.Parse("10:30:00"), ScheduleTimeView = "10:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 23, ScheduleTimeSpan = TimeSpan.Parse("11:00:00"), ScheduleTimeView = "11:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 24, ScheduleTimeSpan = TimeSpan.Parse("11:30:00"), ScheduleTimeView = "11:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 25, ScheduleTimeSpan = TimeSpan.Parse("12:00:00"), ScheduleTimeView = "12:00 am" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 26, ScheduleTimeSpan = TimeSpan.Parse("12:30:00"), ScheduleTimeView = "12:30 am" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 27, ScheduleTimeSpan = TimeSpan.Parse("13:00:00"), ScheduleTimeView = "01:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 28, ScheduleTimeSpan = TimeSpan.Parse("13:30:00"), ScheduleTimeView = "01:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 29, ScheduleTimeSpan = TimeSpan.Parse("14:00:00"), ScheduleTimeView = "02:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 30, ScheduleTimeSpan = TimeSpan.Parse("14:30:00"), ScheduleTimeView = "02:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 31, ScheduleTimeSpan = TimeSpan.Parse("15:00:00"), ScheduleTimeView = "03:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 32, ScheduleTimeSpan = TimeSpan.Parse("15:30:00"), ScheduleTimeView = "03:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 33, ScheduleTimeSpan = TimeSpan.Parse("16:00:00"), ScheduleTimeView = "04:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 34, ScheduleTimeSpan = TimeSpan.Parse("16:30:00"), ScheduleTimeView = "04:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 35, ScheduleTimeSpan = TimeSpan.Parse("17:00:00"), ScheduleTimeView = "05:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 36, ScheduleTimeSpan = TimeSpan.Parse("17:30:00"), ScheduleTimeView = "05:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 37, ScheduleTimeSpan = TimeSpan.Parse("18:00:00"), ScheduleTimeView = "06:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 38, ScheduleTimeSpan = TimeSpan.Parse("18:30:00"), ScheduleTimeView = "06:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 39, ScheduleTimeSpan = TimeSpan.Parse("19:00:00"), ScheduleTimeView = "07:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 40, ScheduleTimeSpan = TimeSpan.Parse("19:30:00"), ScheduleTimeView = "07:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 41, ScheduleTimeSpan = TimeSpan.Parse("20:00:00"), ScheduleTimeView = "08:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 42, ScheduleTimeSpan = TimeSpan.Parse("20:30:00"), ScheduleTimeView = "08:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 43, ScheduleTimeSpan = TimeSpan.Parse("21:00:00"), ScheduleTimeView = "09:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 44, ScheduleTimeSpan = TimeSpan.Parse("21:30:00"), ScheduleTimeView = "09:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 45, ScheduleTimeSpan = TimeSpan.Parse("22:00:00"), ScheduleTimeView = "10:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 46, ScheduleTimeSpan = TimeSpan.Parse("22:30:00"), ScheduleTimeView = "10:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 47, ScheduleTimeSpan = TimeSpan.Parse("23:00:00"), ScheduleTimeView = "11:00 pm" });
			time.Add(new ScheduleTime() { ScheduleTimeID = 48, ScheduleTimeSpan = TimeSpan.Parse("23:30:00"), ScheduleTimeView = "11:30 pm" });

			time.Add(new ScheduleTime() { ScheduleTimeID = 49, ScheduleTimeSpan = TimeSpan.Parse("23:59:59"), ScheduleTimeView = "11:59 pm" });

			return time;
		}
	}

	public static class SelectedSeats
	{
		public static List<ClientSpaceSeat> Seats { set; get; }
		public static List<ClientSpaceSeat> SeatsToRemove { set; get; }
		public static List<ClientSpaceSeat> SeatsSpaceBook { set; get; }
		public static List<ClientSpaceSeat> SeatsToRemoveSpaceBook { set; get; }
	}

	public class FacilityVM
	{
		public int FacilityID { get; set; }
		public string FacilityName { get; set; }
		public bool Selected { get; set; }
		public bool IsPaidAmenity { get; set; }
		public double PaidAmenityPrice { get; set; }
	}

	public class FacilityCatVM
	{
		public string CatName { get; set; }
		public List<FacilityVM> FacilityList { set; get; }
	}

	public class DurationType
	{
		public string MembershipDurationType { set; get; }
	}

	public class ScheduleTime
	{
		public int ScheduleTimeID { set; get; }
		public string ScheduleTimeView { set; get; }
		public TimeSpan? ScheduleTimeSpan { set; get; }
	}
}