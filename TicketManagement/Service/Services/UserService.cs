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
        public async Task<UserDTO> Add(UserDTO userdto)
        {
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                FirstName = userdto.FirstName,
                LastName = userdto.LastName,
                Email = userdto.Email,
                Phone = userdto.Phone,
                Password = hasher.HashPassword(null, userdto.Password),
                UserTypeId = userdto.UserTypeId,
                DepartmentId = userdto.DepartmentId,
                JobTitle = userdto.JobTitle,
                Status = userdto.Status
            };

            var newUser = await _BaseRepository.Add(user);

            return _mapper.Map<UserDTO>(newUser.Entity);
        }

    }
}
