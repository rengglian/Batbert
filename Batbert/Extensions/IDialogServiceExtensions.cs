using Batbert.Interfaces;
using Batbert.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Batbert.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowButtonFilesDialog(this IDialogService dialogService, string buttonName, IEnumerable<IButtonContent> buttonContent, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "buttonName", buttonName },
                { "buttonContent", buttonContent }
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

        public static void ShowDownLoadFFmpegDialog(this IDialogService dialogService, Action<IDialogResult> callback)
        {
            dialogService.ShowDialog("DownLoadFFmpegDialogView", callback);
        }
    }
}
