using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Server.Services;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class PuntosdeInteresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IPoiService poiService;

        public PuntosdeInteresController(ApplicationDbContext context, IPoiService poiService)
        {
            this.context = context;
            this.poiService = poiService;
        }

        [HttpGet("prueba")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await context.PuntosdeInteres.Take(10).OrderBy(c => c.Id).ToListAsync());
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PuntoDto>>> Get([FromQuery] double posicionLatitud, [FromQuery] double posicionLongitud)
        {
            try
            {
                return await poiService.Get(posicionLatitud, posicionLongitud);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("import")]
        public async Task<ActionResult> Import(List<PuntodeInteres> puntos)
        {
            try
            {
                await poiService.Import(puntos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
