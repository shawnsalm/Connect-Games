namespace Connect_Games.DomainModels.Rules
{
    /// <summary>
    /// Rule used to test if a connection is made.
    /// </summary>
    public interface IConnectRule
    {
        /// <summary>
        /// Will return true if the move results in matching the
        /// rule's connection rule.
        /// </summary>
        /// <param name="prevMove"></param>
        /// <param name="currentMove"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        bool? ExecuteRule(int prevMove, int currentMove, int width);
    }
}
