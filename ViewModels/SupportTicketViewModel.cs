using System.ComponentModel.DataAnnotations;

namespace PCM.ViewModels
{
    public class SupportTicketViewModel
    {
        [Required(ErrorMessage = "Summary is required.")]
        public string Summary { get; set; }

        [Display(Name = "Reported By")]
        public string Reported { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        public string? Collection { get; set; }

        [Display(Name = "Link")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; }
    }
}
