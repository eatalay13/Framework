﻿namespace Core.Infrastructure.NotificationService
{
    public struct NotifyData
    {
        public NotifyType Type { get; set; }
        public string Message { get; set; }
        public bool Encode { get; set; }
    }
}