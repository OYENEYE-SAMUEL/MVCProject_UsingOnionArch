using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface INotificationService
    {
        Response<NotificationReponseModel> Create(NotificationRequestModel model);
        Response <NotificationReponseModel> GetNotify(Guid id);
        Response <ICollection<NotificationReponseModel>> GetAll();
        Response <ICollection<NotificationReponseModel>> GetNoticeByManagerId(Guid managerId);
    }
}
