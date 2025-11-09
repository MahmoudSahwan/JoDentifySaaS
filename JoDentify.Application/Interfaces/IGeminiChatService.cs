using JoDentify.Application.DTOs.Gemini;

namespace JoDentify.Application.Interfaces
{
	public interface IGeminiChatService
	{
		Task<GeminiChatResponseDto> GenerateContentAsync(GeminiChatRequestDto requestDto);
	}
}
