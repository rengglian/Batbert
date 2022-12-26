using Batbert.Interfaces;

namespace Batbert.Models
{
    public class ButtonContent : IButtonContent
    {
        public string FileName { get; set; } = string.Empty;
        public int Index { get; set; } = 0;
    }
}
