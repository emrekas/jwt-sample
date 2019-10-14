namespace JWTSample.Entities
{
    //Veritabanımız için ApplicationUser modelini oluşturduk.
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "unnamed";
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
