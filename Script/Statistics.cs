//-----------------------------------------------------------------------
// Author: Luke Randazzo
// Free use MIT License. April, 2023.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

// For our debug assertions, this will be included if it's a unity project
#if UNITY_2019_2_OR_NEWER
using UnityEngine;
#else
using System.Diagnostics;
#endif // UNITY_EDITOR

namespace SDS
{
    /// <summary>
    /// A library of functions related to pulling information from Spans: median, mean, variance, etc.
    /// </summary>
    public static class Statistics
    {
        /// <summary>
        /// Find the median value in the span. Rearranges the span as necessary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T FindMedianDetructive<T>(Span<T> span, IComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            int length = span.Length;
            int middleIndex = length / 2;

            int leftIndex = 0;
            int rightIndex = length - 1;

            while (leftIndex <= rightIndex)
            {
                int pivotIndex = Partition(span, leftIndex, rightIndex, comparer);
                if (pivotIndex == middleIndex)
                {
                    return span[middleIndex];
                }
                else if (pivotIndex < middleIndex)
                {
                    leftIndex = pivotIndex + 1;
                }
                else
                {
                    rightIndex = pivotIndex - 1;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Private function. Partition the span i.e. Quicksort. 
        /// </summary>
        private static int Partition<T>(Span<T> span, int leftIndex, int rightIndex, IComparer<T> comparer)
        {
            int pivotIndex = leftIndex;
            T pivotValue = span[pivotIndex];

            int i = leftIndex + 1;
            int j = rightIndex;

            while (i <= j)
            {
                if (comparer.Compare(span[i], pivotValue) <= 0)
                {
                    i++;
                    continue;
                }

                if (comparer.Compare(span[j], pivotValue) >= 0)
                {
                    j--;
                    continue;
                }

                Swap(span, i, j);
            }

            Swap(span, pivotIndex, j);
            return j;
        }

        /// <summary>
        /// Swap the elements at two indices.
        /// </summary>
        private static void Swap<T>(Span<T> span, int i, int j)
        {
            T temp = span[i];
            span[i] = span[j];
            span[j] = temp;
        }
    }
}
