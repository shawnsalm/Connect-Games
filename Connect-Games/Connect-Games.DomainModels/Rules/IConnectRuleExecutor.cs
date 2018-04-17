using System.Collections.Generic;

namespace Connect_Games.DomainModels.Rules
{
    /// <summary>
    /// Excutes connection rules to determine if
    /// there is a winner.
    /// </summary>
    public interface IConnectRuleExecutor
    {
        bool IsWinner(IList<int> playersMoves, out IList<IList<int>> winningSequences);
    }
}
