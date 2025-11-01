namespace JoDentify.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
    }
}