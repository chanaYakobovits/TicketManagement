using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Common.Models;

namespace Service.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAll();
        Task<IEnumerable<UserTypeDTO>> GetUserTypes();
        Task<IEnumerable<DepartmentDTO>> GetDepartment();
        Task<(bool Success, string Message, UserDTO? Data)> Add(UserDTO userdto);
        Task<UserDTO?> Authenticate(string employeeId, string password);
    }
}
