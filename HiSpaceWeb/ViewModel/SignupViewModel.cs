using HiSpaceModels;
using HiSpaceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.ViewModel
{

	public class SignupViewModel
	{
		public SignupViewModel()
		{
            signupUser = new SignupUser();
		}
		public SignupUser signupUser { set; get; }		
        public bool IsBooking { set; get; }
    }

}
