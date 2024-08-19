using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _noticeRepo;
        private readonly ICurrentUser _currentUser;
        public NotificationService(INotificationRepository noticeRepo, ICurrentUser currentUser)
        {
            _noticeRepo = noticeRepo;
            _currentUser = currentUser;
        }

        public Response<NotificationReponseModel> Create(NotificationRequestModel model)
        {
            var notice = new Notification
            {
                Message = model.Message,
                Receiver = model.Receiver,
                Sender = model.Sender,
                CreatedBy = _currentUser.GetCurrentUser(),
            };
            _noticeRepo.Create(notice);
            return new Response<NotificationReponseModel>
            {
                Message = "notication created successfully",
                Status = true,
                Value = new NotificationReponseModel
                {
                    Message = notice.Message,
                    Receiver = notice.Receiver,
                    Sender = notice.Sender,
                    CreatedBy = notice.CreatedBy,
                    DateCreated = notice.DateCreated,
                }
            };
        }

        public Response<ICollection<NotificationReponseModel>> GetAll()
        {
            var notice = _noticeRepo.GetAll();
            var listOfnotice = notice.Select(n => new NotificationReponseModel
            {
                Message = n.Message,
                Receiver = n.Receiver,
                Sender = n.Sender,
                CreatedBy = n.CreatedBy,
                DateCreated = n.DateCreated,

            }).ToList();
            return new Response<ICollection<NotificationReponseModel>>
            {
                Status = true,
                Value = listOfnotice
            };
        }

        public Response<ICollection<NotificationReponseModel>> GetNoticeByManagerId(Guid managerId)
        {
            var mangerNotice = _noticeRepo.GetNoticeByManagerId(managerId);
            var listOfManagerNotice = mangerNotice.Select(f => new NotificationReponseModel
            {
                Message = f.Message,
                Receiver = f.Receiver,
                Sender = f.Sender,
                CreatedBy = f.CreatedBy,
                DateCreated = f.DateCreated,
            }).ToList();
            return new Response<ICollection<NotificationReponseModel>>
            {
                Status = true,
                Value = listOfManagerNotice
            };
        }

        public Response<NotificationReponseModel> GetNotify(Guid id)
        {
            var notice = _noticeRepo.GetNotify(id);
            if (notice == null)
            {
                return null;
            }
            return new Response<NotificationReponseModel>
            {
                Status = true,
                Value = new NotificationReponseModel
                {
                    Message = notice.Message,
                    Receiver = notice.Receiver,
                    Sender = notice.Sender,
                    CreatedBy = notice.CreatedBy,
                    DateCreated = notice.DateCreated,
                }
            };
        }
    }
}
