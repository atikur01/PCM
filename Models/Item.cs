
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PCM.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string> Tags   { get; set; }

        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public string CustomString1 { get; set; }
        public string? CustomString2 { get; set; }
        public string? CustomString3 { get; set; }

        public int? CustomInt1 { get; set; }
        public int? CustomInt2 { get; set; }
        public int? CustomInt3 { get; set; }

        public string? CustomMultilineText1 { get; set; }
        public string? CustomMultilineText2 { get; set; }
        public string? CustomMultilineText3 { get; set; }

        public bool? CustomBoolean1 { get; set; }
        public bool? CustomBoolean2 { get; set; }
        public bool? CustomBoolean3 { get; set; }

        public DateTime? CustomDate1 { get; set; }
        public DateTime? CustomDate2 { get; set; }
        public DateTime? CustomDate3 { get; set; }
    }
}
