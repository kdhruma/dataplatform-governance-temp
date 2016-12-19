using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Core
{
    /// <summary>
    /// Enumerator implementation of RSQueue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RSQueueEnumerator<T> : IEnumerator<T>
    {

        #region Local Variables

        private List<T> items = null;

        private Int32 location;

        #endregion

        #region Constructor

        /// <summary>
        /// Queue enumerator contructor
        /// </summary>
        /// <param name="items">Items to be used for enumeration</param>
        public RSQueueEnumerator(List<T> items)
        {
            this.items = items;
            location = -1;
        }

        #endregion

        #region IEnumerator<T> Members

        /// <summary>
        /// Provides current item from list/queue
        /// </summary>
        public T Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return items[location];
                }
                else
                {
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose method implementing IDisposable interface
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Provides current enumeration item
        /// </summary>
        Object IEnumerator.Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return (Object)items[location];
                }
                else
                {
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        /// <summary>
        /// Moves iteration to next index
        /// </summary>
        /// <returns>Indicates whether move operation suceeded or not</returns>
        public Boolean MoveNext()
        {
            location++;
            return (location < items.Count);
        }

        /// <summary>
        /// Resets indexer to default location
        /// </summary>
        public void Reset()
        {
            location = -1;
        }

        #endregion
    }
}
