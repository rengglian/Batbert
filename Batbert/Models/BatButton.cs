using Batbert.Extensions;
using Batbert.Interfaces;
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

        private int _buttonContentCount = 0;
        public string SubFolderName { get; } = "";
        public int ButtonContentCount
        {
            get => _buttonContentCount;
            set => SetProperty(ref _buttonContentCount, value);
        }

        private List<IButtonContent> ButtonContentList = new();

        public DelegateCommand AddFolderCommand { get; private set; }

        public BatButton(string subFolderName, IDialogService dialogService)
        {
            _dialogService = dialogService;
            SubFolderName = subFolderName;
            AddFolderCommand = new DelegateCommand(AddFolderHandler);
        }

        public void AddFolderHandler()
        {
            _dialogService.ShowButtonFilesDialog(SubFolderName, ButtonContentList, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    ButtonContentList = r.Parameters.GetValue<IEnumerable<IButtonContent>> ("buttonContent").ToList();
                    ButtonContentCount = ButtonContentList.Max(b => b.MergedIndex) + 1;
                }
            });
        }

        public IEnumerable<IButtonContent> GetButtonContentList()
        {
            return ButtonContentList;
        }
    }
}
