using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface ITicketService
    {
        Task<(bool Success, string Message, TicketDTO? Data)> Add(TicketDTO dto);
        Task<IEnumerable<TicketDTO>> GetByUserId(int userId);
        //Task<TicketDTO?> GetById(int id);
        Task<TicketDetailDTO?> GetById(int id);
        Task<IEnumerable<TicketDetailDTO>> GetByAssignedUser(int userId);
        Task<(bool Success, string Message)> UpdateStatus(int ticketId, string newStatus, int userId, string comment);
        Task<(bool Success, string Message)> AddMessage(int ticketId, int userId, string content);
    }
}
