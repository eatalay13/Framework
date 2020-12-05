using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Infrastructure.NotificationService
{
    public class NotificationService : INotificationService
    {
        #region Fields

        private readonly string _notificationListKey = "temp-notify-list";

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly ILogger<NotificationService> _logger;

        #endregion

        #region Ctor

        public NotificationService(IHttpContextAccessor httpContextAccessor,
            ITempDataDictionaryFactory tempDataDictionaryFactory, ILogger<NotificationService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _logger = logger;
        }

        #endregion

        #region Utilities

        protected virtual void PrepareTempData(NotifyType type, string message, bool encode = true)
        {
            var notifies = GetNotifies();

            notifies.Add(new NotifyData
            {
                Message = message,
                Type = type,
                Encode = encode
            });

            TempData[_notificationListKey] = JsonConvert.SerializeObject(notifies);

            _logger.LogWarning(string.Concat(_httpContextAccessor.HttpContext.User.Identity.Name, ": ", message));
        }

        #endregion


        #region Properties

        private ITempDataDictionary TempData => _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);

        #endregion

        #region Methods

        public virtual void Notification(NotifyType type, string message, bool encode = true)
        {
            PrepareTempData(type, message, encode);
        }


        public virtual void SuccessNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Success, message, encode);
        }


        public virtual void WarningNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Warning, message, encode);
        }


        public virtual void ErrorNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Error, message, encode);
        }


        public virtual void ErrorNotification(Exception exception)
        {
            if (exception == null)
                return;

            ErrorNotification(exception.Message);
        }

        public virtual IList<NotifyData> GetNotifies()
        {
            return TempData.ContainsKey(_notificationListKey)
                ? JsonConvert.DeserializeObject<IList<NotifyData>>(TempData[_notificationListKey].ToString() ?? string.Empty)
                : new List<NotifyData>();
        }

        #endregion
    }
}