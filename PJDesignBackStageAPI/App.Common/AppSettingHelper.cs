using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class AppSettingHelper
    {
        public static IConfigurationSection GetSection(string sectionName)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var fileName = env == null ? "appsettings.json" : $"appsettings.{env}.json";

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(fileName);
            var config = builder.Build();

            return config.GetSection(sectionName); ;
        }
    }
}
