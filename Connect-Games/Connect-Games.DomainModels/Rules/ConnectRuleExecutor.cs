using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect_Games.DomainModels.Rules
{
    /// <summary>
    /// Excutes connection rules to determine if
    /// there is a winner.
    /// </summary>
    [Serializable]
    public sealed class ConnectRuleExecutor : IConnectRuleExecutor
    {
        #region Private Member Variables

        private readonly int _width;

        private readonly int _numberConnectedToWin;

        private readonly IList<IConnectRule> _connectRules;

        #endregion

        #region Constructors

        public ConnectRuleExecutor(int width, int numberConnectedToWin)
        {
            _width = width;
            _numberConnectedToWin = numberConnectedToWin;

            // initialize rules
            _connectRules = new List<IConnectRule>
                                {
                                    new HorizontalConnectRule(),
                                    new VerticalConnectRule(),
                                    new DiagonalAscendingConnectRule(),
                                    new DiagonalDescendingConnectRule()
                                };
        }

        #endregion

        #region Public Members

        public bool IsWinner(IList<int> playersMoves, out IList<IList<int>> winningSequences)
        {
            var localWinningSequences = new ConcurrentBag<IList<IList<int>>>();

            winningSequences = new List<IList<int>>();

            foreach (var connectRule in _connectRules)
            {
                localWinningSequences.Add(ExecuteRule(playersMoves, connectRule));
            }

            foreach (var listOfSequences in localWinningSequences)
            {
                ((List<IList<int>>)winningSequences).AddRange(listOfSequences);
            }

            return winningSequences.Count > 0;
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playersMoves"></param>
        /// <param name="connectRule"></param>
        /// <returns></returns>
        private IList<IList<int>> ExecuteRule(IEnumerable<int> playersMoves, IConnectRule connectRule)
        {
            // Returning winning sequences.
            var winningSequences = new List<IList<int>>();

            // Create a holder for tracking the current sequence being tested.
            var currentConnected = new List<int>();

            var prevMove = -1;

            // Local copy of players moves used to sort the moves.
            var localPlayersMoves = new List<int>(playersMoves);

            localPlayersMoves.Sort();

            bool won = false;

            // Process each player's moves against the game rule to determine 
            // if they have won.
            for (int i = 0; i < localPlayersMoves.Count; i++)
            {
                currentConnected.Clear();

                var currentMove = localPlayersMoves[i];
                // set the first connected item
                currentConnected.Add(currentMove);
                prevMove = currentMove;

                for (int j = i + 1; j < localPlayersMoves.Count; j++)
                {
                    currentMove = localPlayersMoves[j];

                    // Test to see if the current move complies to the current rule.
                    // If it does, add to the current connect sequence.
                    // Else, start the sequence over.
                    var results = connectRule.ExecuteRule(prevMove, currentMove, _width);

                    // skip the current validation if no value was returned
                    if (results.HasValue)
                    {
                        if (results.Value)
                        {
                            currentConnected.Add(currentMove);

                            prevMove = currentMove;
                        }
                    }

                    // do we have a winner
                    if (_numberConnectedToWin == currentConnected.Count)
                    {
                        winningSequences.Add(new List<int>(currentConnected));
                        currentConnected.Clear();
                        won = true;
                        break;
                    }
                }

                if(won)
                {
                    break;
                }
            }
            
            return winningSequences.Distinct(new ListComparer()).ToList();
        }

        #endregion
    }
}
