namespace Connect_Games.Models
{
    /// <summary>
    /// View model for displaying unhandled errors.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}