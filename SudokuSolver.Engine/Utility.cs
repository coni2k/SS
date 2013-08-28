using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Engine
{
    public static class Utility
    {
        #region - LINQ -

        public static TSource IfSingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Count(predicate) == 1
                ? source.Single(predicate)
                : default(TSource);
        }

        #endregion
    }
}
