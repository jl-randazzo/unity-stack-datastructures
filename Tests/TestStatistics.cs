//-----------------------------------------------------------------------
// Author: Luke Randazzo
// Free use MIT License. May, 2023.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;
using SDS;

#if UNITY_2019_2_OR_NEWER
using UnityEngine.TestTools;
#endif // UNITY_2019_2_OR_NEWER

public class TestStatistics
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestFindMedianValue()
    {
        Span<int> span = stackalloc int[] { 1, 1, -3, 20, 4, 5, 8 };
        Assert.AreEqual(4, Statistics.FindMedianDestructive(span));

        Span<float> floatSpan = stackalloc float[] { 0.0f, -32.0f, 32.0f, 11.0f, -10.0f };
        Assert.AreEqual(0.0f, Statistics.FindMedianDestructive(floatSpan));
    }
}
