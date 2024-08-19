using PCM.Models;

namespace PCM.ViewModels
{
    public class SearchViewModel
    {
       public List<Item>? Items { get; set; }

        public List<Collection>? Collections { get; set; }

        public string? Keyword { get; set; } 
    }
}
