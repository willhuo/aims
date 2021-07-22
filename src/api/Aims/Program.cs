using System;
using System.Reflection;
using Aims.Repo;
using Dijing.Common.Core.Enums;
using Dijing.Common.Core.Utility;
using Dijing.SerilogExt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Aims
{
    /// <summary>
    /// �������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var version = ReflectHelper.GetFileVersion(Assembly.GetExecutingAssembly());
#if RELEASE
            EnvironmentHelper.GetInstance().OSPlatfrom = OSPlatfromEnum.Linux;
            InitLog.SetLog(RunModeEnum.Release);
#elif DEBUG
#pragma warning disable CA1416 // ��֤ƽ̨������
            Console.Title += $"������ý�������[{version}]";
#pragma warning restore CA1416 // ��֤ƽ̨������
            EnvironmentHelper.GetInstance().OSPlatfrom = OSPlatfromEnum.Windows;
            InitLog.SetLog(RunModeEnum.Debug);
#endif
            var host = CreateHostBuilder(args).Build();
            var envName = host.Services.GetRequiredService<IWebHostEnvironment>().EnvironmentName;
            Log.Warning("aims starting up, version={0}, envname={1}", version, envName);
            SeedData.Initialize(host);
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel(x=>x.ListenAnyIP(5000))
                        .UseSerilog();
                });
    }
}
