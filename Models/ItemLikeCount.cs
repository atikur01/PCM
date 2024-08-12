using System.ComponentModel.DataAnnotations;

namespace PCM.Models
{
    public class ItemLikeCount
    {
        [Key]
        public Guid ItemId { get; set; }    

        public int LikeCount { get; set; }  
    }
}
