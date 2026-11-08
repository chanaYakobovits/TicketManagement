using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "יש להזין אימייל או מספר עובד")]
        public string EmployeeIdOrEmail { get; set; } = null!;

        [Required(ErrorMessage = "יש להזין סיסמה")]
        public string Password { get; set; } = null!;
    }
}
