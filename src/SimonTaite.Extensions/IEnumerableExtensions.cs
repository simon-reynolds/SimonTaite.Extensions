using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SimonTaite.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T element)
        {
            return ToUnarySequence(element);
        }
        
        public static IEnumerable<T> ToUnarySequence<T>(this T element)
        {
            yield return element;
        }
        
        public static IEnumerable<T> Tail<T>(this IEnumerable<T> source)
        {
            Contract.Requires<ArgumentOutOfRangeException>(source.Any());
            
            return source.Skip(1);
        }
        
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, int pageLength)
        {
            return source.Partition(pageLength);
        }
        
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageSize, int pageNumber)
        {
            return source.Skip(pageNumber * pageSize).Take(pageSize);
        }
        
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int partitionSize)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            
            if (partitionSize <= 0)
            {
                yield return source;
            }
            else
            {
                T[] array = new T[partitionSize];
                int i = 0;

                foreach (T item in source)
                {
                    array[i] = item;
                    i++;

                    if (i == partitionSize)
                    {
                        yield return array;
                        array = new T[partitionSize];
                        i = 0;
                    }
                }

                if (i != 0)
                {
                    Array.Resize(ref array, i);
                    yield return array;
                }
            }
        }
        
        public static IEnumerable<T> DistinctByProperty<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(selector != null);
            
            return source.GroupBy(selector).Select(g => g.First());
        }
        
        public static IEnumerable<T> DynamicDistinct<T>(this IEnumerable<T> source, Func<T, T, bool> func)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(func != null);
            
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