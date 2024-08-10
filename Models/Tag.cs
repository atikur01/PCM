using System.ComponentModel.DataAnnotations;

namespace PCM.Models
{
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; }   
        
        public Guid ItemId { get; set; }   
        
        public Item? Item { get; set; } 

        public string? Name { get; set; }     
 
    }
}
