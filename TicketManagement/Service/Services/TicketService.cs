using AutoMapper;
using Common.DTO;
using Common.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TicketService : ITicketService
    {
        private readonly BaseRepository<Ticket> _ticketRepository;
        private readonly BaseRepository<Aianalysis> _aiRepository;
        private readonly IAIService _aiService;
        private readonly IMapper _mapper;
        private readonly BaseRepository<TicketStatusHistory> _statusHistoryRepository;
        private readonly BaseRepository<Message> _messageRepository;


        public TicketService(
    BaseRepository<Ticket> ticketRepository,
    BaseRepository<Aianalysis> aiRepository,
    BaseRepository<TicketStatusHistory> statusHistoryRepository,
    BaseRepository<Message> messageRepository,
    IAIService aiService,
    IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _aiRepository = aiRepository;
            _statusHistoryRepository = statusHistoryRepository;
            _messageRepository = messageRepository;
            _aiService = aiService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDTO>> GetByUserId(int userId)
        {
            try
            {
                var tickets = await _ticketRepository.Items
                    .Where(t => t.OpenedByUserId == userId)
                    .Include(t => t.Category)
                    .Include(t => t.TicketAssignments)
                        .ThenInclude(ta => ta.User)
                    .OrderByDescending(t => t.OpenedDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
            }
            catch
            {
                return Enumerable.Empty<TicketDTO>();
            }
        }

    
        public async Task<TicketDetailDTO?> GetById(int id)
        {
            var ticket = await _ticketRepository.Items
                .Include(t => t.Category)
                .Include(t => t.OpenedByUser)
                .Include(t => t.Attachments)
                .Include(t => t.Messages).ThenInclude(m => m.User)
                .Include(t => t.TicketStatusHistories).ThenInclude(h => h.ChangedByUser)
                .Include(t => t.TicketAssignments).ThenInclude(ta => ta.User)
               .Include(t => t.Aianalyses)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            return ticket == null ? null : _mapper.Map<TicketDetailDTO>(ticket);
        }
        public async Task<(bool Success, string Message, TicketDTO? Data)> Add(TicketDTO dto)
        {
            try
            {
                // 1. שמירת הטיקט עם ערכי ברירת מחדל
                var ticket = new Ticket
                {
                    Subject = dto.Subject,
                    Description = dto.Description,
                    CategoryId = 1,
                    UrgencyLevel = 1,
                    CurrentStatus = "פתוח",
                    OpenedByUserId = dto.OpenedByUserId,
                    OpenedDate = DateOnly.FromDateTime(DateTime.Now),
                    OpenedTime = TimeOnly.FromDateTime(DateTime.Now)
                };

                var result = await _ticketRepository.Add(ticket);
                var savedTicket = result.Entity;

                // 2. קריאה ל-AI
                var aiResult = await _aiService.AnalyzeTicket(dto.Subject ?? "", dto.Description);

                // 3. עדכון הטיקט עם תוצאות ה-AI
                savedTicket.CategoryId = aiResult.PredictedCategoryId;
                savedTicket.UrgencyLevel = (int)aiResult.UrgencyScore;
                await _ticketRepository.Update(savedTicket);

                // 4. שמירה בטבלת AIAnalysis
                var analysis = new Aianalysis
                {
                    TicketId = savedTicket.Id,
                    PredictedCategoryId = aiResult.PredictedCategoryId,
                    UrgencyScore = aiResult.UrgencyScore,
                    RiskLevel = aiResult.RiskLevel,
                    AnalysisText = aiResult.AnalysisText,
                    AnalyzedAt = DateTime.Now
                };
                await _aiRepository.Add(analysis);

                return (true, "הפנייה נפתחה בהצלחה.", _mapper.Map<TicketDTO>(savedTicket));
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message, null);
            }
        }
        public async Task<IEnumerable<TicketDetailDTO>> GetByAssignedUser(int userId)
        {
            var tickets = await _ticketRepository.Items
                .Where(t => t.TicketAssignments.Any(a => a.UserId == userId))
                .Include(t => t.Category)
                .Include(t => t.OpenedByUser)
                .Include(t => t.Aianalyses)
                .Include(t => t.TicketAssignments).ThenInclude(a => a.User)
                .OrderByDescending(t => t.UrgencyLevel)
                .ThenByDescending(t => t.OpenedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TicketDetailDTO>>(tickets);
        }
        public async Task<(bool Success, string Message)> UpdateStatus(int ticketId, string newStatus, int userId, string comment)
        {
            try
            {
                var ticket = await _ticketRepository.Items.FirstOrDefaultAsync(t => t.Id == ticketId);
                if (ticket == null) return (false, "פנייה לא נמצאה");

                var history = new TicketStatusHistory
                {
                    TicketId = ticketId,
                    PreviousStatus = ticket.CurrentStatus,
                    NewStatus = newStatus,
                    ChangedByUserId = userId,
                    Comment = comment,
                    ChangedAt = DateTime.Now
                };

                ticket.CurrentStatus = newStatus;
                await _ticketRepository.Update(ticket);
                await _statusHistoryRepository.Add(history);

                return (true, "סטטוס עודכן בהצלחה");
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> AddMessage(int ticketId, int userId, string content)
        {
            try
            {
                var message = new Message
                {
                    TicketId = ticketId,
                    UserId = userId,
                    Content = content,
                    SentAt = DateTime.Now
                };

                await _messageRepository.Add(message);
                return (true, "תגובה נשלחה בהצלחה");
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
