using PCM.Models;

namespace PCM.ViewModels
{
    public class HomeViewModel
    {
        public List<Item>? Items { get; set; }

        public List<Collection>? Collections { get; set; }   

        public Dictionary<string, string>? Tags { get; set; } 
    }
}
