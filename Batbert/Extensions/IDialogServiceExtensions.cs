using Batbert.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Batbert.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowButtonFilesDialog(this IDialogService dialogService, string buttonName, IEnumerable<string> fileList, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "buttonName", buttonName },
                { "fileList", fileList }
            };

            dialogService.ShowDialog("ButtonFilesDialogView", p, callback);
        }

        public static void ShowConfirmAndProgressDialog(this IDialogService dialogService, string destination, IEnumerable<BatButton> buttonList, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "destination", destination },
                { "buttonList", buttonList }
            };

            dialogService.ShowDialog("ConfirmAndProgressDialogView", p, callback);
        }
    }
}
