//-----------------------------------------------------------------------
// Author: Luke Randazzo
// Free use MIT License. April, 2023.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;
using SDS;

#if UNITY_2019_2_OR_NEWER
using UnityEngine.TestTools;
#endif // UNITY_2019_2_OR_NEWER

public class TestRingPriorityQueue
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestRingPriorityQueueSortsIntegers()
    {
        int numRands = 50;
        int[] integerArray = new int[numRands];
        for (int i = 0; i < numRands; i++)
        {
            integerArray[i] = UnityEngine.Random.Range(0, 10000);
        }

        Span<int> randSpan = stackalloc int[numRands];

        RingPriorityQueue<int> ringPriorityQueue = new RingPriorityQueue<int>(randSpan, Comparer<int>.Default);
        for (int i = 0; i < numRands; i++)
        {
            ringPriorityQueue.Insert(integerArray[i]);
        }

        int lastVal = int.MinValue;
        // After this, the integer array will be completely sorted.
        int outIdx = 0;
        int numFound = 0;
        while (ringPriorityQueue.TryDequeue(out int val))
        {
            numFound++;
            integerArray[outIdx++] = val;
            Assert.GreaterOrEqual(val, lastVal);
            lastVal = val;
        }

        Assert.AreEqual(numRands, numFound);
    }
}
