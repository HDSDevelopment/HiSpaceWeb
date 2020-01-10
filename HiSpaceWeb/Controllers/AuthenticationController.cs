using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using HiSpaceModels;
using HiSpaceWeb.Utilities;
using HiSpaceService.Models;
using HiSpaceWeb.ViewModel;
using HiSpaceService.ViewModel;

namespace HiSpaceWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication

        public ActionResult Login()
        {
            ViewBag.IsBooking = HttpContext.Request.Query["IsBooking"];
            UserLoginViewModel vModel = new UserLoginViewModel();
            vModel.IsBooking = Convert.ToBoolean(ViewBag.IsBooking);


            if (vModel.IsBooking && GetSessionObject() != null)
            {
                if (ApplicationState.Instance.CartSpaces.Count() > 0)
                {
                    using (var client = new HttpClient())
                    {
                        WorkspaceDetailViewModel vModelWS = new WorkspaceDetailViewModel();
                        client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                        var responseTask = client.GetAsync(Common.Instance.ApiClientGetWorkSpaceDetails + ApplicationState.Instance.CartSpaces[0].ClientSpaceFloorPlanID.ToString());
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsAsync<WorkSpaceDetailsResponse>();
                            readTask.Wait();
                            vModelWS.WorkSpaceDetails = readTask.Result;
                        }

                        //send booking request

                        MemberBookingRequest bookingReuest = new MemberBookingRequest();
                        bookingReuest.memberBookingSpaces = ApplicationState.Instance.CartSpaces;
                        bookingReuest.memberBookingSpaceSeats = ApplicationState.Instance.CartSeats;

                        foreach (var _space in bookingReuest.memberBookingSpaces)
                        {
                            _space.ClientID = vModelWS.WorkSpaceDetails.selectedSpace.ClientID;
                            _space.ClientLocationID = vModelWS.WorkSpaceDetails.selectedSpace.ClientLocationID;
                            _space.MemberID = GetSessionObject().MemberID;
                            _space.SpacePrice = vModelWS.WorkSpaceDetails.selectedSpace.Price;
                            _space.CreatedBy = GetSessionObject().UserID;
                        }

                        foreach (var _memberSpace in bookingReuest.memberBookingSpaces)
                        {
                            using (var client3 = new HttpClient())
                            {
                                client3.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                var postTask3 = client3.PostAsJsonAsync<MemberBookingSpace>(Common.Instance.ApiSpaceBookingRequestSpace, _memberSpace);
                                postTask3.Wait();

                                var result3 = postTask3.Result;
                                if (result3.IsSuccessStatusCode)
                                {
                                    var _newMemberRequestedSpace = result3.Content.ReadAsAsync<MemberBookingSpace>().Result;
                                    foreach (var _seat in bookingReuest.memberBookingSpaceSeats)
                                    {
                                        _seat.MemberBookingSpaceID = _newMemberRequestedSpace.MemberBookingSpaceID;
                                        _seat.CreatedBy = GetSessionObject().UserID;
                                    }

                                    using (var client4 = new HttpClient())
                                    {
                                        client4.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                        var postTask4 = client4.PostAsJsonAsync<List<MemberBookingSpaceSeat>>(Common.Instance.ApiSpaceBookingRequestSpaceSeats, bookingReuest.memberBookingSpaceSeats);
                                        postTask4.Wait();

                                        var result4 = postTask4.Result;
                                        if (result4.IsSuccessStatusCode)
                                        {
                                            //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                            //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                            //return RedirectToAction("Index", "MemberBooking", new { ClientLocationID = spaceVM.ClientLocationID });
                                        }
                                    }

                                    //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                    //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                    //return RedirectToAction("Index", "MemberBooking", new { ClientLocationID = spaceVM.ClientLocationID });
                                }
                            }
                        }
                        //end booking request
                    }

                }
            }

            if (GetSessionObject() != null)
            {
                return RedirectToAction("Index", "Home");
            }

            SetSessionVariables();
            return View(vModel);
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel user)
        {
            //ViewBag.IsBooking = HttpContext.Request.Query["IsBooking"];
            //bool IsBooking = Convert.ToBoolean(ViewBag.IsBooking);
            //user.IsBooking = IsBooking;

            var ssd = user.IsBooking;
            //if (user.IsBooking)
            //    user.signupUser.IsClient = false;
            //else
            //    user.signupUser.IsClient = true;


            UserLogin _user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                //HTTP GET
                var responseTask = client.PostAsJsonAsync(Common.Instance.ApiUserLoginAuthenticateUser, user.UserLogin);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserLogin>();
                    readTask.Wait();

                    _user = readTask.Result;
                }
            }
            if (_user != null && _user.Username == user.UserLogin.Username && _user.Password == user.UserLogin.Password)
            {
                AssignSessionVariables(_user);
                SetSessionVariables();

                if (user.IsBooking)
                {
                    if (ApplicationState.Instance.CartSpaces.Count() > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            WorkspaceDetailViewModel vModel = new WorkspaceDetailViewModel();
                            client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                            var responseTask = client.GetAsync(Common.Instance.ApiClientGetWorkSpaceDetails + ApplicationState.Instance.CartSpaces[0].ClientSpaceFloorPlanID.ToString());
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var readTask = result.Content.ReadAsAsync<WorkSpaceDetailsResponse>();
                                readTask.Wait();
                                vModel.WorkSpaceDetails = readTask.Result;
                            }

                            //send booking request

                            MemberBookingRequest bookingReuest = new MemberBookingRequest();
                            bookingReuest.memberBookingSpaces = ApplicationState.Instance.CartSpaces;
                            bookingReuest.memberBookingSpaceSeats = ApplicationState.Instance.CartSeats;

                            foreach (var _space in bookingReuest.memberBookingSpaces)
                            {
                                _space.ClientID = vModel.WorkSpaceDetails.selectedSpace.ClientID;
                                _space.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID;
                                _space.MemberID = _user.MemberID;
                                _space.SpacePrice = vModel.WorkSpaceDetails.selectedSpace.Price;
                                _space.CreatedBy = _user.UserID;
                            }

                            foreach (var _memberSpace in bookingReuest.memberBookingSpaces)
                            {
                                using (var client3 = new HttpClient())
                                {
                                    client3.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                    var postTask3 = client3.PostAsJsonAsync<MemberBookingSpace>(Common.Instance.ApiSpaceBookingRequestSpace, _memberSpace);
                                    postTask3.Wait();

                                    var result3 = postTask3.Result;
                                    if (result3.IsSuccessStatusCode)
                                    {
                                        var _newMemberRequestedSpace = result3.Content.ReadAsAsync<MemberBookingSpace>().Result;
                                        foreach (var _seat in bookingReuest.memberBookingSpaceSeats)
                                        {
                                            _seat.MemberBookingSpaceID = _newMemberRequestedSpace.MemberBookingSpaceID;
                                            _seat.CreatedBy = _user.UserID;
                                        }

                                        using (var client4 = new HttpClient())
                                        {
                                            client4.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                            var postTask4 = client4.PostAsJsonAsync<List<MemberBookingSpaceSeat>>(Common.Instance.ApiSpaceBookingRequestSpaceSeats, bookingReuest.memberBookingSpaceSeats);
                                            postTask4.Wait();

                                            var result4 = postTask4.Result;
                                            if (result4.IsSuccessStatusCode)
                                            {
                                                //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                                //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                                //return RedirectToAction("Index", "MemberBooking", new { ClientLocationID = spaceVM.ClientLocationID });
                                            }
                                        }

                                        //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                        //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                        //return RedirectToAction("Index", "MemberBooking", new { ClientLocationID = spaceVM.ClientLocationID });
                                    }
                                }
                            }
                            //end booking request
                        }

                    }
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewData["loginError"] = "err = Incorrect username or password.";
                return View("Login", user);
            }

            return View();
        }

        public ActionResult Signup()
        {
            ViewBag.IsBooking = HttpContext.Request.Query["IsBooking"];
            SignupViewModel vModel = new SignupViewModel();
            vModel.IsBooking = Convert.ToBoolean(ViewBag.IsBooking);

            SetSessionVariables();
            return View(vModel);
        }

        [HttpPost]
        public ActionResult Signup(SignupViewModel newSignupUser)
        {
            SetSessionVariables();

            //ViewBag.IsBooking = HttpContext.Request.Query["IsBooking"];
            //bool IsBooking = Convert.ToBoolean(ViewBag.IsBooking);
            if (newSignupUser.IsBooking)
                newSignupUser.signupUser.IsClient = false;
            else
                newSignupUser.signupUser.IsClient = true;

            if (!newSignupUser.IsBooking)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<SignupUser>(Common.Instance.ApiUserLoginSignupUser, newSignupUser.signupUser);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<bool>();
                        readTask.Wait();
                        bool rs = readTask.Result;

                        if (rs)
                        {
                            UserLogin login = new UserLogin() { Username = newSignupUser.signupUser.Username, Password = newSignupUser.signupUser.Password, UserType = (newSignupUser.signupUser.IsClient ? 2 : 4) };
                            AssignSessionVariables(login);
                            SetSessionVariables();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Username already exist");
                            return View("Signup", newSignupUser);
                        }
                    }
                }
            }
            else
            {
                if (newSignupUser != null)
                {
                    MemberMaster NewMember = new MemberMaster();
                    UserLogin NewUserLogin = new UserLogin();

                    if (ApplicationState.Instance.CartSpaces.Count() > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            WorkspaceDetailViewModel vModel = new WorkspaceDetailViewModel();
                            client.BaseAddress = new Uri(Common.Instance.ApiClientControllerName);
                            var responseTask = client.GetAsync(Common.Instance.ApiClientGetWorkSpaceDetails + ApplicationState.Instance.CartSpaces[0].ClientSpaceFloorPlanID.ToString());
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var readTask = result.Content.ReadAsAsync<WorkSpaceDetailsResponse>();
                                readTask.Wait();
                                vModel.WorkSpaceDetails = readTask.Result;
                            }

                            //ApplicationState.Instance.CartSpaces[0].ClientSpaceFloorPlanID

                            NewMember.ClientID = vModel.WorkSpaceDetails.selectedSpace.ClientID;
                            NewMember.MemberStatus = true;
                            NewMember.MemberName = newSignupUser.signupUser.Username;
                            NewMember.CreatedBy = 1;
                            NewMember.CreatedDateTime = DateTime.Now;

                            using (var client1 = new HttpClient())
                            {
                                client1.BaseAddress = new Uri(Common.Instance.ApiMemberControllerName);

                                //HTTP POST
                                var postTask1 = client1.PostAsJsonAsync<MemberMaster>(Common.Instance.ApiMemberAddMemberMaster, NewMember);
                                postTask1.Wait();

                                var result1 = postTask1.Result;
                                if (result1.IsSuccessStatusCode)
                                {
                                    var rs = result1.Content.ReadAsAsync<MemberMaster>().Result;
                                    NewUserLogin.MemberID = rs.MemberID;
                                    NewUserLogin.Username = newSignupUser.signupUser.Username;
                                    NewUserLogin.Password = newSignupUser.signupUser.Password;
                                    NewUserLogin.Active = true;
                                    NewUserLogin.UserType = 4;
                                    NewUserLogin.ClientID = NewMember.ClientID;

                                    using (var client2 = new HttpClient())
                                    {
                                        //userLogin section added                            
                                        client2.BaseAddress = new Uri(Common.Instance.ApiUserLoginUserLoginControllerName);
                                        var postTask2 = client2.PostAsJsonAsync<UserLogin>(Common.Instance.ApiUserLoginAddUserLogin, NewUserLogin);
                                        postTask2.Wait();

                                        var result2 = postTask2.Result;
                                        if (result2.IsSuccessStatusCode)
                                        {
                                            var rs1 = result2.Content.ReadAsAsync<UserLogin>();
                                            NewUserLogin = rs1.Result;
                                            AssignSessionVariables(NewUserLogin);
                                            SetSessionVariables();

                                            //send booking request

                                            MemberBookingRequest bookingReuest = new MemberBookingRequest();
                                            bookingReuest.memberBookingSpaces = ApplicationState.Instance.CartSpaces;
                                            bookingReuest.memberBookingSpaceSeats = ApplicationState.Instance.CartSeats;

                                            foreach (var _space in bookingReuest.memberBookingSpaces)
                                            {
                                                _space.ClientID = vModel.WorkSpaceDetails.selectedSpace.ClientID;
                                                _space.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID;
                                                _space.MemberID = NewUserLogin.MemberID;
                                                _space.SpacePrice = vModel.WorkSpaceDetails.selectedSpace.Price;
                                                _space.CreatedBy = NewUserLogin.UserID;
                                            }

                                            foreach (var _memberSpace in bookingReuest.memberBookingSpaces)
                                            {
                                                using (var client3 = new HttpClient())
                                                {
                                                    client3.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                                    var postTask3 = client3.PostAsJsonAsync<MemberBookingSpace>(Common.Instance.ApiSpaceBookingRequestSpace, _memberSpace);
                                                    postTask3.Wait();

                                                    var result3 = postTask3.Result;
                                                    if (result3.IsSuccessStatusCode)
                                                    {
                                                        var _newMemberRequestedSpace = result3.Content.ReadAsAsync<MemberBookingSpace>().Result;
                                                        foreach (var _seat in bookingReuest.memberBookingSpaceSeats)
                                                        {
                                                            _seat.MemberBookingSpaceID = _newMemberRequestedSpace.MemberBookingSpaceID;
                                                            _seat.CreatedBy = NewUserLogin.UserID;
                                                        }

                                                        using (var client4 = new HttpClient())
                                                        {
                                                            client4.BaseAddress = new Uri(Common.Instance.ApiSpaceBookingControllerName);
                                                            var postTask4 = client4.PostAsJsonAsync<List<MemberBookingSpaceSeat>>(Common.Instance.ApiSpaceBookingRequestSpaceSeats, bookingReuest.memberBookingSpaceSeats);
                                                            postTask4.Wait();

                                                            var result4 = postTask4.Result;
                                                            if (result4.IsSuccessStatusCode)
                                                            {
                                                                //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                                                //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                                                return RedirectToAction("Index", "Home");
                                                            }
                                                        }

                                                        //SpaceBookingViewModel spaceVM = new SpaceBookingViewModel();
                                                        //spaceVM.ClientLocationID = vModel.WorkSpaceDetails.selectedSpace.ClientLocationID.Value;
                                                        //return RedirectToAction("Index", "MemberBooking", new { ClientLocationID = spaceVM.ClientLocationID });
                                                    }
                                                }
                                            }
                                            //end booking request
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }


            return View();
        }

        public ActionResult ForgotPassword()
        {
            SetSessionVariables();

            return View();
        }

        public void SetSessionVariables()
        {
            #region

            UserLogin rs = HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
            ViewBag.Username = HttpContext.Session.GetString(Common.SessionUsername);
            ViewBag.Type = HttpContext.Session.GetInt32(Common.SessionType);
            ViewBag.UserID = HttpContext.Session.GetInt32(Common.SessionUserID);
            ViewBag.ClientID = HttpContext.Session.GetInt32(Common.SessionClientID);
            ViewBag.MemberID = HttpContext.Session.GetInt32(Common.SessionMemberID);
            ViewBag.ClientLocationID = HttpContext.Session.GetInt32(Common.SessionClientLocationID);

            #endregion
        }

        public void AssignSessionVariables(UserLogin _user)
        {
            HttpContext.Session.SetObjectAsJson("_user", _user);

            if (_user.ClientID != null)
            {
                int? ClientID = _user.ClientID;
                var _ClientID = ClientID.Value;
                HttpContext.Session.SetInt32(Common.SessionClientID, _ClientID);
            }
            if (_user.MemberID != null)
            {
                int? MemberID = _user.MemberID;
                var _MemberID = MemberID.Value;
                HttpContext.Session.SetInt32(Common.SessionMemberID, _MemberID);
            }
            if (_user.ClientLocationID != null)
            {
                int? ClientLocationID = _user.ClientLocationID;
                var _ClientLocationID = ClientLocationID.Value;
                HttpContext.Session.SetInt32(Common.SessionMemberID, _ClientLocationID);
            }

            HttpContext.Session.SetString(Common.SessionUsername, _user.Username);
            HttpContext.Session.SetInt32(Common.SessionType, _user.UserType);
            HttpContext.Session.SetInt32(Common.SessionUserID, _user.UserID);
        }

        public UserLogin GetSessionObject()
        {
            return HttpContext.Session.GetObjectFromJson<UserLogin>("_user");
        }
    }
}