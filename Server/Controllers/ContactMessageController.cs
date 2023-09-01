using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SycappsWeb.Server.Services;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class ContactMessageController : ControllerBase
    {
        private readonly ILogger<ContactMessageController> logger;
        private readonly IContactMessageService contactService;

        public ContactMessageController(ILogger<ContactMessageController> logger, IContactMessageService contactService)
        {
            this.logger = logger;
            this.contactService = contactService;
        }

        [HttpPost("post")]
        public async Task<ActionResult> Post([FromBody] ContactMessageRequest messageRequest)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values)
                {
                    logger.LogError(modelError.Errors[0].ErrorMessage, string.Empty);
                }
                return BadRequest(ModelState);
            }

            var messageToInsert = (MensajeContacto)messageRequest;
            try
            {
                await contactService.Add(messageToInsert);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, string.Empty);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedResponse<List<MensajeContacto>>>> List([FromQuery] PaginationFilter filter)
        {
            PagedResponse<List<MensajeContacto>> messages = null;
            try
            {
                var pagination = new PaginationFilter(filter.PageNumber, filter.PageSize);
                messages = await contactService.All(pagination);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);
                BadRequest(ex.Message);
            }

            return Ok(messages);
        }

        [HttpGet("unread")]
        public async Task<ActionResult<PagedResponse<List<MensajeContacto>>>> Unread([FromQuery] PaginationFilter filter)
        {
            var pagination = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var messages = await contactService.Unread(pagination);

            return messages;
        }
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await contactService.MarkAsRead(id);

            return Ok();
        }
    }
}
