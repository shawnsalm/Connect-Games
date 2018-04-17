using Connect_Games.DomainModels.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect_Games.DomainModels.Game
{
    public class Match : IMatch
    {
        public void PlayGame(IGame game)
        {
            int playerIndex = 0;

            while(game.CurrentGameResult == GameResult.NotComplete)
            {
                if(!game.TryMove(((IComputerPlayer)game.Players[playerIndex]).
                       CalculateNextMove(game.CurrentMoves, game.CurrentMovesPerPlayer)))
                {
                    throw new Exception("Invalid move bby player");
                }

                playerIndex++;

                if(playerIndex > 1)
                {
                    playerIndex = 0;
                }
            }
        }
    }
}
