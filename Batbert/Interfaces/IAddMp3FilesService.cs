using System.Collections.Generic;

namespace Batbert.Interfaces
{
    public interface IAddMp3FilesService
    {
        IEnumerable<string> SelectedFileNames { get; }
        string Folder { get; set; }

        void AddFiles();
    }
}