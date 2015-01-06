using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Infrastructure.Notifications
{
    public interface INotificationSender
    {
        void SendUpdateTaskNotification(DomainModel.User recipient, DomainModel.Task task);
        void SendDeleteTaskNotification(DomainModel.User recipient, DomainModel.Task task);
        void SendNewCommentInTaskNotification(DomainModel.User recipient, DomainModel.Task task);
        void SendNewLoggedWorkInTaskNotification(DomainModel.User recipient, DomainModel.Task task);
    }
}
