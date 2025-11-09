using JoDentify.Application.DTOs.Gemini;
using JoDentify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoDentify.API.Controllers
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class GeminiChatController : ControllerBase
    {
        private readonly IGeminiChatService _geminiChatService;

        public GeminiChatController(IGeminiChatService geminiChatService)
        {
            _geminiChatService = geminiChatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] GeminiChatRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _geminiChatService.GenerateContentAsync(requestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // (ده لو الـ API Key غلط أو حصل مشكلة)
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}




