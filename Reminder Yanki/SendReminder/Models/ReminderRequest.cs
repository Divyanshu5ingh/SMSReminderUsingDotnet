namespace SendReminder.Models
{
    public class ReminderRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime ReminderTime { get; set; }
    }
}
