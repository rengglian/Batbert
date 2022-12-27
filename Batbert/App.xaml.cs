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
using NLog.Targets;
using NLog;
using DryIoc;
using System.IO;
using System;
using System.Linq;
using Prism.Services.Dialogs;
using Batbert.Extensions;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Batbert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private ILogger<App> _logger;
        public static IConfiguration Config { get; private set; }

        protected override Window CreateShell()
        {
            LoadAppsettings();

            LoggerConfiguration();

            _logger.Information("Start Application");

            // Stop things from shutting down when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialogService = Container.Resolve<IDialogService>();

            var ffmpeg_path = Config.GetSection("ffmpeg:ExecPath").Value;
            var ffmpeg_key = Config.GetSection("ffmpeg").Key;
            if (!Directory.Exists(ffmpeg_path) || Directory.GetFiles(ffmpeg_path, "ffmpeg.exe", SearchOption.AllDirectories).Length == 0)
            {
                dialogService.ShowDownLoadFFmpegDialog(r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {

                    }
                });
            } else
            {
                _logger.Information($"{ffmpeg_key} correct installed");
            }
               
            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Loaded += (_, __) =>
            {
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Activate();
            };
            return mainWindow;
        }
            
        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            
            if (Current.MainWindow != null)
            { 
                Current.MainWindow.Show();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<ChooseDestinationFolderService>(typeof(IChooseDestinationFolderService));
            containerRegistry.RegisterManySingleton<AddMp3FilesService>(typeof(IAddMp3FilesService));

            containerRegistry.RegisterDialog<ButtonFilesDialogView, ButtonFilesDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmAndProgressDialogView, ConfirmAndProgressDialogViewModel>();
            containerRegistry.RegisterDialog<DownLoadFFmpegDialogView, DownLoadFFmpegDialogViewModel>();

            containerRegistry.RegisterManySingleton(typeof(NLogService<>), typeof(ILogger<>));

            _logger = containerRegistry.GetContainer().Resolve<ILogger<App>>();
        }

        private static void LoggerConfiguration()
        {
            LoggingConfiguration config = new();

            FileTarget fileTargetTraceLog = new()
            {
                FileName = @"${basedir}trace.log",
                Layout = "${longdate}|${level:uppercase=true}|${threadname:whenEmpty=${threadid}}|${message:withexception=true}",
                CreateDirs = true,
                ArchiveOldFileOnStartup = true,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMdd_HHmmss"
            };

            config.AddTarget("tracelogfile", fileTargetTraceLog);

            LoggingRule ruleTrace = new("*", LogLevel.Trace, fileTargetTraceLog)
            {
                FilterDefaultAction = FilterResult.Log
            };

            config.LoggingRules.Add(ruleTrace);

            LogManager.Configuration = config;
        }

        private static void LoadAppsettings()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
