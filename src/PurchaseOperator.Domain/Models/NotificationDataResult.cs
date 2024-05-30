namespace PurchaseOperator.Domain.Models
{
    public class NotificationDataResult<T> where T : class
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public bool IsNotification { get; set; } = false;
        public T? Data { get; set; }
    }
}