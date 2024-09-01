using System.ComponentModel.DataAnnotations;

namespace PCM.Models
{
    public class Tickets
    {
        [Key]
        public string TicketId { get; set; }
        public string? Summary { get; set; }
        public string? Priority { get; set; }
        public string? Reported { get; set; }
        public string? Collection { get; set; }
        
        public string? Link { get; set; }
        
        public string? CreatedAt { get; set; }   

        public string? Status { get; set; }  
    }
}
