namespace Banking.Identity.Models
{
    public class User
    {
        public int AccountNumber { get; set; }
        public string FullName { get; set; }
        public string Currency { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
