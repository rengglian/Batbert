﻿using Batbert.Helper;
using Batbert.Interfaces;
using Batbert.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Batbert.Dialogs.ViewModels;

public class ConfirmAndProgressDialogViewModel : BindableBase, IDialogAware
{
    private readonly ILogger<ConfirmAndProgressDialogViewModel> _logger;
    private List<BatButton> _buttonList = new();

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

    public ConfirmAndProgressDialogViewModel(ILogger<ConfirmAndProgressDialogViewModel> logger)
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
        DestinationPath = parameters.GetValue<string>("destination");
        _buttonList = parameters.GetValue<IEnumerable<BatButton>>("buttonList").ToList();
        TotalFiles = _buttonList.Sum(button => button.ButtonContentCount);
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
        await Task.Run(() => DoWork());
        CloseCommandHandler("true");
    }
    private void DoWork()
    {
        foreach (BatButton button in _buttonList)
        {
            int fileNumber = 1;
            string pathString = Path.Combine(DestinationPath, button.SubFolderName);
            _logger.Information($"Create Folder {pathString}");
            _ = Directory.CreateDirectory(pathString);
            var fileList = button.GetButtonContentList();
            var groupedFileList = fileList.GroupBy(buttons => buttons.MergedIndex).Select(content => content.ToList()).ToList();

            foreach (List<IButtonContent> subList in groupedFileList)
            {
                ActualFile = "-i \"" + string.Join("\" -i \"", subList.Select(p => p.FileName)) + "\"";
                string targetFileName = button.SubFolderName.Contains("mp3") ? $"{fileNumber:D4}.mp3" : $"{fileNumber:D3}.mp3";
                fileNumber++;
                string destFile = Path.Combine(pathString, targetFileName);
                _logger.Information($"Copy File [{ActualFile}] to {destFile}");
                var testCommand = FFmpegWrapper.CreateMp3(subList.Select(p => p.FileName), destFile);
                _logger.Information($"Test Command {testCommand}");
                //File.Copy(buttonContent.FileName, destFile, true);
                ActualFileNumber++;
            }
        }
    }
}
