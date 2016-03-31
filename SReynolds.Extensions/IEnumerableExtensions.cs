using System;
using System.Collections.Generic;
using System.Linq;

namespace SReynolds.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DynamicDistinct<T>(this IEnumerable<T> source, Func<T, T, bool> func)
            where T : class
        {
            var dynamicComparer = new DynamicComparer<T>(func); 
            
            return source.Distinct(dynamicComparer);
        }

        private sealed class DynamicComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equalityFunc;
            
            public DynamicComparer(Func<T, T, bool> equalityFunc)
            {
                _equalityFunc = equalityFunc;
            }
            
            public bool Equals(T x, T y)
            {
                return _equalityFunc(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0; // this forces evaluation of equals
            }
        }
    }
}