using Microsoft.AspNetCore.Mvc;
using SycappsWeb.Server.ExtensionMethods;
using SycappsWeb.Server.Services;
using SycappsWeb.Shared.ExtensionMethods;
using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Server.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize]
public class Un2TrekActividadController : ControllerBase
{
    private readonly IActividadService actividadService;
    private readonly ITrekiService trekiService;

    public Un2TrekActividadController(IActividadService actividadService, ITrekiService trekiService)
    {
        this.actividadService = actividadService;
        this.trekiService = trekiService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActividadResponse>> Get(int id)
    {
        var actividad = await actividadService.Get(id);
        if (actividad != null)
        {
            return Ok(actividad);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ActividadResponse>> Post([FromBody] ActividadRequest actividad)
    {
        var actividadInsert = actividad.ToEntity();
        return Ok(await actividadService.Add(actividadInsert));
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ActividadResponse>>> GetActive(DateTime currentDate)
    {
        return Ok(await actividadService.GetActiveActivityList(currentDate));
    }

    [HttpGet("all")]
    public async Task<ActionResult<PagedResponse<IEnumerable<ActividadResponse>>>> GetAll([FromQuery] PaginationFilter filter)
    {
        var activityList = await actividadService.All(filter);
        return Ok(activityList);
    }

    [HttpPut("{id}/update")]
    public async Task<IActionResult> Update(int id, [FromBody] ActividadRequest actividad)
    {
        try
        {
            await actividadService.Update(id, actividad);
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id}/trekilist")]
    public async Task<ActionResult<List<TrekiResponse>>> GetTrekiListByActivity(int id)
    {
        List<TrekiResponse> response = new List<TrekiResponse>();
        var trekiList = await trekiService.GetTrekiListByActivity(id);
        if (trekiList != null)
        {
            foreach (var treki in trekiList)
            {
                response.Add(treki.ToResponse());
            }
        }

        return Ok(response);
    }

    [HttpGet("{id}/unassignedTrekiList")]
    public async Task<ActionResult<List<TrekiResponse>>> GetTrekiListNotInActivity(int id)
    {
        List<TrekiResponse> response = new List<TrekiResponse>();
        var trekiList = await trekiService.GetTrekiListNotInActivity(id);
        if (trekiList != null)
        {
            foreach (var treki in trekiList)
            {
                response.Add(treki.ToResponse());
            }
        }

        return Ok(response);
    }

    [HttpPost("{id}/treki")]
    public async Task<IActionResult> AssignTrekiToActivity(int id, [FromBody] int trekiId)
    {
        try
        {
            await actividadService.AssignTrekiToActivity(id, trekiId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/treki/{trekiId}")]
    public async Task<IActionResult> RemoveTrekiFromActivity(int id, int trekiId)
    {
        try
        {
            await actividadService.RemoveTrekiFromActivity(id, trekiId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
