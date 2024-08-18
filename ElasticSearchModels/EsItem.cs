using Nest;
using PCM.Models;
using System.Collections.ObjectModel;

namespace PCM.ElasticSearchModels
{
    [ElasticsearchType(RelationName = "item")]
    public class EsItem
    {
        [Keyword(Name = "item_id")]

        public Guid ItemId { get; set; }

        [Keyword(Name = "collection_id")]
        public Guid CollectionId { get; set; }

        [Text(Name = "author")]
        public string? Author { get; set; }

        [Text(Name = "collection_name")]
        public string? CollectionName { get; set; }

        [Text(Name = "name")]
        public string? Name { get; set; }

        [Ignore]
        public Collection? Collection { get; set; }

        // Custom Field Names
        [Text(Name = "custom_string_1_value")]
        public string? CustomString1Value { get; set; }

        [Text(Name = "custom_string_2_value")]
        public string? CustomString2Value { get; set; }

        [Text(Name = "custom_string_3_value")]
        public string? CustomString3Value { get; set; }

        [Number(NumberType.Integer, Name = "custom_int_1_value")]
        public int? CustomInt1Value { get; set; }

        [Number(NumberType.Integer, Name = "custom_int_2_value")]
        public int? CustomInt2Value { get; set; }

        [Number(NumberType.Integer, Name = "custom_int_3_value")]
        public int? CustomInt3Value { get; set; }

        [Text(Name = "custom_multiline_text_1_value")]
        public string? CustomMultilineText1Value { get; set; }

        [Text(Name = "custom_multiline_text_2_value")]
        public string? CustomMultilineText2Value { get; set; }

        [Text(Name = "custom_multiline_text_3_value")]
        public string? CustomMultilineText3Value { get; set; }

        [Boolean(Name = "custom_boolean_1_value")]
        public bool? CustomBoolean1Value { get; set; }

        [Boolean(Name = "custom_boolean_2_value")]
        public bool? CustomBoolean2Value { get; set; }

        [Boolean(Name = "custom_boolean_3_value")]
        public bool? CustomBoolean3Value { get; set; }

        [Date(Name = "custom_date_1_value")]
        public DateTime? CustomDate1Value { get; set; }

        [Date(Name = "custom_date_2_value")]
        public DateTime? CustomDate2Value { get; set; }

        [Date(Name = "custom_date_3_value")]
        public DateTime? CustomDate3Value { get; set; }

        [Date(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [Ignore]
        public List<Item>? Items { get; set; }

        [Text(Name = "tags")]
        public List<string>? Tags { get; set; }

        [Ignore]
        public virtual ICollection<Models.Like>? Likes { get; set; }
    }
}
