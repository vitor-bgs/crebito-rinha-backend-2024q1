namespace Crebito.Common.ErrorNotifications
{
    public class ErrorNotificationService
    {
        public bool HasError { get; private set; }
        public NotificationErrorType? ErrorType { get; private set; }
        public ICollection<string> Errors { get; private set; }

        public ErrorNotificationService()
        {
            Errors = new List<string>();
        }

        public void AddError(NotificationErrorType type, string message)
        {
            HasError = true;
            ErrorType = type;
            Errors.Add(message);
        }
    }
}
