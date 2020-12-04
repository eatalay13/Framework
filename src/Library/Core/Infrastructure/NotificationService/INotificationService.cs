using System;
using System.Collections.Generic;

namespace Core.Infrastructure.NotificationService
{
    public interface INotificationService
    {
        void ErrorNotification(Exception exception);
        void ErrorNotification(string message, bool encode = true);
        void Notification(NotifyType type, string message, bool encode = true);
        void SuccessNotification(string message, bool encode = true);
        void WarningNotification(string message, bool encode = true);
        IList<NotifyData> GetNotifies();
    }
}