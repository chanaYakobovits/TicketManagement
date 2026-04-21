using Common.DTO;
using Data;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _UserService.GetAll();
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("UserType")]
        public async Task<IActionResult> GetUserTypes()
        {
            var type = await _UserService.GetUserTypes();
            return type != null ? Ok(type) : NotFound();
        }
        [HttpGet("Department")]
        public async Task<IActionResult> GetDepartment()
        {
            var deper = await _UserService.GetDepartment();
            return deper != null ? Ok(deper) : NotFound();
        }
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "הנתונים שהתקבלו אינם תקינים."
                });
            }

            try
            {
                var user = await _UserService.Add(dto);

                return Ok(new
                {
                    success = true,
                    message = "משתמש נוסף בהצלחה.",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

    }
}
