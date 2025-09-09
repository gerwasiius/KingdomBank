using Identity.API.Interfaces;
using Identity.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
public sealed class JwksController : ControllerBase
{
    private readonly IKeyRotationRepository _repo;
    private readonly IKeyVault _vault;

    public JwksController(IKeyRotationRepository repo, IKeyVault vault)
    {
        _repo = repo; _vault = vault;
    }

    [HttpGet]
    [Route("/.well-known/jwks.json")]
    public async Task<IActionResult> Get()
    {
        var active = await _repo.GetAllByStatusAsync("active");
        var grace = await _repo.GetAllByStatusAsync("grace");
        var keys = new List<Jwk>();

        foreach (var d in active.Concat(grace))
        {
            var jwk = await _vault.GetPublicJwkAsync(d);
            keys.Add(jwk);
        }

        return Ok(new JwkSet(keys.ToArray()));
    }
}
