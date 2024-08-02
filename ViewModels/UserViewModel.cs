namespace PCM.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; }
    }
}
