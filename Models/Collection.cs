namespace PCM.Models
{
    public class Collection
    {
        public Guid CollectionId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; } 

        public int? TotalItems { get; set; } 

        // Custom Field Names
        public string? CustomString1Name { get; set; }
        public string? CustomString2Name { get; set; }
        public string? CustomString3Name { get; set; }

        public string? CustomInt1Name { get; set; }
        public string? CustomInt2Name { get; set; }
        public string? CustomInt3Name { get; set; }

        public string? CustomMultilineText1Name { get; set; }
        public string? CustomMultilineText2Name { get; set; }
        public string? CustomMultilineText3Name { get; set; }

        public string? CustomBoolean1Name { get; set; }
        public string? CustomBoolean2Name { get; set; }
        public string? CustomBoolean3Name { get; set; }

        public string? CustomDate1Name { get; set; }
        public string? CustomDate2Name { get; set; }
        public string? CustomDate3Name { get; set; }

        public List<Item>? Items { get; set; }


    }
}
