using Batbert.Interfaces;
using Batbert.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Batbert.Dialogs.ViewModels
{
    public class ButtonFilesDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IAddMp3FilesService _addMp3FilesService;
        private readonly ILogger<ButtonFilesDialogViewModel> _logger;

        private string _buttonName = "";
        private List<IButtonContent> _buttonContentList = new();
        private string _choosenFolder = "";

        private int _index = 0;

        public string Title => $"Button {_buttonName} File List Dialog";

        public List<IButtonContent> ButtonContentList
        {
            get => _buttonContentList;
            set => SetProperty(ref _buttonContentList, value);
        }

        public string ChoosenFolder
        {
            get => _choosenFolder;
            set => SetProperty(ref _choosenFolder, value);
        }

        public DelegateCommand ChooseFilesCommand { get; private set; }
        public DelegateCommand ResetFilesCommand { get; private set; }
        public DelegateCommand<string> CloseCommand { get; private set; }
        public DelegateCommand<object> MergeCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public ButtonFilesDialogViewModel(IAddMp3FilesService addMp3FilesService, ILogger<ButtonFilesDialogViewModel> logger)
        {
            _addMp3FilesService = addMp3FilesService;
            _logger = logger;

            ChooseFilesCommand = new DelegateCommand(ChooseFilesCommandHandler);
            ResetFilesCommand = new DelegateCommand(ResetFilesCommandHanlder);
            CloseCommand = new DelegateCommand<string>(CloseCommandHandler);
            MergeCommand = new DelegateCommand<object>(MergeCommandHandler);
        }

        private void MergeCommandHandler(object obj)
        {
            _logger.Information("Trying to merge");
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
            _buttonName = parameters.GetValue<string>("buttonName");
            ButtonContentList = parameters.GetValue<IEnumerable<IButtonContent>>("buttonContent").ToList();
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private void CloseCommandHandler(string parameter)
        {
            var p = new DialogParameters { { "buttonContent", null } };

            if (parameter.Equals("true"))
            {
                p = new DialogParameters { { "buttonContent", ButtonContentList } };
                RaiseRequestClose(new DialogResult(ButtonResult.OK, p));

            }
            else
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel, p));
            }
        }

        private void ChooseFilesCommandHandler()
        {
            try
            {
                _addMp3FilesService.AddFiles();
                var tmpList = new List<IButtonContent>();
                tmpList.AddRange(ButtonContentList);
                tmpList.AddRange(ConvertFileListToButtonContet(_addMp3FilesService.SelectedFileNames.ToList()));
                ButtonContentList = tmpList;
                ChoosenFolder = _addMp3FilesService.Folder;
            }
            catch (InvalidOperationException e)
            {
                ButtonContentList.Add(new ButtonContent {
                    FileName = e.Message, 
                    Index = 0 
                }) ;
            }
        }
        private void ResetFilesCommandHanlder()
        {
            ButtonContentList = new List<IButtonContent>();
            ChoosenFolder = "";
        }

        private IEnumerable<IButtonContent> ConvertFileListToButtonContet(IEnumerable<string> fileList)
        {
            List<ButtonContent> buttonContent = new();
            foreach (var file in fileList) {
                
                buttonContent.Add(new ButtonContent
                {
                    FileName = file,
                    Index = _index
                });
                _index++;
            }
            return buttonContent;
        }
    }
}
