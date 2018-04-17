using System;

namespace Connect_Games.DomainModels.Game
{
    /// <summary>
    /// Args for event that fires when a player moves.
    /// </summary>
    public class MoveMadeEventArgs : EventArgs
    {
        public MoveMadeEventArgs(int move, int playerIndex)
        {
            Move = move;
            PlayerIndex = playerIndex;
        }

        public int Move { get; private set; }

        public int PlayerIndex { get; private set; }
    }
}
