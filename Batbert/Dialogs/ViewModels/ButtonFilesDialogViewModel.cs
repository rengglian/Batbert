
using Batbert.Interfaces;
using Batbert.Services;
using NLog;
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

        private string buttonName_ = "";
        private List<string> _fileList = new();
        private string _choosenFolder = "";

        public string Title => $"Button {buttonName_} File List Dialog";

        public List<string> FileList
        {
            get => _fileList;
            set => SetProperty(ref _fileList, value);
        }

        public string ChoosenFolder
        {
            get => _choosenFolder;
            set => SetProperty(ref _choosenFolder, value);
        }

        public DelegateCommand ChooseFilesCommand { get; private set; }
        public DelegateCommand ResetFilesCommand { get; private set; }
        public DelegateCommand<string> CloseCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public ButtonFilesDialogViewModel(IAddMp3FilesService addMp3FilesService, ILogger<ButtonFilesDialogViewModel> logger)
        {
            _addMp3FilesService = addMp3FilesService;
            _logger = logger;

            ChooseFilesCommand = new DelegateCommand(ChooseFilesCommandHandler);
            ResetFilesCommand = new DelegateCommand(ResetFilesCommandHanlder);
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
            buttonName_ = parameters.GetValue<string>("buttonName");
            FileList = parameters.GetValue<IEnumerable<string>>("fileList").ToList();
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private void CloseCommandHandler(string parameter)
        {
            var p = new DialogParameters { { "fileList", null } };

            if (parameter.Equals("true"))
            {
                p = new DialogParameters { { "fileList", FileList } };
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
                var tmpList = new List<string>();
                tmpList.AddRange(FileList);
                tmpList.AddRange(_addMp3FilesService.SelectedFileNames.ToList());
                FileList = tmpList;
                ChoosenFolder = _addMp3FilesService.Folder;
            }
            catch (InvalidOperationException e)
            {
                FileList.Add(e.Message);
            }
        }
        private void ResetFilesCommandHanlder()
        {
            FileList = new List<string>();
            ChoosenFolder = "";
        }
    }
}
