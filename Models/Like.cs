using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Models
{
    public class Like
    {
        [Key]
        public Guid LikeID { get; set; }
        public Guid? ItemId { get; set; }
        public Guid? VisitorUserID { get; set; }    
        public Item Item { get; set; }

    }
}
