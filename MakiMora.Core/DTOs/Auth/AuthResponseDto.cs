namespace MakiMora.Core.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
        public UserDto User { get; set; } = null!;
    }
}