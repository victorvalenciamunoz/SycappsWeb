using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SycappsWeb.Server.ExtensionMethods;
using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Server.Services;
using SycappsWeb.Shared.ExtensionMethods;
using SycappsWeb.Shared.Models.Un2Trek;
using System.Security.Claims;

namespace SycappsWeb.Server.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class Un2TrekTrekiController : ControllerBase
{
    private readonly ILogger<Un2TrekTrekiController> logger;
    private readonly ITrekiService trekiService;

    public Un2TrekTrekiController(ILogger<Un2TrekTrekiController> logger,
                                ITrekiService trekiService)
    {
        this.logger = logger;
        this.trekiService = trekiService;
    }

    //api/v1/Un2TrekTreki/around?latitud=40.2515007401391&longitud=-3.82886812090874
    [HttpGet("around")]
    public async Task<ActionResult<List<TrekiResponse>>> Get([FromQuery] double latitud, [FromQuery] double longitud)
    {
        List<TrekiResponse>? trekisAround = null;
        List<TrekiDto> trekis = null;
        try
        {
            trekis = await trekiService.GetTrekisAround(latitud, longitud);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Empty);
            return BadRequest(ex.Message);
        }

        if (trekis != null && trekis.Any())
        {
            trekisAround = new List<TrekiResponse>();
            foreach (var treki in trekis)
            {
                trekisAround.Add(treki.ToResponse());
            }
        }

        return Ok(trekisAround);
    }

    [HttpPost]
    public async Task<ActionResult<TrekiResponse>> Post([FromBody] TrekiRequest treki)
    {

        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                logger.LogError(modelError.Errors[0].ErrorMessage, string.Empty);
            }
            return BadRequest(ModelState);
        }
        try
        {
            var trekiToInsert = treki.ToDto();
            await trekiService.Add(trekiToInsert);
            var trekiResponse = trekiToInsert.ToResponse();
            return Ok(trekiResponse);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, string.Empty);
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] TrekiRequest treki)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                logger.LogError(modelError.Errors[0].ErrorMessage, string.Empty);
            }
            return BadRequest(ModelState);
        }
        var result = await trekiService.Modify(treki.ToDto());

        if (result)
            return Ok();
        else
            return BadRequest();
    }

    [HttpPost("capture")]
    public async Task<IActionResult> Capture(CaptureTrekiRequest captureTreki)
    {
        var idUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = await trekiService.Capture(captureTreki, idUsuario!);
        if (result.HasErrors)
        {
            logger.LogError(result.Errors[0], string.Empty);
            return BadRequest(result.Errors[0]);
        }

        return Ok(result.Element);
    }

}
