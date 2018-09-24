using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LabWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser(with => {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out;
            });
            var result = parser.ParseArguments<AppOptions>(args);

            result.MapResult(
              options =>
              {
                  if (options.Host.ToLower() == "service-fabric-host")
                  {
                      BuildWebHost(options).Run();
                  }
                  else
                  {
                      BuildWebHost(args).Run();
                  }
                  return 0;
              },
              errors =>
              {
                  return 1;
              });
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseApplicationInsights()
                   .Build();

        public static IWebHost BuildWebHost(AppOptions argOptions) =>
                                WebHost.CreateDefaultBuilder()
                                        .UseStartup<Startup>()
                                        .UseApplicationInsights()
                                        .UseHttpSys(options =>
                                        {
                                            options.UrlPrefixes.Add($"{argOptions.Protocol}://+:{argOptions.Port}/{argOptions.EndPoint}");
                                        }).Build();
    }

    public class AppOptions
    {
        [Option(Default = "self-host", HelpText = "The target host - Options [self-host] or [service-fabric-host]")]
        public string Host { get; set; }
        [Option(Default = "http", HelpText = "The target protocol - Options [http] or [https]")]
        public string Protocol { get; set; }
        [Option(Default = "localhost", HelpText = "The target IP Address or Uri - Example [localhost] or [127.0.0.1]")]
        public string IpAddressOrFQDN { get; set; }
        [Option(Default = "5000", HelpText = "The target port - Example [80] or [5000]")]
        public string Port { get; set; }
        [Option(Default = "LabApp", HelpText = "The target port - Example [80] or [5000]")]
        public string EndPoint { get; set; }
    }
}
