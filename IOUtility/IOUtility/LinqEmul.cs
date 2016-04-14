﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IOUtilityCLR
{
    public class LinqEmul
    {
        public delegate bool ItemPredicateHandler<T>(T item);

        public static bool Any<T>(IEnumerable<T> collection, ItemPredicateHandler<T> predicate)
        {
            foreach (T item in collection)
            {
                if (predicate == null || predicate(item))
                    return true;
            }

            return false;
        }

        public static IEnumerable<T> SkipWhile<T>(IEnumerable<T> collection, ItemPredicateHandler<T> predicate)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            try
            {
                return LinqEmul.SkipWhile<T>(enumerator, predicate);
            }
            catch
            {
                throw;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static IEnumerable<T> SkipWhile<T>(IEnumerator<T> enumerator, ItemPredicateHandler<T> predicate)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            bool matched = false;
            while (enumerator.MoveNext())
            {
                if (!predicate(enumerator.Current))
                {
                    matched = true;
                    yield return enumerator.Current;
                    break;
                }
            }
            if (matched)
            {
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
        }

        public static int Count<T>(IEnumerable<T> collection, ItemPredicateHandler<T> predicate)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            try
            {
                return LinqEmul.Count<T>(enumerator, predicate);
            }
            catch
            {
                throw;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static int Count<T>(IEnumerator<T> enumerator, ItemPredicateHandler<T> predicate)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            int count = 0;
            if (predicate == null)
            {
                while (enumerator.MoveNext())
                    count++;
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    if (predicate(enumerator.Current))
                        count++;
                }
            }

            return count;
        }
    }
}