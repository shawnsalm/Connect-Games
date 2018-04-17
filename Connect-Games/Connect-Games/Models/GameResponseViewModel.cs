namespace Connect_Games.Models
{
    /// <summary>
    /// Used as an output message for the training of computer students.
    /// </summary>
    public class GameResponseViewModel
    {
        public bool IsGameFinsihed { get; set; }
        public string Winner { get; set; }
        public int[] Sequence { get; set; }
        public int ComputerMove;
        public int[] WinningSequence { get; set; }
        public bool WinningWentFirst { get; set; }
        public string Info { get; set; }
    }
}
