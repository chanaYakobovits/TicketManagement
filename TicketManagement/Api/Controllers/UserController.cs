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
        public async Task<ActionResult> AddUser([FromBody] UserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "הנתונים שהתקבלו אינם תקינים." });

            try
            {
                var (success, message, data) = await _UserService.Add(dto);

                if (!success)
                    return Conflict(new { success = false, message });

                return Ok(new { success = true, message, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult> login([FromBody] LoginDTO dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "הנתונים שהתקבלו אינם תקינים." });

            try
            {
                var (success, token, user) = await _UserService.Authenticate(
                    dto.EmployeeId, dto.Password);

                if (!success)
                    return Unauthorized(new { success = false, message = token });

                return Ok(new { success = true, token = token, data = user });
                //                              ↑ הטוקן יישלח לפרונט
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email))
                return BadRequest(new { success = false, message = "נא להזין כתובת מייל" });

            var (success, message) = await _UserService.ForgotPassword(dto.Email);
            return Ok(new { success, message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "נתונים חסרים" });

            var (success, message) = await _UserService.ResetPassword(dto.Token, dto.NewPassword);

            return success
                ? Ok(new { success = true, message })
                : BadRequest(new { success = false, message });
        }
    }
}
