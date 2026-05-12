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
    }
}
