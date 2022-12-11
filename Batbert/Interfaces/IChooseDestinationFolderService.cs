namespace Batbert.Interfaces
{
    public interface IChooseDestinationFolderService
    {
        string Folder { get; set; }

        void ChooseFolder();
    }
}