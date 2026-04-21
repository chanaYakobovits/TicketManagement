using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class DepartmentDTO
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<UserDTO> Users { get; set; } = new List<UserDTO>();
}
