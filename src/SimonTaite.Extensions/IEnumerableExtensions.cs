using System;
using System.Collections.Generic;
using System.Linq;

namespace SimonTaite.Extensions
{
    /// <summary>
    /// A group of extension methods for <see cref="IEnumerable&lt;T&gt;" />
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates an IEnumerable containing the single input element
        /// </summary>
        /// <param name="element"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this T element)
        {
            return ToUnarySequence(element);
        }
        
        /// <summary>
        /// Creates an IEnumerable containing the single input element
        /// </summary>
        /// <param name="element"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ToUnarySequence<T>(this T element)
        {
            yield return element;
        }

        /// <summary>
        /// An Alias for First()
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static T Head<T>(this IEnumerable<T> source)
            => source.First();
        
        /// <summary>
        /// Returns an <see cref="IEnumerable&lt;T&gt;" /> without its first element
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Tail<T>(this IEnumerable<T> source)
        {
            if (!source.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(source), @"source must contain at least one entry");
            }
            
            return source.Skip(1);
        }

        /// <summary>
        /// Iterates through an <see cref="IEnumerable&lt;T&gt;" />, performing the supplied <see cref="Action" />
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="T">The element type</typeparam>
        public static void Iterate<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Splits an <see cref="IEnumerable&lt;T&gt;" /> into a collection for pagination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageLength"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>        
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, int pageLength)
        {
            return source.Partition(pageLength);
        }
        
        /// <summary>
        /// Returns a given page from an <see cref="IEnumerable&lt;T&gt;" /> based on page size and number
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageSize, int pageNumber)
        {
            return source.Skip(pageNumber * pageSize).Take(pageSize);
        }
        
        /// <summary>
        /// Splits an <see cref="IEnumerable&lt;T&gt;" /> into smaller lists of a given size
        /// </summary>
        /// <param name="source"></param>
        /// <param name="partitionSize"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int partitionSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), @"source cannot be null");
            }
            
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
        
        /// <summary>
        /// Select Distinct entries in an <see cref="IEnumerable&lt;T&gt;" /> based on a property of <typeparamref name="T"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <typeparam name="TKey">The property of <typeparamref name="T"/> to select distinct entries with</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> DistinctByProperty<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), @"source cannot be null");
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), @"selector cannot be null");
            }
            
            return source.GroupBy(selector).Select(g => g.First());
        }
        
        /// <summary>
        /// Creates a default comparer for <see cref="IEnumerable&lt;T&gt;" /> based on the supplied func and selects distinct entries
        /// </summary>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <typeparam name="T">The element type</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> DynamicDistinct<T>(this IEnumerable<T> source, Func<T, T, bool> func)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), @"source cannot be null");
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func), @"func cannot be null");
            }
            
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