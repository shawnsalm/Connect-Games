using Connect_Games.DomainModels.Game;
using Connect_Games.DomainModels.MoveBehaviors;
using Connect_Games.DomainModels.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect_Games.DomainModels.AI
{
    /// <summary>
    /// Represents a default computer player that uses
    /// a simple algorithm for determining its next move.
    /// </summary>
    [Serializable]
    public class ComputerPlayer : ConnectPlayer, IComputerPlayer
    {
        #region Private Member Variables

        private readonly IMoveBehavior _moveBehavior;
        private readonly IConnectRuleExecutor _connectRuleExecutor;
        private readonly IGameHistoryRepository _gameHistoryRepository;
        private readonly int _computerPlayerIndex;
        private readonly int _gameType;

        #endregion

        #region Constructors

        public ComputerPlayer(IMoveBehavior moveBehavior,
                                 IConnectRuleExecutor connectRuleExecutor,
                                 IGameHistoryRepository gameHistoryRepository,
                                 int computerPlayerIndex,
                                 Guid playerId,
                                 int gameType,
                                 string name) : base(playerId, name)
        {
            _moveBehavior = moveBehavior;
            _connectRuleExecutor = connectRuleExecutor;
            _gameHistoryRepository = gameHistoryRepository;
            _computerPlayerIndex = computerPlayerIndex;
            _gameType = gameType;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Calculates the next move the computer player will make
        /// </summary>
        /// <param name="currentMoves"></param>
        /// <param name="allPlayersMoves"></param>
        /// <returns></returns>
        public int CalculateNextMove(IList<int> currentMoves, IList<IList<int>> allPlayersMoves)
        {
            var availableMoves = _moveBehavior.GetAvailableMoves(currentMoves);

            // can I win
            var nextMove = CalculateWinningMove(allPlayersMoves[_computerPlayerIndex], availableMoves);

            if (nextMove.HasValue)
            {
                return nextMove.Value;
            }

            // can I lose
            nextMove = CalculateLossBlockingMove(allPlayersMoves, availableMoves);

            if (nextMove.HasValue)
            {
                return nextMove.Value;
            }

            // is there an existing winning sequence
            nextMove = CalculateFromExistingSequencesMove(currentMoves, availableMoves);

            if (nextMove.HasValue)
            {
                return nextMove.Value;
            }

            // randomly pick a move
            nextMove = CalculateRandomMove(availableMoves);

            List<int> tempAvailableMoves = availableMoves.ToList();

            int? tempNextMove = null;
            int? currentNextMove = nextMove;

            while (tempAvailableMoves.Count > 0)
            {
                tempAvailableMoves.Remove(currentNextMove.Value);

                // see if this move will help the opp.
                List<List<int>> tempAllPlayersMoves = new List<List<int>>();

                foreach(var moves in allPlayersMoves)
                {
                    tempAllPlayersMoves.Add(moves.ToList());
                }

                tempAllPlayersMoves[_computerPlayerIndex].Add(currentNextMove.Value);

                var temp2AvailableMoves = _moveBehavior.GetAvailableMoves(tempAllPlayersMoves.SelectMany(t => t));

                int computerPlayerIndex = _computerPlayerIndex == 0 ? 1 : 0;

                tempNextMove = CalculateWinningMove(tempAllPlayersMoves[computerPlayerIndex], temp2AvailableMoves);

                if(tempNextMove == null)
                {
                    break;
                }

                if(tempAvailableMoves.Count == 0)
                {
                    break;
                }

                tempNextMove = tempAvailableMoves.First();

                if(tempNextMove == null)
                {
                    break;
                }
                else
                {
                    currentNextMove = tempNextMove;
                }
            }
            

            return currentNextMove.Value;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tries to calculate a winning move.
        /// </summary>
        /// <param name="myMoves"></param>
        /// <param name="availableMoves"></param>
        /// <returns></returns>
        private int? CalculateWinningMove(ICollection<int> myMoves, IEnumerable<int> availableMoves)
        {
            int? nextMove = null;

            var currentGameMovesCount = myMoves.Count;

            if (currentGameMovesCount > 0)
            {
                // Copy the current moves and add one place of our next move for testing
                // if we can win.
                IList<int> testGameMoves = new List<int>(myMoves) { -1 };

                // can I win
                foreach (var move in availableMoves)
                {
                    IList<IList<int>> winningSequences;

                    testGameMoves[currentGameMovesCount] = move;

                    if (!_connectRuleExecutor.IsWinner(testGameMoves, out winningSequences)) continue;
                    nextMove = move;
                    break;
                }
            }

            return nextMove;
        }

        /// <summary>
        /// Tries to determine a way of blocking the other player
        /// from winning.
        /// </summary>
        /// <param name="allPlayersMoves"></param>
        /// <param name="availableMoves"></param>
        /// <returns></returns>
        private int? CalculateLossBlockingMove(IList<IList<int>> allPlayersMoves, IEnumerable<int> availableMoves)
        {
            int? nextMove = null;

            for (var playerMovesIndex = 0; playerMovesIndex < allPlayersMoves.Count; playerMovesIndex++)
            {
                if (playerMovesIndex != _computerPlayerIndex && allPlayersMoves[playerMovesIndex] != null
                    && allPlayersMoves[playerMovesIndex].Count > 0)
                {
                    var testGameMoves = new List<int>(allPlayersMoves[playerMovesIndex]) { -1 };

                    foreach (var move in availableMoves)
                    {
                        IList<IList<int>> winningSequences;

                        testGameMoves[testGameMoves.Count - 1] = move;

                        if (!_connectRuleExecutor.IsWinner(testGameMoves, out winningSequences)) continue;
                        nextMove = move;
                        break;
                    }
                }
            }

            return nextMove;
        }

        /// <summary>
        /// Tries to use past game histories to determine the next move.
        /// </summary>
        /// <param name="currentMoves"></param>
        /// <returns></returns>
        private int? CalculateFromExistingSequencesMove(IList<int> currentMoves, IEnumerable<int> availableMoves)
        {
            int? nextMove = null;

            var existingGameHistories = _gameHistoryRepository.FindGameHistoryHighestPriority(GameResult.Win,
                                                                                            PlayerId,
                                                                                            currentMoves,
                                                                                            _gameType);

            // if not, are we on a sequence in which we have lost before and if so
            // can we make a different move
            foreach (var existingGameHistory in existingGameHistories)
            {
                // Confirm that we got back a sequence and that it is long enough to get
                // the next move.
                if (existingGameHistory != null && existingGameHistory.Moves != null &&
                    existingGameHistory.Moves.Count > currentMoves.Count &&
                    (_computerPlayerIndex % 2) == (existingGameHistory.Moves.Count % 2))
                {
                    if (availableMoves.Count() > 1)
                    {
                        foreach (var availableMove in availableMoves)
                        {
                            if (availableMove != existingGameHistory.Moves[currentMoves.Count])
                            {
                                nextMove = availableMove;
                                break;
                            }
                        }

                        if (nextMove != null)
                        {
                            break;
                        }
                    }
                }
            }


            if (nextMove == null)
            {
                // can we find a sequence where we win
                foreach (var existingGameHistory in existingGameHistories.OrderBy(x => Guid.NewGuid()))
                {
                    // Confirm that we got back a sequence and that it is long enough to get
                    // the next move.
                    if (existingGameHistory != null && existingGameHistory.Moves != null &&
                        existingGameHistory.Moves.Count > currentMoves.Count &&
                        (_computerPlayerIndex % 2) != (existingGameHistory.Moves.Count % 2))
                    {
                        nextMove = existingGameHistory.Moves[currentMoves.Count];
                        break;
                    }
                }

            }
        
            return nextMove;
        }

        /// <summary>
        /// If all else fails, make a random move.
        /// </summary>
        /// <param name="availableMoves"></param>
        /// <returns></returns>
        private static int CalculateRandomMove(IList<int> availableMoves)
        {
            // randomly pick a move
            var random = new Random();

            return availableMoves[random.Next(0, availableMoves.Count() - 1)];
        }

        #endregion
    }
}
