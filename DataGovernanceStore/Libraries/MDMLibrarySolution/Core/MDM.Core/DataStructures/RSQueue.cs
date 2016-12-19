using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Core
{
    /// <summary>
    /// Custom implementation of Queue data structure (FIFO) with needed utility methods
    /// </summary>
    /// <typeparam name="T">Template for Type</typeparam>
    public class RSQueue<T> : IEnumerable<T>
    {

        #region Local Varibles

        private List<T> queue = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default contsructor for RSQueue
        /// </summary>
        public RSQueue()
        {
            queue = new List<T>();
        }

        /// <summary>
        /// Constructor accepting predefined Queue capacity
        /// </summary>
        /// <param name="capacity">Item capacity for Queue</param>
        public RSQueue(Int32 capacity)
        {
            queue = new List<T>(capacity);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to end of the Queue
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            queue.Add(item);
        }

        /// <summary>
        /// Clears queue
        /// </summary>
        public void Clear()
        {
            queue.Clear();
        }

        /// <summary>
        /// Removes first item from queue and returns the item
        /// </summary>
        /// <returns>Removed item</returns>
        public T Dequeue()
        {
            if (queue.Count == 0)
            {
                return default(T);
            }

            T t = queue[0];
            queue.RemoveAt(0);

            return t;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Number of Queue items
        /// </summary>
        public Int32 Count
        {
            get
            {
                return queue.Count;
            }
        }

        /// <summary>
        /// Indicates whether the queue is empty or not
        /// </summary>
        public Boolean IsEmpty
        {
            get
            {
                return (Count == 0);
            }
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Returns queue item based on index
        /// </summary>
        /// <param name="index">Index(location) of Queue item</param>
        /// <returns>Returns queue item of type T</returns>
        public T this[Int32 index]
        {
            get
            {
                return queue[index];
            }
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Provides enumerator to be used for enumation of queue
        /// </summary>
        /// <returns>Enumerator for item of type T</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new RSQueueEnumerator<T>(queue);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new RSQueueEnumerator<T>(queue);
        }

        #endregion
    }

}
