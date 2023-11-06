using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly IContactMessageService contactService;

        public ContactMessageController(IContactMessageService contactService)
        {
            this.contactService = contactService;
        }

        [HttpPost("post")]
        public async Task<ActionResult> Post([FromBody] ContactMessageRequest messageRequest)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values)
                {
                    Log.Logger.Error("Error validating contact message {@modelError.Errors}", modelError.Errors);
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
                Log.Logger.Fatal("General exception creating contact message {@messageRequest}\r\n{@ex}", messageRequest, ex);
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
                Log.Logger.Fatal("General exception retreiving contact message list {@filter}\r\n{@ex}", filter, ex);
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
