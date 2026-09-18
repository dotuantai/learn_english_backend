using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using learn_english_backend.Models.DTOs;
using learn_english_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace learn_english_backend.Controllers;

[ApiController]
[Route("api/learning")]
public sealed class LearningController(ILearningService learningService)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType<LearningContentResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LearningContentResponse>> GetContent(
        CancellationToken cancellationToken) =>
        Ok(await learningService.GetContentAsync(cancellationToken));

    [Authorize]
    [HttpGet("progress")]
    [ProducesResponseType<LearningProgressResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LearningProgressResponse>> GetProgress(
        CancellationToken cancellationToken) =>
        Ok(await learningService.GetProgressAsync(
            GetUserId(), cancellationToken));

    [Authorize]
    [HttpPut("progress/{wordId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetMastered(
        int wordId,
        UpdateWordProgressRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await learningService.SetMasteredAsync(
            GetUserId(), wordId, request.Mastered, cancellationToken);

        if (!updated)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Vocabulary word not found."
            });
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("progress/import")]
    [ProducesResponseType<LearningProgressResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LearningProgressResponse>> ImportProgress(
        ImportLearningProgressRequest request,
        CancellationToken cancellationToken) =>
        Ok(await learningService.ImportProgressAsync(
            GetUserId(), request.MasteredWordIds, cancellationToken));

    private string GetUserId() =>
        User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException(
            "The authenticated user has no identifier claim.");
}
