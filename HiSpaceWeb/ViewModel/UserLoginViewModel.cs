using HiSpaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSpaceWeb.ViewModel
{

	public class UserLoginViewModel
	{
		public UserLoginViewModel()
		{
			UserLogin = new UserLogin();
		}
		public UserLogin UserLogin { set; get; }
		public int ClientMasterID { set; get; }
        public bool IsBooking { set; get; }
    }

}
