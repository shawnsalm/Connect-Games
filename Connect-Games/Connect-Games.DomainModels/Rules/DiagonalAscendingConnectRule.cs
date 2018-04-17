using System;

namespace Connect_Games.DomainModels.Rules
{
    [Serializable]
    public sealed class DiagonalAscendingConnectRule : IConnectRule
    {
        /// <summary>
        ///  Test for diagonal Right to Left connection (two elements in a row).
        /// </summary>
        /// <param name="prevMove"></param>
        /// <param name="currentMove"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool? ExecuteRule(int prevMove, int currentMove, int width)
        {
            return (currentMove < (prevMove + width - 1)
                        ? null
                        : (bool?)(currentMove == (prevMove + width - 1) &&
                                   ((prevMove % width) != 0)));
        }
    }
}
