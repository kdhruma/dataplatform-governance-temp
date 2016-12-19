using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Core
{
    /// <summary>
    /// Custom implementation of Stack data structure (LIFO) with needed utility methods
    /// </summary>
    /// <typeparam name="T">Template for Type</typeparam>
    public class RSStack<T> : IEnumerable<T>
    {
        #region Local Variables

        List<T> list = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default contsructor for RSStack
        /// </summary>
        public RSStack()
        {
            list = new List<T>();
        }

        /// <summary>
        /// Constructor accepting predefined stack capacity
        /// </summary>
        /// <param name="capacity">Item capacity for Stack</param>
        public RSStack(Int32 capacity)
        {
            list = new List<T>(capacity);
            Stack<Int32> test = new Stack<int>(3);

        }

        #endregion

        #region Public Propertes

        /// <summary>
        /// Number of stack items
        /// </summary>
        public Int32 Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Indicates whether the stack is empty or not
        /// </summary>
        public Boolean IsEmpty
        {
            get
            {
                return (Count == 0);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reverse the stack
        /// </summary>
        public void Reverse()
        {
            list.Reverse();

        }

        /// <summary>
        /// Swaps two stack items
        /// </summary>
        public void Swap()
        {
            if (this.Count < 2)
            {
                return;
            }

            T Item1 = Pop();
            T Item2 = Pop();

            Push(Item1);
            Push(Item2);
        }


        /// <summary>
        /// Push a single item on the stack
        /// </summary>
        /// <param name="item">Item of type T to be pushed in stack</param>
        public void Push(T item)
        {
            list.Add(item);
        }


        /// <summary>
        /// Push an array of items on the stack
        /// </summary>
        /// <param name="items">Items of type T to be pushed in stack</param>
        public void Push(T[] items)
        {
            for (Int32 i = 0; i < items.Length; i++)
            {
                Push(items[i]);
            }
        }

        /// <summary>
        /// Push the items from a stack onto a stack.  
        /// </summary>
        /// <param name="stack">Source statck</param>
        public void Push(RSStack<T> stack)
        {
            stack.Reverse();

            for (Int32 i = 0; i < stack.Count; i++)
            {
                Push(stack.Pop());
            }
        }

        /// <summary>
        /// Pop single item from stack
        /// </summary>
        /// <returns>Satck Item</returns>
        public T Pop()
        {
            if (Count == 0)
            {
                return default(T);
            }

            T item = list[Count - 1];

            list.RemoveAt(Count - 1);

            return item;
        }

        /// <summary>
        /// Pop multiple item as per the count provided
        /// </summary>
        /// <param name="popCount">Item count</param>
        /// <returns>List of Satck item</returns>
        public T[] Pop(Int32 popCount)
        {
            if (Count == 0)
            {
                return null;
            }

            // adjust the pop count if popping too many
            if (popCount > Count)
            {
                popCount = Count;
            }

            T[] items = new T[popCount];

            for (Int32 i = 0; i < popCount; i++)
            {
                items[i] = Pop();
            }
            return items;
        }

        /// <summary>
        /// Pop all the items from Stack
        /// </summary>
        /// <returns>Aarray of stack item</returns>
        public T[] PopAll()
        {
            return Pop(Count);
        }


        /// <summary>
        /// Pops the items from the stack and places them in their own stack
        /// </summary>
        /// <param name="popCount">Number of Item to pop</param>
        /// <returns>New stack with required pop count</returns>
        public RSStack<T> PopStack(Int32 popCount)
        {
            RSStack<T> stack = new RSStack<T>(popCount);

            for (Int32 i = 0; i < popCount; i++)
            {
                stack.Push(this.Pop());
            }

            stack.Reverse();

            return stack;

        }

        /// <summary>
        /// Retrieves stack item
        /// </summary>
        /// <returns>Single stack item</returns>
        public T Peek()
        {
            if (Count == 0)
            {
                return default(T);
            }

            return list[Count - 1];
        }

        /// <summary>
        /// Peeks item from stacj as per the count provided
        /// </summary>
        /// <param name="peekCount">Total number of item to peek</param>
        /// <returns>Array of stack item</returns>
        public T[] Peek(Int32 peekCount)
        {
            if (Count == 0)
            {
                return null;
            }

            // adjust the pop count if peeking too many
            if (peekCount > Count)
            {
                peekCount = Count;
            }

            T[] items = new T[peekCount];

            for (Int32 i = 0; i < peekCount; i++)
            {
                items[i] = list[Count - 1 - i];
            }
            return items;
        }

        /// <summary>
        /// Peek all item from Stack
        /// </summary>
        /// <returns>Array of stack item</returns>
        public T[] PeekAll()
        {
            return Peek(Count);
        }

        /// <summary>
        /// Search the stack and find a matching T.  
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>This method returns an integer that indicates the number of Pop()'s required
        /// to retrieve the found item.  If the item is not found, it returns 0</returns>
        public Int32 Search(T item)
        {
            Int32 numPops = 0;
            Boolean found = false;

            for (Int32 i = 0; i < this.Count; i++)
            {
                numPops++;
                if (list[Count - 1 - i].Equals(item) == true)
                {
                    found = true;
                    break;
                }
            }

            if (found == true)
            {
                return numPops;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Remove the item from the stack.  For example, to remove the "oldest" (or first item) use Stack.RemoveItem(0);
        /// </summary>
        /// <param name="index">Index for the item to remove</param>
        public void RemoveItem(Int32 index)
        {
            if (index < 0 || index >= Count)
            {
                return;
            }

            this.list.RemoveAt(index);
        }


        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns>Exception</returns>
        public IEnumerator<T> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}