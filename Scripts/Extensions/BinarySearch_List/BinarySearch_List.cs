using System;
using UnityEngine;
using System.Collections.Generic;

namespace Extensions.BinarySearch
{
    public static class BinarySearch_List
    {
        #region Binary Search and Binary Insert Sort

        private static int SearchInsertIndex_R<T>(List<T> list, int low, int high, object value)
        where T : IComparable
        {
            if (low >= high) return low;

            int mid = low + (high - low) / 2;

            int comparer = list[mid].CompareTo(value);

            if (comparer > 0)
                return SearchInsertIndex_R(list, low, mid, value);
            else if (comparer < 0)
                return SearchInsertIndex_R(list, mid + 1, high, value);

            // reached target 
            return mid;
        }

        /// <summary>
        /// Search insert index for value inside list
        /// Use recursion
        /// </summary>
        /// <typeparam name="T">datatype</typeparam>
        /// <param name="list">collection of T</param>
        /// <param name="low">search from index</param>
        /// <param name="high">max index</param>
        /// <param name="value">for compare</param>
        /// <returns>Insert index</returns>
        public static int BinarySearch_R<T>(this List<T> list, int low, int high, object value)
            where T : IComparable
        {
            low = Mathf.Clamp(low, 0, list.Count);
            high = Mathf.Clamp(high, 0, list.Count);

            return SearchInsertIndex_R(list, low, high, value);
        }

        /// <summary>
        /// Search insert index for value inside list
        /// Use recursion
        /// </summary>
        /// <typeparam name="T">datatype</typeparam>
        /// <param name="list">collection of T</param>
        /// <param name="value">for compare</param>
        /// <returns>Insert index</returns>
        public static int BinarySearch_R<T>(this List<T> list, object value)
            where T : IComparable
        {
            return BinarySearch_R<T>(list, 0, list.Count, value);
        }

        /// <summary>
        /// Insert value into List
        /// </summary>
        /// <typeparam name="T">type of IComparable</typeparam>
        /// <param name="list">Container</param>
        /// <param name="value">insert value</param>
        public static int Insert_R<T>(this List<T> list, object value)
            where T : IComparable
        {
            int insertIndex = list.BinarySearch_R(value);
            list.Insert(insertIndex, (T)value);
            return insertIndex;
        }

        /// <summary>
        /// Binary insert sort recursion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void BinarySort_R<T>(this List<T> list)
            where T : IComparable
        {
            int count = list.Count;
            for (int i = 1; i < count; i++)
            {
                T value = list[i];
                int index = SearchInsertIndex_R(list, 0, i, value);

                list.Insert(index, value);
                list.RemoveAt(i + 1);
            }
        }

        /// <summary>
        /// Find index of value and update it, or insert new
        /// </summary>
        /// <typeparam name="T">IComparable type</typeparam>
        /// <param name="list">collection</param>
        /// <param name="value">value for update or insert</param>
        /// <returns>true if update otherwise insert</returns>
        public static bool UpdateOrInsert_R<T>(this List<T> list, object value)
            where T : IComparable
        {
            int count = list.Count;
            int insertIndex = list.BinarySearch_R(0, count, value);

            if (insertIndex < count)
            {
                if (list[insertIndex].CompareTo(value) == 0)
                {
                    list[insertIndex] = (T)value;
                    return true;
                }
            }

            list.Insert(insertIndex, (T)value);
            return false;
        }

        /// <summary>
        /// Remove by binary search
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Remove_R<T>(this List<T> list, object value)
            where T : IComparable
        {
            int count = list.Count;
            int index = list.BinarySearch_R(0, count, value);

            if (index < count &&
                list[index].CompareTo(value) == 0)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get object inside list
        /// </summary>
        /// <typeparam name="T">IComparable type</typeparam>
        /// <param name="list">collection</param>
        /// <param name="value">compare value</param>
        /// <returns></returns>
        public static T FirstOrDefault_R<T>(this List<T> list, object value)
            where T : IComparable
        {
            int count = list.Count;
            int insertIndex = list.BinarySearch_R(0, count, value);

            if (insertIndex < count)
            {
                if (list[insertIndex].CompareTo(value) == 0)
                {
                    return list[insertIndex];
                }
            }
            return default(T);
        }

        #endregion
    }
}