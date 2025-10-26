namespace Demo.Presentation.ViewModels.Identity
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirsrtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
