using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

namespace RecoilNet
{
    /// <summary>
    /// Used to setup XUnit dependency injection and logging for tests.
    /// </summary>
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging(logging =>
            {
                logging.AddXunitOutput();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<RecoilStore>();
            services.AddTransient<IRecoilStore>(provider => provider.GetRequiredService<RecoilStore>());
        }
    }
}
