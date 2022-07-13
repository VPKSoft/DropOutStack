#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace VPKSoft.DropOutStack
{
    /// <summary>
    /// A class for limited capacity dropout stack.
    /// Implements the <see cref="System.Collections.Generic.IEnumerable{T}" />
    /// Implements the <see cref="System.Collections.Generic.IEnumerator{T}" />
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    /// <seealso cref="System.Collections.Generic.IEnumerator{T}" />
    [Serializable]
    public class DropOutStack<T> : Stack<T>, IEnumerable<T>, IEnumerator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropOutStack{T}"/> class.
        /// </summary>
        /// <param name="capacity">The maximum capacity of the stack.</param>
        public DropOutStack(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), @"The capacity must be at least one.");
            }

            Initialize(capacity);
        }

        private T[] items;
        private bool isDisposed;
        private int capacity;
        private int position;
        private T current;
        private int cursor;

        #region PublicProperties                        
        /// <summary>
        /// Gets the <typeparamref name="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index - The index is out of range.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    // throw new ArgumentOutOfRangeException(nameof(index), @"The index is out of range.");
                }

                return items[(Count - index + cursor - 1) % Count];
            }
        }

        /// <summary>
        /// Gets or sets the maximum capacity for items for this <see cref="DropOutStack{T}"/> instance. 
        /// </summary>
        /// <value>The maximum capacity for items for this <see cref="DropOutStack{T}"/> instance.</value>
        /// <remarks>Setting the capacity value to a new one will clear the data stored in this <see cref="DropOutStack{T}"/> instance.</remarks>
        public int Capacity
        {
            get => capacity;

            set
            {
                if (value != capacity)
                {
                    Initialize(value);
                }
                else
                {
                    Reset();
                }
            }
        }

        /// <summary>
        ///Gets the number of elements contained in the <see cref="DropOutStack{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="DropOutStack{T}"/>.</value>
        public new int Count { get; private set; }
        #endregion

        #region StackMethods
        /// <summary>
        /// Removes all objects from the <see cref="DropOutStack{T}"/>.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public new void Clear()
        {
            Capacity = Capacity;
        }

        /// <summary>
        /// Returns the object at the top of the <see cref="DropOutStack{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the top of the <see cref="DropOutStack{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="DropOutStack{T}"/> is empty.</exception>
        // ReSharper disable once UnusedMember.Global
        public new T Peek()
        {
            if (Count > 0)
            {
                return items[cursor];
            }

            throw new InvalidOperationException("The DropOutStack<T> is empty.");
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="DropOutStack{T}"/>.
        /// </summary>
        /// <returns>The object removed from the top of the <see cref="DropOutStack{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="DropOutStack{T}"/> is empty.</exception>
        // ReSharper disable once UnusedMember.Global
        public new T Pop()
        {
            if (Count > 0)
            {
                Count--;

                cursor = (items.Length + cursor - 1) % items.Length;
                var result = items[cursor];

                return result;
            }

            throw new InvalidOperationException("The DropOutStack<T> is empty.");
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="DropOutStack{T}"/>.
        /// </summary>
        /// <param name="item">The object to push onto the <see cref="DropOutStack{T}"/>. The value can be null for reference types.</param>
        public new void Push(T item)
        {
            //Buffer.BlockCopy(items, 0, items, 1, capacity - 1);

            items[cursor] = item;
            Count++;

            if (Count > capacity)
            {
                Count = capacity;
            }

            cursor = (cursor + 1) % items.Length;
        }

        /// <summary>
        /// Copies the <see cref="DropOutStack{T}"/> to a new array.
        /// </summary>
        /// <returns>A new array containing copies of the elements of the <see cref="DropOutStack{T}"/>.</returns>
        // ReSharper disable once UnusedMember.Global
        public new T[] ToArray()
        {
            var result = new T[Count];

            if (Count == 0)
            {
                return result;
            }

            for (int i = 0; i < Count; i++)
            {
                result[i] = items[(Count - i + cursor - 1) % Count];
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            for (int i = cursor; i < cursor + Count; i++)
            {
                yield return items[i % Count];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            if (++position >= capacity)
            {
                return false;
            }

            current = items[(position + cursor) % capacity];
            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            position = -1;
            current = default;
        }

        /// <summary>
        /// Initializes the <see cref="DropOutStack{T}"/> with a new capacity.
        /// </summary>
        /// <param name="capacityValue">The new capacity value for the <see cref="DropOutStack{T}"/>.</param>
        private void Initialize(int capacityValue)
        {
            items = new T[capacityValue];
            Count = 0;
            capacity = capacityValue;
            position = -1;
            current = default;
            cursor = 0;
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        public T Current => current;

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The current.</value>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Finalizes an instance of the <see cref="DropOutStack{T}"/> class.
        /// </summary>
        ~DropOutStack()
        {
            Dispose(false);
        }
    }
}
