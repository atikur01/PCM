using System.ComponentModel.DataAnnotations;

namespace PCM.Models
{
    public class Category
    {
        [Key]
        public string Name { get; set; }    
    }
}
