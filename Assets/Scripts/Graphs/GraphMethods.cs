using System;
using System.Collections.Generic;

public class GraphMethods
{
    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool All<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        foreach (var item in source)
        {
            if (!predicate(item))
                return false;
        }
        return true;
    }
    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool Any<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate) 
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        foreach (var item in source)
        {
            if (predicate(item))
                return true;
        }
        return false;
    }
    /// <summary>
    /// Determines whether a sequence contains a specified element by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        var comparer = EqualityComparer<TSource>.Default;
        
        foreach (var element in source)
        {
            if (comparer.Equals(element, item))
                return true;
        }
        return false;
    }
    /// <summary>
    /// Determines whether a sequence contains a specified element by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        foreach (var element in source)
        {
            if (comparer.Equals(element, item))
                return true;
        }
        return false;
    }
    /// <summary>
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source)
    {
        return Distinct(source, EqualityComparer<TSource>.Default);
    }
    /// <summary>
    /// Returns distinct elements from a sequence by using a specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
        //Time complexity: O(n)
        //Memory complexity: O(n)
        var result = new HashSet<TSource>();

        foreach (var element in source)
        {
            if (result.Count == 0)
            {
                result.Add(element);
            }
            else
            {
                var isDistinct = true;

                foreach (var t in result)
                {
                    if (comparer.Equals(element, t))
                    {
                        isDistinct = false;
                        break;
                    }
                }

                if (isDistinct)
                {
                    result.Add(element);
                }
            }       
        }
        return result;
    }
    /// <summary>
    /// Returns the element at a specified index in a sequence.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static TSource ElementAt<TSource>(IEnumerable<TSource> source, int index)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        int count = 0;

        foreach (var item in source)
        {
            if (count == index) // Verifica si se alcanzó el índice.
                return item;
            count++;
        }
        throw new ArgumentOutOfRangeException(nameof(index));
    }
    /// <summary>
    /// Produces the set difference of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        //Time complexity: O(n + m)
        //Memory complexity: O(n)
        var set = new HashSet<TSource>(source2);

        foreach (var item in source1)
        {
            if (!set.Contains(item))
                yield return item;
        }
    }
    /// <summary>
    /// Produces the set difference of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        //Time complexity: O(n1 + n2)
        //Memory complexity: O(n2)
        HashSet<TSource> set = new HashSet<TSource>(source2, comparer);

        foreach (TSource item in source1)
        {
            if (!set.Contains(item))
            {
                yield return item;
            }
        }
    }
    /// <summary>
    /// Returns the first element in a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource First<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                return item;
            }
        }
        throw new InvalidOperationException("No element satisfies the condition.");
    }
    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Last<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        TSource result = default;
        bool found = false;

        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                result = item;
                found = true;
            }
        }

        if (!found)
        {
            throw new InvalidOperationException("No element satisfies the condition.");
        }

        return result;
    }
    /// <summary>
    /// Produces the set intersection of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        if (source1 == null) 
            throw new ArgumentNullException(nameof(source1));
        if (source2 == null) 
            throw new ArgumentNullException(nameof(source2));

        HashSet<TSource> set = new HashSet<TSource>(source2);

        foreach (TSource item in source1)
        {
            // Verificar si el elemento está en el conjunto y eliminarlo para evitar duplicados
            if (set.Remove(item))
            {
                yield return item;
            }
        }
    }
    /// <summary>
    /// Produces the set intersection of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        //Time complexity: O(n1 + n2)
        //Memory complexity: O(n2 + k)
        HashSet<TSource> set = new HashSet<TSource>(source2, comparer);
        HashSet<TSource> seen = new HashSet<TSource>(comparer);

        foreach (TSource item in source1)
        {
            if (set.Contains(item) && !seen.Contains(item))
            {
                seen.Add(item);
                yield return item;
            }
        }
    }
    /// <summary>
    /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int Count<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        int count = 0;

        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                count++;
            }
        }
        return count;
    }
    /// <summary>
    /// Determines whether two sequences are equal by comparing their elements by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool SequenceEqual<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        //Time complexity: O(min(n1,n2)))
        //Memory complexity: O(1)
        using IEnumerator<TSource> enumerator1 = source1.GetEnumerator();
        using IEnumerator<TSource> enumerator2 = source2.GetEnumerator();

        while (enumerator1.MoveNext())
        {
            if (!enumerator2.MoveNext() || !comparer.Equals(enumerator1.Current, enumerator2.Current))
            {
                return false;
            }
        }

        return !enumerator2.MoveNext();
    }
    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Single<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        TSource result = default;
        bool found = false;

        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                if (found)
                {
                    throw new InvalidOperationException("More than one element satisfies the condition.");
                }
                result = item;
                found = true;
            }
        }

        if (!found)
        {
            throw new InvalidOperationException("No element satisfies the condition.");
        }

        return result;
    }
    /// <summary>
    /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> SkipWhile<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        //Time complexity: O(n)
        //Memory complexity: O(1)
        bool skipping = true;

        foreach (TSource item in source)
        {
            if (skipping && !predicate(item))
            {
                skipping = false;
            }

            if (!skipping)
            {
                yield return item;
            }
        }
    }
    /// <summary>
    /// Produces the set union of two sequences by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        HashSet<TSource> seen = new HashSet<TSource>();

        foreach (TSource item in source1)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }

        foreach (TSource item in source2)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }
    }
    /// <summary>
    /// Produces the set union of two sequences by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        if (source1 == null) throw new ArgumentNullException(nameof(source1));
        if (source2 == null) throw new ArgumentNullException(nameof(source2));

        // HashSet para evitar duplicados
        HashSet<TSource> seen = new HashSet<TSource>(comparer);

        // Agregar elementos de source1
        foreach (TSource item in source1)
        {
            if (seen.Add(item)) // Agrega solo si no está presente
            {
                yield return item;
            }
        }

        // Agregar elementos de source2
        foreach (TSource item in source2)
        {
            if (seen.Add(item)) // Agrega solo si no está presente
            {
                yield return item;
            }
        }
    }
    /// <summary>
    /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        foreach (TSource item in source)
        {
            if (predicate(item)) // Evaluar el predicado
            {
                yield return item; // Devolver el elemento si cumple la condición
            }
        }
    }

    public static List<TSource> ToList<TSource>(IEnumerable<TSource> source)
    {
        List<TSource> list = new List<TSource>();
        foreach (TSource data in source)
        {
            list.Add(data);
        }

        return list;
    }
}
