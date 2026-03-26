namespace WebFrontEnd.Services.Auth
{
    public class AuthResponse
    {
        public string token { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CardCode { get; set; }
        public string UniqueName { get; set; }
    }
}
