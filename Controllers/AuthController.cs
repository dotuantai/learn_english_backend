using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using learn_english_backend.Models.DTOs;
using learn_english_backend.Models.Results;
using learn_english_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace learn_english_backend.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Refresh(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);
        return ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke(
        RevokeTokenRequest request,
        CancellationToken cancellationToken)
    {
        await authService.RevokeAsync(request, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<CurrentUserResponse> Me()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? string.Empty;

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToArray();

        return Ok(new CurrentUserResponse(
            userId,
            User.FindFirstValue(JwtRegisteredClaimNames.Email),
            roles));
    }

    private ActionResult<AuthResponse> ToActionResult(AuthResult result)
    {
        if (result.Succeeded)
        {
            return Ok(result.Response);
        }

        if (result.FailureKind == AuthFailureKind.Validation &&
            result.ValidationErrors is not null)
        {
            return BadRequest(new ValidationProblemDetails(result.ValidationErrors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = result.ErrorMessage
            });
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = result.ErrorMessage
        };
        problemDetails.Extensions["code"] = result.ErrorCode;

        return Unauthorized(problemDetails);
    }
}
