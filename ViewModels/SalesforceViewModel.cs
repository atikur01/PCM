using System.ComponentModel.DataAnnotations;

namespace PCM.ViewModels
{
    public class SalesforceViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AccountName { get; set; }
    }
}
