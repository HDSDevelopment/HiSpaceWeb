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
	public class ApplicationVariable
	{
		public int _count = 0;
		private static ApplicationVariable _ObjApp;
		private readonly RequestDelegate _next;

		public ApplicationVariable(RequestDelegate next)
		{
			_next = next;
		}

		public ApplicationVariable ReturnObject()
		{
			if (_ObjApp == null)
				_ObjApp = new ApplicationVariable(_next);
			return _ObjApp;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var count = ++this.ReturnObject()._count;
			httpContext.Response.Headers.Append("Count", count.ToString());
			await _next.Invoke(httpContext);
		}
	}

	public sealed class ApplicationState
	{
		public List<ClientSpaceSeat> ReadClientSeats = new List<ClientSpaceSeat>();
		public List<MemberBookingSpace> CartSpaces = new List<MemberBookingSpace>();
		public List<MemberBookingSpaceSeat> CartSeats = new List<MemberBookingSpaceSeat>();
		public List<MemberBookingSpaceSeatViewModel> CartSeatsViewModel = new List<MemberBookingSpaceSeatViewModel>();

		public int? ClientID { set; get; }
		public int? ClientLocationID { set; get; }

		private ApplicationState()
		{
			ReadClientSeats = new List<ClientSpaceSeat>();
			CartSpaces = new List<MemberBookingSpace>();
			CartSeats = new List<MemberBookingSpaceSeat>();
			CartSeatsViewModel = new List<MemberBookingSpaceSeatViewModel>();
		}

		private static readonly object padlock = new object();
		private static ApplicationState instance = null;

		public static ApplicationState Instance
		{
			get
			{
				if (instance == null)
				{
					lock (padlock)
					{
						if (instance == null)
						{
							instance = new ApplicationState();
						}
					}
				}
				return instance;
			}
		}
	}
}