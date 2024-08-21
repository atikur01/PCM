using Nest;
using PCM.Models;

namespace PCM.ElasticSearchModels
{
    [ElasticsearchType(RelationName = "comment")]
    public class EsComment
    {

        [PropertyName("item_id")]
        public Guid ItemId { get; set; }

        [PropertyName("comment_id")]
        public Guid CommentID { get; set; }

        [Ignore]
        public Item Item { get; set; }

        [PropertyName("user_name")]
        public string UserName { get; set; }

        [PropertyName("message")]
        public string Message { get; set; }

        [PropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
