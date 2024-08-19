using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FishContext _fishContext;
        public NotificationRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public Notification Create(Notification notification)
        {
            var notice = _fishContext.Notifications.Add(notification);
            return notification;
        }

        public ICollection<Notification> GetAll()
        {
            var notices = _fishContext.Notifications.Where(f => f.IsDeleted.Equals(false)).ToList();
            return notices;
        }

        public ICollection<Notification> GetNoticeByManagerId(Guid managerId)
        {
            var notice = _fishContext.Notifications
                .Where(n => n.StaffId == managerId && n.IsDeleted == false).ToList();
            return notice;
        }

        public Notification GetNotify(Guid id)
        {
            var notice = _fishContext.Notifications
                .FirstOrDefault(f => f.Id == id && f.IsDeleted == false);
            return notice;
        }

    }
}
