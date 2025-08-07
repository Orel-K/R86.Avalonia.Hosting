using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using R86.Avalonia.Hosting;
using SampleHostedApplication.ViewModels;
using SampleHostedApplication.Views;

namespace SampleHostedApplication
{
    public partial class App : HostedApplication<App>
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                /*
                    // Note: App.Services is not available until `StartAsync` is called
                        So, we can not create the MainWindow here as its content is ViewBase<T> that expect
                        `App.Services` to be available

                        desktop.MainWindow = new MainWindow
                        {
                            DataContext = new MainWindowViewModel(),
                        };
                 */
            }

            base.OnFrameworkInitializationCompleted();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
            }

            return base.StartAsync(cancellationToken);
        }
    }
}
