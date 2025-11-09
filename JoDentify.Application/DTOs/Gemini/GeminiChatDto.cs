using System.ComponentModel.DataAnnotations;

namespace JoDentify.Application.DTOs.Gemini
{
    
    public class GeminiChatRequestDto
    {
        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Prompt { get; set; } = string.Empty;
    }

    public class GeminiChatResponseDto
    {
        public string ResponseText { get; set; } = string.Empty;
    }
}
