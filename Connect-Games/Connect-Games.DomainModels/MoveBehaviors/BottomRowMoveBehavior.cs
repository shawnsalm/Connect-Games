using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Games.DomainModels.MoveBehaviors
{
    /// <summary>
    /// Models the behavior allowed by a game such as
    /// connect four where the pieces are stacked on 
    /// top of one another.
    /// </summary>
    [Serializable]
    public sealed class BottomRowMoveBehavior : IMoveBehavior
    {
        private readonly int _width;
        private readonly int _height;

        public BottomRowMoveBehavior(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Returns the available moves left given a list of 
        /// already made moves.
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        public IList<int> GetAvailableMoves(IEnumerable<int> moves)
        {
            // If we were passed a null list, create an empty one.
            var currentMoves = (moves == null) ? new List<int>() : new List<int>(moves);

            var availableMoves = new ConcurrentBag<int>();

            // Seed the total moves available.  We turn the matrix on its
            // side to make the Calculation for available moves per column easier.
            var totalMovesByColumns = new int[_width][];

            for (var rowIndex = 0; rowIndex < _width; rowIndex++)
            {
                totalMovesByColumns[rowIndex] = new int[_height];

                for (var columnIndex = 0; columnIndex < _height; columnIndex++)
                {
                    totalMovesByColumns[rowIndex][columnIndex] = (columnIndex * _width) + rowIndex;
                }
            }

            // Check every column (which in this case, we've turned the matrix on its side so that
            // testing the columns is by row) and add the highest number that isn't already
            // taken in the current moves list.  If there is no more available, done add anything 
            // for that column.
            // The use of Parallel.For in almost any real case would cause more overhead
            // then just using a normal for loop. It's used here just for fun.
            Parallel.For(0, _width,
                         columnIndex =>
                         {
                             var availableMove = (from move in totalMovesByColumns[columnIndex].Except(currentMoves)
                                                  select (int?)move).Max();

                             if (availableMove.HasValue)
                             {
                                 availableMoves.Add(availableMove.Value);
                             }
                         });

            return availableMoves.ToList();
        }
    }
}
