namespace CompanyPhonebook.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchQuery { get; set; }  // To keep the search query when navigating pages
    }
}

