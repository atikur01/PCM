using System.ComponentModel.DataAnnotations;

namespace PCM.Models
{
    public class ItemLikeCount
    {
        [Key]
        public Guid ItemLikeCountId { get; set; }
        public Guid ItemId { get; set; }    
        public Item Item { get; set; }  
        public int LikeCount { get; set; }  
    }
}
