using Batbert.Interfaces;
using Microsoft.Win32;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Batbert.Services
{
    public class AddMp3FilesService : BindableBase, IAddMp3FilesService
    {
        readonly OpenFileDialog _openFileDialog = new();
        private List<string> FileList = new();

        private string _folder = "";
        public string Folder
        {
            get => _folder;
            set => SetProperty(ref _folder, value);
        }

        public void AddFiles()
        {
            _openFileDialog.Filter = "MP3 Files(*.MP3)|*.MP3|All files (*.*)|*.*";
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.Multiselect = true;
            _openFileDialog.RestoreDirectory = true;
            _openFileDialog.CheckFileExists = false;

            string defaultFilename = "All MP3 files";
            _openFileDialog.FileName = defaultFilename;

            var choosenFile = _openFileDialog.ShowDialog();
            if (choosenFile.HasValue && choosenFile.Value)
            {
                if (_openFileDialog.FileName.Contains(defaultFilename))
                {
                    int pos = _openFileDialog.FileName.IndexOf(defaultFilename);
                    Folder = _openFileDialog.FileName.Remove(pos);
                    FileList = Directory.GetFiles(Folder, "*.mp3").ToList();
                } else
                {
                    FileList = _openFileDialog.FileNames.ToList();
                }
                
            }
        }

        public IEnumerable<string> SelectedFileNames
        {
            get => FileList;
        }
    }
}
