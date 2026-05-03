using AutoMapper;
using Common.DTO;
using Common.Models;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
//using Microsoft.AspNetCore.Identity;


namespace Service.Services
{
    public class UserService : IUserService
   
    {
        private readonly BaseRepository<User> _BaseRepository;
        private readonly BaseRepository<UserType> _BaseRepositoryType;
        private readonly BaseRepository<Department> _BaseRepositoryDepartment;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;

        public UserService(BaseRepository<User> BaseRepository, BaseRepository<UserType> BaseRepositoryType,
            BaseRepository<Department> BaseRepositoryDepartment, IMapper mapper, IConfiguration config, JwtService jwtService)
        {
            _BaseRepository = BaseRepository;
            _BaseRepositoryType = BaseRepositoryType;
            _BaseRepositoryDepartment = BaseRepositoryDepartment;
            _mapper = mapper;
            _config = config;
            _jwtService = jwtService;
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
        //public async Task<UserDTO?> Authenticate(string employeeId, string password)
        //{
        //    try
        //    {
        //        var user = await _BaseRepository.Items.FirstOrDefaultAsync(u =>
        //            u.Email == employeeId || u.EmployeeId.ToString() == employeeId);

        //        if (user == null) return null;

        //        var hasher = new PasswordHasher<User>();
        //        var result = hasher.VerifyHashedPassword(user, user.Password, password);

        //        if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
        //        {
        //            var dto = _mapper.Map<UserDTO>(user);
        //            dto.Password = string.Empty;
        //            return dto;
        //        }

        //        return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        public async Task<(bool Success, string Message)> ForgotPassword(string email)
        {
            var user = await _BaseRepository.Items.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return (true, "אם המייל קיים במערכת, נשלח אליו קישור לאיפוס סיסמה");

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                .Replace("+", "-").Replace("/", "_").Replace("=", "");

            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            await _BaseRepository.Update(user);

            var clientUrl = _config["ClientUrl"];
            var resetLink = $"{clientUrl}/reset-password?token={token}";
            await SendResetEmailAsync(user.Email, resetLink);

            return (true, "אם המייל קיים במערכת, נשלח אליו קישור לאיפוס סיסמה");
        }

        public async Task<(bool Success, string Message)> ResetPassword(string token, string newPassword)
        {
            var user = await _BaseRepository.Items
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == token);

            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
                return (false, "הקישור אינו תקף או שפג תוקפו");

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, newPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _BaseRepository.Update(user);

            return (true, "הסיסמה עודכנה בהצלחה");
        }

        private async Task SendResetEmailAsync(string toEmail, string resetLink)
        {
            using var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(
                _config["Gmail:From"],
                _config["Gmail:AppPassword"]
            );
            smtp.EnableSsl = true;

            var mail = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_config["Gmail:From"]!, _config["Gmail:DisplayName"]),
                Subject = "איפוס סיסמה",
                IsBodyHtml = true,
                Body = $"""
            <div dir="rtl" style="font-family:Arial;padding:20px;max-width:500px;">
                <h2 style="color:#0067b8;">איפוס סיסמה</h2>
                <p>קיבלנו בקשה לאיפוס הסיסמה שלך.</p>
                <p>לחץ על הכפתור להגדרת סיסמה חדשה. הקישור תקף ל-15 דקות.</p>
                <a href="{resetLink}"
                   style="background:#0067b8;color:white;padding:12px 24px;
                          border-radius:8px;text-decoration:none;display:inline-block;
                          margin:20px 0;">
                    איפוס סיסמה
                </a>
                <p style="color:#888;font-size:12px;margin-top:20px;">
                    אם לא ביקשת איפוס סיסמה, התעלם מהודעה זו.
                </p>
            </div>
        """
            };
            mail.To.Add(toEmail);
            await smtp.SendMailAsync(mail);
        }

        public async Task<(bool Success, string Token, UserDTO? User)> Authenticate(string employeeId, string password)
        {
            try
            {
                var user = await _BaseRepository.Items.FirstOrDefaultAsync(u =>
                    u.Email == employeeId || u.EmployeeId.ToString() == employeeId);

                if (user == null)
                    return (false, "מספר עובד או סיסמה שגויים", null);

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.Password, password);

                if (result == PasswordVerificationResult.Failed)
                    return (false, "מספר עובד או סיסמה שגויים", null);

                var dto = _mapper.Map<UserDTO>(user);
                dto.Password = string.Empty;

                var token = _jwtService.GenerateToken(dto); // ← הוסף JwtService ל-constructor

                return (true, token, dto);
            }
            catch
            {
                return (false, "אירעה שגיאה פנימית", null);
            }
        }
    }
}
