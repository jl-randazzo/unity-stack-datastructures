# Unity Stack Data Structures

  The following data structures are designed to work with `System.Span<T>` in C#, which can be allocated on the stack with C#'s `stackalloc` function. These data structures can of course be used outside of Unity, but structures such as these are uniquely well-suited to Game Development because of the unique requirement to avoid managed heap allocations where possible.

# Guide
## Install
To install, open Unity Package Manager, open the + dropdown, and select 'Add packge from git URL...'
In the popup window, paste the link to this repository.
[Unity Documentation](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
## Code Snippet

You can find examples in the test assembly. For example:
```
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SDS;

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
```
