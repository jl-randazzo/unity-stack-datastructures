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
    /// A priority queue that has stack scope. This is useful for heavy-weight operations that would otherwise allocate a lot of memory.
    /// </summary>
    public ref struct RingPriorityQueue<T>
        where T : struct
    {
        private Span<T> _span;
        private IComparer<T> _c;

        private int _start;
        private int _end;
        private int _count;

        public RingPriorityQueue(Span<T> span, IComparer<T> comparer)
        {
            _span = span;
            _start = 0;
            _end = 0;
            _c = comparer;
            _count = 0;

            Debug.Assert(_count < _span.Length);
        }

        /// <summary>
        /// Insert the element into the RingPriorityQueue.
        /// </summary>
        /// <param name="elem"></param>
        public void Insert(T elem)
        {
            Debug.Assert(_count < _span.Length);

            if (_count == 0 || _c.Compare(elem, this[0]) <= 0)
            {
                Dec(ref _start);
                _span[_start] = elem;
                _count++;
                return;
            }
            else if (_c.Compare(elem, this[_count - 1]) >= 0)
            {
                _span[_end] = elem;
                Inc(ref _end);
                _count++;
                return;
            }

            // binary search for element
            int l = 0;
            int r = _count - 1;
            int selectIdx = -1;
            while (l <= r)
            {
                selectIdx = (l + r) / 2;

                int comp = _c.Compare(elem, this[selectIdx]);
                if (comp > 0)
                {
                    selectIdx++;
                    l = selectIdx;
                }
                else if (comp < 0)
                {
                    r = selectIdx - 1;
                }
                else
                {
                    InsertAt(selectIdx, elem);
                    _count++;
                    return;
                }
            }

            InsertAt(selectIdx, elem);
            _count++;
        }

        /// <summary>
        /// Try to Dequeue an element. Returns true if the operation is successful.
        /// </summary>
        public bool TryDequeue(out T val)
        {
            if (_count != 0)
            {
                _count--;
                val = _span[_start];
                Inc(ref _start);
                return true;
            }
            val = default(T);
            return false;
        }

        /// <summary>
        /// Remaps an index to the internal range.
        /// </summary>
        private ref T this[int i]
        {
            get
            {
                i += _start;
                i %= _span.Length;
                return ref _span[i];
            }
        }

        /// <summary>
        /// Increments a value (start or end) and wraps it back to the beginning
        /// </summary>
        private void Inc(ref int val)
        {
            val++;
            val %= _span.Length;
        }

        /// <summary>
        /// Decrements a value (start or end) and wraps it to the end.
        /// </summary>
        private void Dec(ref int val)
        {
            val--;
            if (val == -1)
            {
                val += _span.Length;
            }
        }

        /// <summary>
        /// Internal insert will push back or forward elements to make room.
        /// </summary>
        private void InsertAt(int idx, T elem)
        {
            int mid = _count / 2;
            if (idx < mid)
            {
                Dec(ref _start);
                for (int i = 1; i <= idx; i++)
                {
                    this[i - 1] = this[i];
                }

                // we need to increment this index since we moved back the start.
                this[idx + 1] = elem;
            }
            else
            {
                // move forward the end, copy everything down to the insertion index
                Inc(ref _end);
                for (int i = _count - 1; i >= idx; i--)
                {
                    this[i + 1] = this[i];
                }

                this[idx] = elem;
            }
        }
    }
}
