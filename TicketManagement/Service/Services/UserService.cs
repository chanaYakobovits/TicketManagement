using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTO;
using Common.Models;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.IServices;
//using Microsoft.AspNetCore.Identity;


namespace Service.Services
{
    public class UserService : IUserService
   
    {
        private readonly BaseRepository<User> _BaseRepository;
        private readonly BaseRepository<UserType> _BaseRepositoryType;
        private readonly BaseRepository<Department> _BaseRepositoryDepartment;
        private readonly IMapper _mapper;
        public UserService(BaseRepository<User> BaseRepository, BaseRepository<UserType> BaseRepositoryType,
            BaseRepository<Department> BaseRepositoryDepartment, IMapper mapper)
        {
            _BaseRepository = BaseRepository;
            _BaseRepositoryType = BaseRepositoryType;
            _BaseRepositoryDepartment = BaseRepositoryDepartment;
            _mapper = mapper;
           
        }
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
           

            try
            {
                //var details = await _BaseRepository.GetAllUsers();

                var details = await _BaseRepositoryType.Items.ToListAsync();
                if (details != null)
                {
                    var result = _mapper.Map<IEnumerable<UserDTO>>(details);
                   
                    return result;
                }
                else
                {
                 
                    return Enumerable.Empty<UserDTO>();
                }
            }
            catch (Exception ex)
            {
                
                return Enumerable.Empty<UserDTO>();
            }
        }
        public async Task<IEnumerable<UserTypeDTO>> GetUserTypes()
        {


            try
            {
                var details = await _BaseRepositoryType.Items.ToListAsync();

                if (details != null)
                {
                    var result = _mapper.Map<IEnumerable<UserTypeDTO>>(details);

                    return result;
                }
                else
                {

                    return Enumerable.Empty<UserTypeDTO>();
                }
            }
            catch (Exception ex)
            {

                return Enumerable.Empty<UserTypeDTO>();
            }
        }
        public async Task<IEnumerable<DepartmentDTO>> GetDepartment()
        {


            try
            {
                var details = await _BaseRepositoryDepartment.Items.ToListAsync();

                if (details != null)
                {
                    var result = _mapper.Map<IEnumerable<DepartmentDTO>>(details);

                    return result;
                }
                else
                {

                    return Enumerable.Empty<DepartmentDTO>();
                }
            }
            catch (Exception ex)
            {

                return Enumerable.Empty<DepartmentDTO>();
            }
        }

        public async Task<(bool Success, string Message, UserDTO? Data)> Add(UserDTO userdto)
        {
            // בדיקה אם המייל כבר קיים
            var emailExists = await _BaseRepository.Items.AnyAsync(u => u.Email == userdto.Email);
            if (emailExists)
                return (false, "כתובת המייל כבר קיימת במערכת.", null);

            // בדיקה אם מספר העובד כבר קיים
            var employeeIdExists = await _BaseRepository.Items.AnyAsync(u => u.EmployeeId == userdto.EmployeeId);
            if (employeeIdExists)
                return (false, "מספר העובד כבר קיים במערכת.", null);

            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                FirstName = userdto.FirstName,
                LastName = userdto.LastName,
                Email = userdto.Email,
                Phone = userdto.Phone,
                Password = hasher.HashPassword(null, userdto.Password),
                EmployeeId = userdto.EmployeeId,
                UserTypeId = userdto.UserTypeId,
                DepartmentId = userdto.DepartmentId,
                JobTitle = userdto.JobTitle,
                Status = userdto.Status
            };

            var newUser = await _BaseRepository.Add(user);

            return (true, "משתמש נוסף בהצלחה.", _mapper.Map<UserDTO>(newUser.Entity));
        }
        public async Task<UserDTO?> Authenticate(string employeeId, string password)
        {
            try
            {
                var user = await _BaseRepository.Items.FirstOrDefaultAsync(u =>
                    u.Email == employeeId || u.EmployeeId.ToString() == employeeId);

                if (user == null) return null;

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.Password, password);

                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    var dto = _mapper.Map<UserDTO>(user);
                    dto.Password = string.Empty;
                    return dto;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
