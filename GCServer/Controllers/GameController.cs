using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GCServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly GCServerSettings _settings;

    public GameController(ILogger<GameController> logger, IOptions<GCServerSettings> gcServerSettings)
    {
        _logger = logger;
        _settings = gcServerSettings.Value;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Post()
    {
        try
        {
            string auth = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(auth)) return BadRequest(auth);
            auth = auth.Replace("Bearer ", null);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(auth);
            string email = (string)jsonToken.Payload["email"];

            if (!_settings.AllowedEmails?.Contains(email) ?? true)
            {
                _logger.LogWarning("{0} tried POST but is not allowed! Allowed emails are {1}", email, _settings.AllowedEmails);
                return Unauthorized(email);
            }

            _logger.LogInformation("Endpoint accessed by {0}", email);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }
    }
}
