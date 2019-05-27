using System;
using System.Collections.Generic;
using System.Linq;

namespace NConsole.Options
{
    using Xunit;
    using static StringComparison;

    internal static class AssertExtensionMethods
    {
        public static T AssertNotNull<T>(this T obj)
        {
            Assert.NotNull(obj);
            return obj;
        }

        public static object AssertNotSame(this object actual, object expected)
        {
            Assert.NotSame(expected, actual);
            return actual;
        }

        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        public static bool AssertTrue(this bool actual)
        {
            Assert.True(actual);
            return actual;
        }

        public static bool AssertFalse(this bool actual)
        {
            Assert.False(actual);
            return actual;
        }
        // ReSharper restore ConditionIsAlwaysTrueOrFalse

        public static T AssertEqual<T>(this T actual, T expected)
        {
            Assert.Equal(expected, actual);
            return actual;
        }

        public static IEnumerable<T> AssertEmpty<T>(this IEnumerable<T> actual)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.Empty(actual);
            return actual;
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static IEnumerable<T> AssertNotEmpty<T>(this IEnumerable<T> actual)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.NotEmpty(actual);
            return actual;
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static string AssertContains(this string actual, string expected, StringComparison comparison = CurrentCultureIgnoreCase)
        {
            Assert.Contains(expected, actual, comparison);
            return actual;
        }

        public static IEnumerable<T> AssertContains<T>(this IEnumerable<T> actual, T expected)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.Contains(expected, actual);
            return actual;
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static IEnumerable<T> AssertContainsAll<T>(this IEnumerable<T> actual, params T[] expected)
        {
            expected.ToList().ForEach(x => actual.AssertContains(x));
            return actual;
        }

        public static T AssertThrows<T>(this Action action)
            where T : Exception
            => Assert.Throws<T>(action);

        public static IDictionary<TKey, _> AssertContainsKeys<TKey, _>(this IDictionary<TKey, _> actual, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                Assert.Contains(key, actual);
            }

            return actual;
        }

        public static IDictionary<TKey, _> AssertDoesNotContainsKeys<TKey, _>(this IDictionary<TKey, _> actual, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                Assert.DoesNotContain(key, actual);
            }

            return actual;
        }

        public static IDictionary<TKey, TValue> AssertContains<TKey, TValue>(this IDictionary<TKey, TValue> actual, TKey expectedKey, TValue expectedValue)
        {
            Assert.Contains(expectedKey, actual);
            actual[expectedKey].AssertEqual(expectedValue);
            return actual;
        }

        public static IReadOnlyDictionary<TKey, _> AssertContainsKeys<TKey, _>(this IReadOnlyDictionary<TKey, _> actual, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                Assert.Contains(key, actual);
            }

            return actual;
        }

        public static IReadOnlyDictionary<TKey, _> AssertDoesNotContainsKeys<TKey, _>(this IReadOnlyDictionary<TKey, _> actual, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                Assert.DoesNotContain(key, actual);
            }

            return actual;
        }

        public static IReadOnlyDictionary<TKey, TValue> AssertContains<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> actual, TKey expectedKey, TValue expectedValue)
        {
            Assert.Contains(expectedKey, actual);
            actual[expectedKey].AssertEqual(expectedValue);
            return actual;
        }
    }
}
