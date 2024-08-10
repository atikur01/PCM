
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PCM.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }

        public Guid CollectionId { get; set; }

        public string? Author { get; set; }

        public string? CollectionName { get; set; }

        public string? Name { get; set; }
    
        public Collection? Collection { get; set; }


        // Custom Field Names
        public string? CustomString1Value { get; set; }
        public string? CustomString2Value { get; set; }
        public string? CustomString3Value { get; set; }

        public string? CustomInt1Value { get; set; }
        public string? CustomInt2Value { get; set; }
        public string? CustomInt3Value { get; set; }

        public string? CustomMultilineText1Value { get; set; }
        public string? CustomMultilineText2Value { get; set; }
        public string? CustomMultilineText3Value { get; set; }

        public string? CustomBoolean1Value { get; set; }
        public string? CustomBoolean2Value { get; set; }
        public string? CustomBoolean3Value { get; set; }

        public string? CustomDate1Value { get; set; }
        public string? CustomDate2Value { get; set; }
        public string? CustomDate3Value { get; set; }

        public DateTime CreatedAt { get; set; } 


        [NotMapped]
        public List<Item> Items { get; set; }

   
        [NotMapped]
        public List<string> tags { get; set; }
        
    }
}
