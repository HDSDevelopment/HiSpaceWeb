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
        }

        #endregion

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

        #endregion

        #region Session Variables

        public const string SessionUsername = "_Username";
        public const string SessionType = "_Type";
        public const string SessionUserID = "_UserID";
        public const string SessionClientID = "_ClientID";
        public const string SessionMemberID = "_MemberID";
        public const string SessionClientLocationID = "_ClientLocationID";

        #endregion


        #region API Address             

        #endregion

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

        #endregion

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

        public string ApiClientGetClientSpaceSeats = "GetClientSpaceSeats/";
        public string ApiClientGetClientSpaceSeat = "GetClientSpaceSeat/";
        public string ApiClientAddClientSpaceSeat = "AddClientSpaceSeat/";
        public string ApiClientUpdateClientSpaceSeats = "UpdateClientSpaceSeats/";
        public string ApiClientDeleteClientSpaceSeats = "DeleteClientSpaceSeats/";

        public string ApiClientGetClientMembershipPlans = "GetClientMembershipPlans/";
        public string ApiClientGetClientMembershipPlanDetails = "GetClientMembershipPlanDetails/";
        public string ApiClientAddEditClientMembershipPlan = "AddEditClientMembershipPlan/";
        public string ApiClientAddClientMembershipPlanHistory = "AddClientMembershipPlanHistory/";
        public string ApiClientAddMembershipPlanHistory = "AddMembershipPlanHistory/";
        public string ApiClientGetMembershipPlanHistories = "GetMembershipPlanHistories/";



        #endregion

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
        public string ApiCommonGetMembershipPlans = "GetMembershipPlans/";
        public string ApiCommonGetScaleMetrics = "GetAllScaleMetrics/";
        public string ApiCommonGetPendingNotification = "GetPendingNotification/";
        public string ApiCommonGetAllClientLocationSearch = "GetAllClientLocationSearch";
        public string ApiCommonGetAllAmenitiesSearch = "GetAllAmenitiesSearch";
        public string ApiCommonGetAllWorkSpaceTypesSearch = "GetAllWorkSpaceTypesSearch";


        #endregion

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


        #endregion

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


        #endregion

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

        #endregion

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


        #endregion

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

        #endregion


        #endregion

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
}
