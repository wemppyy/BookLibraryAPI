namespace BookLibraryAPI.Models
{
    public sealed class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class AuthResponse
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
