using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class UserTypeDTO
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<UserDTO> Users { get; set; } = new List<UserDTO>();
}
