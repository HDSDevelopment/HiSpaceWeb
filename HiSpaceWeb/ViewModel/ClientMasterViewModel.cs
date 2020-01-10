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
using Microsoft.AspNetCore.Http;

namespace HiSpaceWeb.ViewModel
{
    public class ClientMasterViewModel
    {
        public ClientMasterViewModel()
        {
            ClientMaster = new ClientMaster();
			UserLogin = new UserLogin();
		}

        public ClientMaster ClientMaster { get; set; }
        public IFormFile RCCopy { set; get; }
        public IFormFile PANCopy { set; get; }
        public IFormFile GSTCopy { set; get; }
        public IFormFile MembershipAgreementCopy { set; get; }
        public IFormFile ContactPersonAadhaar { set; get; }
        public IFormFile ContactPersonPAN { set; get; }
        public IFormFile Logo { set; get; }
		public UserLogin UserLogin { set; get; }
	}
}
