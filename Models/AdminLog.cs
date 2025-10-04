namespace CompanyPhonebook.Models
{
    public class AdminLog
    {
        public int Id { get; set; }
        public string Adminusername { get; set; } = string.Empty;
        public string ControllerName { get; set; } = string.Empty;

        public string ActionName { get; set; } = string.Empty;
        public string IPAdress { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
