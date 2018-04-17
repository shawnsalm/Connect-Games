using System;

namespace Connect_Games.DomainModels.Rules
{
    [Serializable]
    public sealed class VerticalConnectRule : IConnectRule
    {
        /// <summary>
        /// Test for vertical connection (two elements in a column).
        /// </summary>
        /// <param name="prevMove"></param>
        /// <param name="currentMove"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool? ExecuteRule(int prevMove, int currentMove, int width)
        {
            return (currentMove < (prevMove + width)
                        ? null
                        : (bool?)(currentMove == (prevMove + width)));
        }
    }
}
