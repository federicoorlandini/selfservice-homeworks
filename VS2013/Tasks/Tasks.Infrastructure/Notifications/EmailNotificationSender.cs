using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Infrastructure.Notifications
{
    public class EmailNotificationSender : INotificationSender
    {
        public void SendUpdateTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            throw new NotImplementedException();
        }

        public void SendDeleteTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            throw new NotImplementedException();
        }

        public void SendNewCommentInTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            throw new NotImplementedException();
        }

        public void SendNewLoggedWorkInTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            throw new NotImplementedException();
        }
    }
}
