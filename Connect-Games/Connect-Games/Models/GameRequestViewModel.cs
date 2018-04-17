namespace Connect_Games.Models
{
    /// <summary>
    /// Used as an input message for the training of computer students.
    /// </summary>
    public class GameRequestViewModel
    {
        public int[] Sequence { get; set; }
        public bool DoesPlayerGoFirst { get; set; }
    }
}
