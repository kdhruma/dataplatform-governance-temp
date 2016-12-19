using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Core.Extensions
{
    /// <summary>
    /// Class for Collection Extensions
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks collection is empty is null
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="source">Collection to null check</param>
        /// <returns>True if source is null or empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return true;
            }

            var collection = source as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !source.Any();
        }

        /// <summary>
        /// Returns source collection or empty collection if source is null
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="source">Collection to null check</param>
        /// <returns>Current collection or empty collection if current one is null</returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
        
        /// <summary>
        /// Converts any collection that implements IEnumerable to Collection type
        /// </summary>
        /// <typeparam name="T">Type of collection items</typeparam>
        /// <param name="source">Collection source</param>
        /// <returns>Returns converted Collection or null if source is null</returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return null;
            }

            if (source is IList<T>)
            {
                return new Collection<T>(source as IList<T>);
            }

            return new Collection<T>(source.ToList());
        }

        /// <summary>
        /// Copies any collection with value type items to a new collection.
        /// </summary>
        /// <typeparam name="T">Type of collection items</typeparam>
        /// <param name="source">Collection source</param>
        /// <param name="onlyUniqueElements"></param>
        /// <returns></returns>
        public static Collection<T> CopyCollection<T>(this ICollection<T> source, Boolean onlyUniqueElements = false) where T : struct
        {
            if (source == null)
            {
                return null;
            }
            var collection = new Collection<T>();

            foreach(var element in source)
            {
                if (onlyUniqueElements)
                {
                    if (collection.Contains(element))
                        continue;
                }
                collection.Add(element);
            }

            return collection;
        }

        /// <summary>
        /// Copies/Clones any collection with reference type items to a new collection.
        /// </summary>
        /// <typeparam name="T">Type of collection items</typeparam>
        /// <param name="source">Collection source</param>
        /// <param name="onlyUniqueElements"></param>
        /// <returns></returns>
        public static Collection<T> CloneCollection<T>(this ICollection<T> source, Boolean onlyUniqueElements = false) where T : ICloneable
        {
            if (source == null)
            {
                return null;
            }
            var collection = new Collection<T>();

            foreach (var element in source)
            {
                if (onlyUniqueElements)
                {
                    if (collection.Contains(element))
                        continue;
                }
                collection.Add((T)element.Clone());
            }

            return collection;
        }

        /// <summary>
        /// Compares two collections ignoring order of their elements
        /// </summary>
        /// <typeparam name="T">Type of collection items</typeparam>
        /// <param name="first">First collection for comparison</param>
        /// <param name="second">Second collection for comparison</param>
        /// <returns>Returns true if collections are equal, false otherwise</returns>
        public static Boolean EqualsIgnoringOrder<T>(this ICollection<T> first, ICollection<T> second) where T: IEquatable<T>
        {
            if (ReferenceEquals(null, second)) return false;
            if (ReferenceEquals(first, second)) return true;

            return first.Count() == second.Count() &&
                   !first.Except(second).Any() &&
                   !first.Except(second).Any();
        }

        /// <summary>
        /// Distributes items by given separator
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="items">items to be distribute</param>
        /// <param name="separator">Item separator</param>
        /// <returns>Collection of item with separator</returns>
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> items, T separator)
        {
            var first = true;
            foreach (var item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    yield return separator;
                }
                yield return item;
            }
        }

        /// <summary>
        /// Concats the given items
        /// </summary>
        /// <param name="source">Collection of Items</param>
        /// <returns>Contacted String</returns>
        public static String Concat(this IEnumerable<String> source)
        {
            var sb = new StringBuilder();
            foreach (var s in source)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compare contains two collection and returns if collection1 is superset of collection2
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection1">Collection1</param>
        /// <param name="collection2">Collection2</param>
        /// <returns>True if both are same</returns>
        public static Boolean IsSuperSetOf<T>(this Collection<T> collection1, Collection<T> collection2)
        {
            if (collection1 == null || collection2 == null)
                return false;

            foreach (T item in collection2)
            {
                if (!collection1.Contains(item))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if collection 1 contains any of collection 2 items
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection1">Collection1</param>
        /// <param name="collection2">Collection2</param>
        /// <returns>True if both are same</returns>
        public static Boolean ContainsAny<T>(this Collection<T> collection1, Collection<T> collection2)
        {
            if (collection1 == null || collection2 == null)
                return false;

            foreach (T item in collection2)
            {
                if (collection1.Contains(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if dictionary contains given value
        /// </summary>
        /// <typeparam name="TKey">Represents type of key</typeparam>
        /// <typeparam name="TValue">Represents type of value</typeparam>
        /// <param name="dictionary">Represents target dictionary</param>
        /// <param name="value">Represents value for search</param>
        /// <returns>Returns true if dictionary contains given value, false otherwise</returns>
        public static Boolean ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            return dictionary.Values.Contains(value);
        }

        /// <summary>
        /// Splits into fixed-sized chunks
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, Int32 batchSize)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }

        /// <summary>
        /// Adds unique source collection to destination collection
        /// </summary>
        /// <typeparam name="T">Represents type of T</typeparam>
        /// <param name="destination">Indicates destination collection</param>
        /// <param name="source">Indicates source collection</param>
        /// <param name="onlyUniqueElements">Indicates only unique elements to be added</param>
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source, Boolean onlyUniqueElements = true)
        {
            if (source != null && source.Count() > 0)
            {
                foreach (T item in source)
                {
                    if (onlyUniqueElements)
                    {
                        if (destination.Contains(item))
                        {
                            continue;
                        }
                    }

                    destination.Add(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, Int32 batchSize)
        {
            yield return source.Current;
            for (Int32 i = 0; i < batchSize && source.MoveNext(); i++)
            {
                yield return source.Current;
            }
        } 
    }
}