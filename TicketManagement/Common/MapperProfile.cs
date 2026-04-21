using AutoMapper;
using Common.DTO;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // הגדרת המרות
            CreateMap<Aianalysis, AianalysisDTO>().ReverseMap();
            CreateMap<Attachment, AttachmentDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Sla, SlaDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<TicketAssignment, TicketAssignmentDTO>().ReverseMap();
            CreateMap<TicketClosure, TicketClosureDTO>().ReverseMap();
            CreateMap<TicketStatusHistory, TicketStatusHistoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserType, UserTypeDTO>().ReverseMap();



        }
    }
}
