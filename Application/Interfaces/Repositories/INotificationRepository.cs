using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Notification Create(Notification notification);
        Notification GetNotify(Guid id);
        ICollection<Notification> GetAll();
        ICollection<Notification>GetNoticeByManagerId(Guid managerId);
    }
}
