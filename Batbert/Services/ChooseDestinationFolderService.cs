using Batbert.Interfaces;
using Microsoft.Win32;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;

namespace Batbert.Services
{
    public class ChooseDestinationFolderService : BindableBase, IChooseDestinationFolderService
    {
        private readonly ILogger<ChooseDestinationFolderService> _logger;
        readonly OpenFileDialog _openFileDialog = new();

        private string _folder = "";
        public string Folder
        {
            get => _folder;
            set => SetProperty(ref _folder, value);
        }

        public ChooseDestinationFolderService(ILogger<ChooseDestinationFolderService> logger)
        {
            _logger = logger;
        }

        public void ChooseFolder()
        {
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.RestoreDirectory = true;
            _openFileDialog.CheckFileExists = false;

            string defaultFilename = "Select a destination folder";
            _openFileDialog.FileName = defaultFilename;

            var choosenFile = _openFileDialog.ShowDialog();
            if (choosenFile.HasValue && choosenFile.Value)
            {
                
                if (!_openFileDialog.FileName.Contains(defaultFilename))
                {
                    throw new InvalidOperationException("Not a folder was selected");
                }
                int pos = _openFileDialog.FileName.IndexOf(defaultFilename);
                Folder = _openFileDialog.FileName.Remove(pos);
                var filePaths = Directory.GetFiles(Folder).ToList();
                var dirPaths = Directory.GetDirectories(Folder).ToList();
                dirPaths.RemoveAll(x => x.Contains("System Volume Information"));
                if (filePaths.Count > 0 || dirPaths.Count > 0)
                {
                    _logger.Error($"filePaths: {string.Join(" ", filePaths.ToArray())} / dirPaths: {string.Join(" ", dirPaths.ToArray())}");
                    
                    throw new InvalidOperationException("Folder is not empty");
                }
            }
        }
    }
}
