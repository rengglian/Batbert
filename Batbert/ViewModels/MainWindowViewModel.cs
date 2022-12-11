﻿using Prism.Commands;
using Prism.Mvvm;
using Batbert.Interfaces;
using System;
using System.IO;
using Batbert.Models;
using System.Collections.Generic;
using Prism.Services.Dialogs;
using System.Linq;
using Batbert.Extensions;

namespace Batbert.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IChooseDestinationFolderService _chooseDestinationFolderService;
        private readonly IDialogService _dialogService;

        private string _title = "BatBert";
        private string _choosenFolder = "";
        private List<BatButton> _batButtons = new();
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string ChoosenFolder
        {
            get => _choosenFolder;
            set => SetProperty(ref _choosenFolder, value);
        }

        public List<BatButton> BatButtons
        {
            get => _batButtons;
            set=> SetProperty(ref _batButtons, value);
        }

        public DelegateCommand ChooseFolderCommand { get; private set; }
        public DelegateCommand WriteDataCommand { get; private set; }

        public MainWindowViewModel(IChooseDestinationFolderService chooseDestinationFolderService, IDialogService dialogService)
        {
            _chooseDestinationFolderService = chooseDestinationFolderService;
            _dialogService = dialogService;

            List<BatButton> tmpButtons = new()
            {
                new BatButton("01", _dialogService),
                new BatButton("02", _dialogService),
                new BatButton("03", _dialogService),
                new BatButton("04", _dialogService),
                new BatButton("05", _dialogService),
                new BatButton("06", _dialogService),
                new BatButton("07", _dialogService),
                new BatButton("08", _dialogService),
                new BatButton("09", _dialogService),
                new BatButton("mp3", _dialogService)
            };

            BatButtons = tmpButtons;
  
            ChooseFolderCommand = new DelegateCommand(ChooseFolderCommandHandler);
            WriteDataCommand = new DelegateCommand(WriteDataCommandHandler);
        }

        private void ChooseFolderCommandHandler()
        {
            try
            {
                _chooseDestinationFolderService.ChooseFolder();
                ChoosenFolder = _chooseDestinationFolderService.Folder;
            }
            catch (InvalidOperationException e) 
            {
                ChoosenFolder = e.Message;
            }
        }

        private void WriteDataCommandHandler()
        {
            _dialogService.ShowConfirmAndProgressDialog(ChoosenFolder, BatButtons, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                }
            });
        }
    }
}