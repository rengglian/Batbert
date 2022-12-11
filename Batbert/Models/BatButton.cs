using Batbert.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace Batbert.Models
{
    public class BatButton : BindableBase
    {
        private readonly IDialogService _dialogService;

        private int _fileCount = 0;
        public string SubFolderName { get; } = "";
        public int FileCount
        {
            get => _fileCount;
            set => SetProperty(ref _fileCount, value);
        }

        private List<string> FileList = new();

        public DelegateCommand AddFolderCommand { get; private set; }

        public BatButton(string subFolderName, IDialogService dialogService)
        {
            _dialogService = dialogService;
            SubFolderName = subFolderName;
            AddFolderCommand = new DelegateCommand(AddFolderHandler);
        }

        public void AddFolderHandler()
        {
            _dialogService.ShowButtonFilesDialog(SubFolderName, FileList, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    FileList = r.Parameters.GetValue<IEnumerable<string>> ("fileList").ToList();
                    FileCount = FileList.Count;
                }
            });
        }

        public IEnumerable<string> GetFileList()
        {
            return FileList;
        }
    }
}
