using MakefileRunner;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CoolboyMakeGUI
{
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MakeGUIsettings.json", optional: false, reloadOnChange: true);

            var config = builder.Build();
            Settings = config.GetSection("AppSettings").Get<AppSettings>();
        }
    }

}