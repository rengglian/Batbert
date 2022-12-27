using Batbert.Helper;
using Batbert.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace Batbert.Dialogs.ViewModels
{
    public class DownLoadFFmpegDialogViewModel : BindableBase, IDialogAware
    {
        private readonly string _ffmpeg_key = App.Config.GetSection("ffmpeg").Key;
        private readonly string _ffmpeg_path = App.Config.GetSection("ffmpeg:ExecPath").Value;
        private readonly string _fileUrl = App.Config.GetSection("ffmpeg:DownloadUrl").Value;
     
        private readonly ILogger<DownLoadFFmpegDialogViewModel> _logger;

        private string _destinationPath = "";
        private string _destinationFile = "";
        private string _actualFile = "";
        private int _totalFiles = 0;
        private int _actualFileNumber = 0;

        public string DestinationPath
        {
            get => _destinationPath;
            set => SetProperty(ref _destinationPath, value);
        }
        public string DestinationFile
        {
            get => _destinationFile;
            set => SetProperty(ref _destinationFile, value);
        }
        public string ActualFile
        {
            get => _actualFile;
            set => SetProperty(ref _actualFile, value);
        }

        public int TotalFiles
        {
            get => _totalFiles;
            set => SetProperty(ref _totalFiles, value);
        }

        public int ActualFileNumber
        {
            get => _actualFileNumber;
            set => SetProperty(ref _actualFileNumber, value);
        }
        public string Title => "Confirm and Copy Files";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand ConfirmAndStartCommand { get; private set; }
        public DelegateCommand<string> CloseCommand { get; private set; }

        public DownLoadFFmpegDialogViewModel(ILogger<DownLoadFFmpegDialogViewModel> logger)
        {
            _logger = logger;
            ConfirmAndStartCommand = new DelegateCommand(ConfirmAndStartCommandHandler);
            CloseCommand = new DelegateCommand<string>(CloseCommandHandler);          
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            DestinationPath = AppDomain.CurrentDomain.BaseDirectory + _ffmpeg_key;
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private void CloseCommandHandler(string parameter)
        {
            var p = new DialogParameters {};

            if (parameter.Equals("true"))
            {
                RaiseRequestClose(new DialogResult(ButtonResult.OK, p));
            }
            else
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel, p));
            }
        }

        private async void ConfirmAndStartCommandHandler()
        {
            await Task.Run(() => DoDownloadWorkAsync());
            DoExtractWork();
        
            CloseCommandHandler("true");
        
        }

        private async Task DoDownloadWorkAsync()
        {
            if (!Directory.Exists(DestinationPath))
            {
                _logger.Information($"Create folder for {_ffmpeg_key}");
                Directory.CreateDirectory(DestinationPath);
            }

            var httpClient = new HttpClient();
            var httpResult = await httpClient.GetAsync(_fileUrl);
            using var resultStream = await httpResult.Content.ReadAsStreamAsync();
            DestinationFile = Path.Combine(DestinationPath, "ffmpeg.zip");
            using var fileStream = File.Create(DestinationFile);
            resultStream.CopyTo(fileStream);
        }

        private void DoExtractWork()
        {
            ZipFile.ExtractToDirectory(DestinationFile, DestinationPath, true);
            File.Delete(DestinationFile);
            var versionString = CmdHelper.Execute(Path.Combine(_ffmpeg_path, "ffmpeg.exe"), "-version");
            _logger.Information($"Check Version: {versionString.Substring(0, versionString.IndexOf(Environment.NewLine))}");
        }
    }
}
