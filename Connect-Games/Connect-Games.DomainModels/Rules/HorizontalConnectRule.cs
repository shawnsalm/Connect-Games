using System;

namespace Connect_Games.DomainModels.Rules
{
    [Serializable]
    public sealed class HorizontalConnectRule : IConnectRule
    {
        /// <summary>
        /// Test for horizontal connection (two elements in a row).
        /// </summary>
        /// <param name="prevMove"></param>
        /// <param name="currentMove"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool? ExecuteRule(int prevMove, int currentMove, int width)
        {
            return currentMove == (prevMove + 1) && (currentMove % width) != 0;
        }
    }
}
