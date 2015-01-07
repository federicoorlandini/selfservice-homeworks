using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Infrastructure.Notifications
{
    /// <summary>
    /// This is our custom class that sends emails with the notification to the watcher users of a task
    /// </summary>
    public class EmailNotificationSender : INotificationSender
    {
        public void SendUpdateTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            // Here we must send emails wit a notification about the update of the task.
            // We must send a notification email to every watcher user of the updated task
        }

        public void SendDeleteTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            // Here we must send emails wit a notification about the delte of the task.
            // We must send a notification email to every watcher user of the deleted task
        }

        public void SendNewCommentInTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            // Here we must send emails wit a notification to inform about a new comment in the task
            // We must send a notification email to every watcher user of the task
        }

        public void SendNewLoggedWorkInTaskNotification(DomainModel.User recipient, DomainModel.Task task)
        {
            // Here we must send emails wit a notification to inform about a logged work in the task
            // We must send a notification email to every watcher user of the task
        }
    }
}
