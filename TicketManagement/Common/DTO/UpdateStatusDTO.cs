using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class UpdateStatusDTO
    {
        public string NewStatus { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
