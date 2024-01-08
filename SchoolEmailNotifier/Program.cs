using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using SchoolEmailNotifier.Application;
using SchoolEmailNotifier.Business;

namespace SchoolEmailNotifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddTransient<CoreApplication>();
                    services.AddTransient<WorksheetBL>();
                    services.AddTransient<StudentWorksheetBL>();
                    services.AddTransient<ValidateTotalAbsentsBL>();
                    services.AddTransient<SendEmailBL>();
                });
    }
}
