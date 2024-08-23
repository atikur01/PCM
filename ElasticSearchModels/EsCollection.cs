using Nest;
using PCM.Models;

namespace PCM.ElasticSearchModels
{
    [ElasticsearchType(RelationName = "collection")]
    public class EsCollection
    {
        //Keyword fields are not analyzed in ElasticSearch
        [Keyword(Name = "collection_id")]
        public Guid CollectionId { get; set; }

        [Keyword(Name = "user_id")]
        public Guid UserId { get; set; }

        [Ignore]
        public User? User { get; set; }

        //Text fields are analyzed in ElasticSearch
        [Text(Name = "name")]
        public string? Name { get; set; }

        [Text(Name = "description")]
        public string? Description { get; set; }

        [Keyword(Name = "image_url")]
        public string? ImageUrl { get; set; }

        [Keyword(Name = "category_name")]
        public string? CategoryName { get; set; }

        [Date(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [Number(NumberType.Integer, Name = "total_items")]
        public int? TotalItems { get; set; }

        // Custom Field Names
        [Text(Name = "custom_string1_name")]
        public string? CustomString1Name { get; set; }

        [Text(Name = "custom_string2_name")]
        public string? CustomString2Name { get; set; }

        [Text(Name = "custom_string3_name")]
        public string? CustomString3Name { get; set; }

        [Text(Name = "custom_int1_name")]
        public string? CustomInt1Name { get; set; }

        [Text(Name = "custom_int2_name")]
        public string? CustomInt2Name { get; set; }

        [Text(Name = "custom_int3_name")]
        public string? CustomInt3Name { get; set; }

        [Text(Name = "custom_multiline_text1_name")]
        public string? CustomMultilineText1Name { get; set; }

        [Text(Name = "custom_multiline_text2_name")]
        public string? CustomMultilineText2Name { get; set; }

        [Text(Name = "custom_multiline_text3_name")]
        public string? CustomMultilineText3Name { get; set; }

        [Text(Name = "custom_boolean1_name")]
        public string? CustomBoolean1Name { get; set; }

        [Text(Name = "custom_boolean2_name")]
        public string? CustomBoolean2Name { get; set; }

        [Text(Name = "custom_boolean3_name")]
        public string? CustomBoolean3Name { get; set; }

        [Text(Name = "custom_date1_name")]
        public string? CustomDate1Name { get; set; }

        [Text(Name = "custom_date2_name")]
        public string? CustomDate2Name { get; set; }

        [Text(Name = "custom_date3_name")]
        public string? CustomDate3Name { get; set; }

        //Maintain relationships between sub-objects 
        [Nested(Name = "items")]
        public List<Item>? Items { get; set; }
    }
}
