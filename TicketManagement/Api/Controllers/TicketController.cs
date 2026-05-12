using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace Api.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class TicketController : ControllerBase
        {
            private readonly ITicketService _ticketService;

            public TicketController(ITicketService ticketService)
            {
                _ticketService = ticketService;
            }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var tickets = await _ticketService.GetByUserId(userId);
            return Ok(tickets);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetById(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }
        [HttpPost]
            public async Task<IActionResult> AddTicket([FromBody] TicketDTO dto)
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "הנתונים שהתקבלו אינם תקינים." });

                try
                {
                    var (success, message, data) = await _ticketService.Add(dto);
                    if (!success)
                        return StatusCode(500, new { success = false, message });

                    return Ok(new { success = true, message, data });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = ex.InnerException?.Message ?? ex.Message });
                }
            }
        }
    }

