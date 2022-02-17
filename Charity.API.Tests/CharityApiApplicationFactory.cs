using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Charity.API.Controllers;

namespace Charity.API.Tests
{
    public class CharityApiApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(collection =>
            {
                var controllerAssemblyName = typeof(AdminController).Assembly.FullName;
                collection.AddMvc().AddApplicationPart(Assembly.Load(controllerAssemblyName));
            });
            return base.CreateHost(builder);
        }
    }
}