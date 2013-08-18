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

        public static TResult SelectIfSingleOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            return source.Count(predicate) == 1
                ? source.Select(selector).Single()
                : default(TResult);
        }

        public static TResult SelectSingleOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector) != null
                ? source.Select(selector).Single()
                : default(TResult);
        }

        #endregion
    }
}
