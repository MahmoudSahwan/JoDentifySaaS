using JoDentify.Application.DTOs.Gemini;
using JoDentify.Application.Interfaces;
using Microsoft.Extensions.Configuration; // (مهم)
using System.Net.Http;
using System.Net.Http.Json; // (مهم)
using System.Text.Json;
using System.Threading.Tasks;

namespace JoDentify.Application.Services
{
	public class GeminiChatService : IGeminiChatService
	{
		private readonly HttpClient _httpClient;
		private readonly string _geminiApiKey; // (هنجيبه من appsettings)
		private readonly string _geminiApiUrl; // (هنجيبه من appsettings)
		private readonly string _systemPrompt; // (جديد)

		public GeminiChatService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;

			// (مهم جداً) لازم تضيف السطور دي في appsettings.json
			_geminiApiKey = configuration["Gemini:ApiKey"] ?? "";
			_geminiApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-09-2025:generateContent?key={_geminiApiKey}";

			// (ده "النظام" اللي هيمشي عليه الـ AI)
			_systemPrompt = "You are 'Health GPT', a professional AI assistant for dentists. Your role is to provide concise, helpful information related to dentistry, clinic management, and patient queries. Do not answer non-medical or non-dental questions. Always be professional, supportive, and accurate. Keep responses relatively short.";
		}

		public async Task<GeminiChatResponseDto> GenerateContentAsync(GeminiChatRequestDto requestDto)
		{
			if (string.IsNullOrWhiteSpace(_geminiApiKey))
			{
				throw new InvalidOperationException("Gemini API key is not configured.");
			}

			// 1. تحضير "الباكيدج" اللي هيتبعت لـ Gemini
			var payload = new
			{
				contents = new[]
				{
					new { parts = new[] { new { text = requestDto.Prompt } } }
				},
				// (مهم) هنا بنعرف الـ AI هو مين
				systemInstruction = new
				{
					parts = new[] { new { text = _systemPrompt } }
				}
			};

			// 2. إرسال الطلب لـ Gemini
			var response = await _httpClient.PostAsJsonAsync(_geminiApiUrl, payload);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				// (ده لو حصل إيرور من جوجل)
				throw new HttpRequestException($"Error from Gemini API: {response.StatusCode}. Details: {errorContent}");
			}

			// 3. قراءة الرد من Gemini
			var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiApiResponse>();

			// (هنا بنفلتر الرد ونطلع النص بس)
			var responseText = geminiResponse?.candidates?[0]?.content?.parts?[0]?.text
								?? "Sorry, I couldn't process that request.";

			return new GeminiChatResponseDto { ResponseText = responseText };
		}
	}

	// (موديلات مساعدة عشان نفهم الرد بتاع جوجل)
	internal class GeminiApiResponse
	{
		public Candidate[]? candidates { get; set; }
	}
	internal class Candidate
	{
		public Content? content { get; set; }
	}
	internal class Content
	{
		public Part[]? parts { get; set; }
	}
	internal class Part
	{
		public string? text { get; set; }
	}
}
