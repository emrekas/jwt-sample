namespace JWTSample.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "unnamed";
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
