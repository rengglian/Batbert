using Batbert.Views;
using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using Batbert.Interfaces;
using Batbert.Services;
using Batbert.Dialogs.Views;
using Batbert.Dialogs.ViewModels;
using NLog.Config;
using NLog.Filters;
using NLog.Layouts;
using NLog.Targets;
using NLog;
using DryIoc;

namespace Batbert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private ILogger<App> _logger;
        protected override Window CreateShell()
            => Container.Resolve<MainWindow>();

        protected override void InitializeShell(Window shell)
        {
            LoggerConfiguration();

            base.InitializeShell(shell);

            if (Current.MainWindow != null)
            {
                _logger.Information("Start Application");

                Current.MainWindow.Show();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<ChooseDestinationFolderService>(typeof(IChooseDestinationFolderService));
            containerRegistry.RegisterManySingleton<AddMp3FilesService>(typeof(IAddMp3FilesService));

            containerRegistry.RegisterDialog<ButtonFilesDialogView, ButtonFilesDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmAndProgressDialogView, ConfirmAndProgressDialogViewModel>();

            containerRegistry.RegisterManySingleton(typeof(NLogService<>), typeof(ILogger<>));

            _logger = containerRegistry.GetContainer().Resolve<ILogger<App>>();
        }

        public static void LoggerConfiguration()
        {
            LoggingConfiguration config = new();

            FileTarget fileTargetTraceLog = new FileTarget
            {
                FileName = @"${basedir}trace.log",
                Layout = "${longdate}|${level:uppercase=true}|${threadname:whenEmpty=${threadid}}|${message:withexception=true}",
                CreateDirs = true,
                ArchiveOldFileOnStartup = true,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMdd_HHmmss"
            };

            config.AddTarget("tracelogfile", fileTargetTraceLog);

            LoggingRule ruleTrace = new("*", LogLevel.Trace, fileTargetTraceLog);

            ruleTrace.FilterDefaultAction = FilterResult.Log;

            config.LoggingRules.Add(ruleTrace);

            LogManager.Configuration = config;
        }
    }
}
