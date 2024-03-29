﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YummyMummy.Data;
using YummyMummy.Models;

namespace YummyMummy
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<AppIdentityDbContext>();
					var userManager = services.GetRequiredService<UserManager<AppUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					// insertt inital data for app identity
					DbInitializer.Initialize(context, userManager, roleManager).Wait();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}
			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
			    .UseDefaultServiceProvider(options=>options.ValidateScopes=false)  //disable Scope Verification
				.Build();
	}
}
