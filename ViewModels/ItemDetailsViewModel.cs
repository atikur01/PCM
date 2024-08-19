using Microsoft.VisualBasic;
using PCM.Models;

namespace PCM.ViewModels
{
    public class ItemDetailsViewModel
    {
        public PCM.Models.Collection? collection { get; set; }  
        public Item? item { get; set; }  
        public string? AuthorName { get; set; } 
        public Like? Like { get; set; }
        public ItemLikeCount ItemLikeCount { get; set; }
        public List<string>? Tags { get; set; }  
        public string? CommenterName { get; set; }   

    }
}
