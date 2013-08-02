using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Engine
{
    public static class LINQExtension
    {
        public static TSource SingleIfLast<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Count(predicate) == 1
                ? source.Single()
                : default(TSource);
        }
    }
}
