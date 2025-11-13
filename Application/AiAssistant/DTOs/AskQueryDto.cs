using System.ComponentModel.DataAnnotations;

namespace Application.AiAssistant.DTOs;

public record AskQueryDto([Required] string Question);
