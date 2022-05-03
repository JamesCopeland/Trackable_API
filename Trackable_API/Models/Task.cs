namespace Trackable_API.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Trace { get; set; }

    }
}
