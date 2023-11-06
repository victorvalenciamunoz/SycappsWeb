using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
    private readonly ITrekiService trekiService;

    public Un2TrekTrekiController(ITrekiService trekiService)
    {
        this.trekiService = trekiService;
    }

    //api/v1/Un2TrekTreki/around?latitud=40.2515007401391&longitud=-3.82886812090874
    [HttpGet("around")]
    public async Task<ActionResult<List<TrekiResponse>>> Get([FromQuery] double latitud, [FromQuery] double longitud)
    {
        List<TrekiResponse>? trekisAround = null;
        List<TrekiDto>? trekis = null;
        try
        {
            trekis = await trekiService.GetTrekisAround(latitud, longitud);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("Error retriving trekis around latitude:{@latitud} longitude{@longitud}\n\r{@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
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

    [HttpPost("add")]
    public async Task<ActionResult<TrekiResponse>> Post([FromBody] TrekiRequest treki)
    {

        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                Log.Logger.Error("Error validating new treki {@treki} {@modelError.Errors}", treki, modelError.Errors);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        try
        {
            var trekiToInsert = treki.ToDto();
            trekiToInsert.Activo = true;

            await trekiService.Add(trekiToInsert);
            var trekiResponse = trekiToInsert.ToResponse();
            return Ok(trekiResponse);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("General exception adding treki {@treki}\n\r{@ex}", treki, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
        }

    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await trekiService.Delete(id);
            if (result.IsError)
            {
                Log.Logger.Error("Error deleting treki {@id} {@result.Errors}", id, result.Errors);
                return BadRequest(new ProblemDetails
                {
                    Detail = result.Errors.First().Description
                });
            }
            return Ok(Result.Deleted);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("General exception deleting treki {@id}\r\n{@ex}", id, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
        }

    }

    [HttpPut("{id}/update")]
    public async Task<IActionResult> Put([FromBody] TrekiRequest treki)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                Log.Logger.Error("Error validating treki on update {@treki} {@modelError.Errors}", treki, modelError.Errors);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var result = await trekiService.Modify(treki.ToDto());

            if (result.IsError)
            {
                Log.Logger.Error("Error updating treki {@treki} {@result.Errors}", treki, result.Errors);
                return BadRequest(new ProblemDetails
                {
                    Detail = result.Errors.First().Description
                });
            }

            return Ok(Result.Updated);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("General exception updating treki {@treki}\r\n{@ex}", treki, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
        }
    }

    [HttpPost("capture")]
    public async Task<IActionResult> Capture(CaptureTrekiRequest captureTreki)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values)
            {
                Log.Logger.Error("Error validating treki on capture {@captureTreki} {@modelError.Errors}", captureTreki, modelError.Errors);
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var idUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await trekiService.Capture(captureTreki, idUsuario!);
            if (result.IsError)
            {
                Log.Logger.Error("Error capturing treki {@captureTreki} {@result.Errors}", captureTreki, result.Errors);
                return BadRequest(new ProblemDetails
                {
                    Detail = result.Errors.First().Description
                });
            }

            return Ok(Result.Success);
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("General exception capturing treki {@captureTreki}\r\n{@ex}", captureTreki, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Detail = ex.Message,
            });
        }
    }
}
