using Microsoft.VisualBasic;
using PCM.Models;

namespace PCM.ViewModels
{
    public class ItemDetailsViewModel
    {
        public PCM.Models.Collection? collection { get; set; }  
        public Item? item { get; set; }  
        public string? AuthorName { get; set; } 

    }
}
