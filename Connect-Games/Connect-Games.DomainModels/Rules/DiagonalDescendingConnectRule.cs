using System;

namespace Connect_Games.DomainModels.Rules
{
    [Serializable]
    public sealed class DiagonalDescendingConnectRule : IConnectRule
    {
        /// <summary>
        ///  Test for diagonal Left to Right connection (two elements in a row).
        /// </summary>
        /// <param name="prevMove"></param>
        /// <param name="currentMove"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool? ExecuteRule(int prevMove, int currentMove, int width)
        {
            return (currentMove < (prevMove + width + 1)
                        ? null
                        : (bool?)(currentMove == (prevMove + width + 1) &&
                                   ((currentMove % width) != 0 || prevMove == 0)));
        }
    }
}
