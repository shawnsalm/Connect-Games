using System.Collections.Generic;
using System.Linq;


namespace Connect_Games.DomainModels.Rules
{
    /// <summary>
    /// Helper class for ConnectRuleExecutor 
    /// </summary>
    public sealed class ListComparer : IEqualityComparer<IList<int>>
    {
        public bool Equals(IList<int> x, IList<int> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IList<int> obj)
        {
            return obj.Sum().GetHashCode();
        }
    }
}
