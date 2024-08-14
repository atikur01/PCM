namespace PCM.Models
{
    public class Comment
    {
        public Guid CommentID { get; set; } 
        public Guid? ItemId { get; set; }
        public Item Item { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        




    }
}
