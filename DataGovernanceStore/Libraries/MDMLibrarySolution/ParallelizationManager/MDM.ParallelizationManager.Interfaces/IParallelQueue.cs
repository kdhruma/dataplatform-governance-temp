using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MDM.ParallelizationManager.Interfaces
{
    using MDM.BusinessObjects;

    public interface IParallelQueue : IProducerConsumerCollection<MDMMessagePackage>
    {
        // Summary:
        //     Gets the bounded capacity of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance.
        //
        // Returns:
        //     The bounded capacity of this collection, or int.MaxValue if no bound was
        //     supplied.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        int BoundedCapacity { get; }
      
        // Summary:
        //     Gets the number of items contained in the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Returns:
        //     The number of items contained in the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        new int Count { get; }
        
        // Summary:
        //     Gets whether this System.Collections.Concurrent.BlockingCollection<T> has
        //     been marked as complete for adding.
        //
        // Returns:
        //     Whether this collection has been marked as complete for adding.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        bool IsAddingCompleted { get; }
        
        // Summary:
        //     Gets whether this System.Collections.Concurrent.BlockingCollection<T> has
        //     been marked as complete for adding and is empty.
        //
        // Returns:
        //     Whether this collection has been marked as complete for adding and is empty.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        bool IsCompleted { get; }
       
        // Summary:
        //     Adds the item to the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection. The value can be a null reference.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        void Add(MDMMessagePackage item);
        
        // Summary:
        //     Adds the item to the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection. The value can be a null reference.
        //
        //   cancellationToken:
        //     A cancellation token to observe.
        //
        // Exceptions:
        //   System.OperationCanceledException:
        //     If the System.Threading.CancellationToken is canceled.
        //
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed
        //     or the System.Threading.CancellationTokenSource that owns cancellationToken
        //     has been disposed.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        void Add(MDMMessagePackage item, CancellationToken cancellationToken);
        
        // Summary:
        //     Marks the System.Collections.Concurrent.BlockingCollection<T> instances as
        //     not accepting any more additions.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        void CompleteAdding();
        
        // Summary:
        //     Copies all of the items in the System.Collections.Concurrent.BlockingCollection<T>
        //     instance to a compatible one-dimensional array, starting at the specified
        //     index of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional array that is the destination of the elements copied
        //     from the System.Collections.Concurrent.BlockingCollection<T> instance. The
        //     array must have zero-based indexing.
        //
        //   index:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.ArgumentNullException:
        //     The array argument is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The index argument is less than zero.
        //
        //   System.ArgumentException:
        //     The index argument is equal to or greater than the length of the array.The
        //     destination array is too small to hold all of the BlockingCcollection elements.The
        //     array rank doesn't match.The array type is incompatible with the type of
        //     the BlockingCollection elements.
        new void CopyTo(MDMMessagePackage[] array, int index);
        
        // Summary:
        //     Releases all resources used by the current instance of the System.Collections.Concurrent.BlockingCollection<T>
        //     class.
        void Dispose();
        
        // Summary:
        //     Provides a consuming System.Collections.Generics.IEnumerable`1 for items
        //     in the collection.
        //
        // Returns:
        //     An System.Collections.Generics.IEnumerable`1 that removes and returns items
        //     from the collection.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        IEnumerable<MDMMessagePackage> GetConsumingEnumerable();
        
        // Summary:
        //     Provides a consuming System.Collections.Generics.IEnumerable`1 for items
        //     in the collection.
        //
        // Parameters:
        //   cancellationToken:
        //     A cancellation token to observe.
        //
        // Returns:
        //     An System.Collections.Generics.IEnumerable`1 that removes and returns items
        //     from the collection.
        //
        // Exceptions:
        //   System.OperationCanceledException:
        //     If the System.Threading.CancellationToken is canceled.
        //
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed
        //     or the System.Threading.CancellationTokenSource that created cancellationToken
        //     has been disposed
        IEnumerable<MDMMessagePackage> GetConsumingEnumerable(CancellationToken cancellationToken);
        
        // Summary:
        //     Takes an item from the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Returns:
        //     The item removed from the collection.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance, or the System.Collections.Concurrent.BlockingCollection<T> is empty
        //     and the collection has been marked as complete for adding.
        MDMMessagePackage Take();
        
        // Summary:
        //     Takes an item from the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   cancellationToken:
        //     Object that can be used to cancel the take operation.
        //
        // Returns:
        //     The item removed from the collection.
        //
        // Exceptions:
        //   System.OperationCanceledException:
        //     The System.Threading.CancellationToken is canceled.
        //
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed
        //     or the System.Threading.CancellationTokenSource that created the token was
        //     canceled.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance or the BlockingCollection is marked as complete for adding, or the
        //     System.Collections.Concurrent.BlockingCollection<T> is empty.
        MDMMessagePackage Take(CancellationToken cancellationToken);
        
        // Summary:
        //     Copies the items from the System.Collections.Concurrent.BlockingCollection<T>
        //     instance into a new array.
        //
        // Returns:
        //     An array containing copies of the elements of the collection.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        new MDMMessagePackage[] ToArray();
        
        // Summary:
        //     Attempts to add the specified item to the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection.
        //
        // Returns:
        //     true if item could be added; otherwise false. If the item is a duplicate,
        //     and the underlying collection does not accept duplicate items, then an System.InvalidOperationException
        //     is thrown.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        new bool TryAdd(MDMMessagePackage item);
        
        // Summary:
        //     Attempts to add the specified item to the System.Collections.Concurrent.BlockingCollection<T>
        //     within the specified time period.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite
        //     (-1) to wait indefinitely.
        //
        // Returns:
        //     true if the item could be added to the collection within the specified time;
        //     otherwise, false. If the item is a duplicate, and the underlying collection
        //     does not accept duplicate items, then an System.InvalidOperationException
        //     is thrown.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents
        //     an infinite time-out.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        bool TryAdd(MDMMessagePackage item, int millisecondsTimeout);
        
        // Summary:
        //     Attempts to add the specified item to the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection.
        //
        //   timeout:
        //     A System.TimeSpan that represents the number of milliseconds to wait, or
        //     a System.TimeSpan that represents -1 milliseconds to wait indefinitely.
        //
        // Returns:
        //     true if the item could be added to the collection within the specified time
        //     span; otherwise, false.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     timeout is a negative number other than -1 milliseconds, which represents
        //     an infinite time-out -or- timeout is greater than System.Int32.MaxValue.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        bool TryAdd(MDMMessagePackage item, TimeSpan timeout);
        
        // Summary:
        //     Attempts to add the specified item to the System.Collections.Concurrent.BlockingCollection<T>
        //     within the specified time period, while observing a cancellation token.
        //
        // Parameters:
        //   item:
        //     The item to be added to the collection.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite
        //     (-1) to wait indefinitely.
        //
        //   cancellationToken:
        //     A cancellation token to observe.
        //
        // Returns:
        //     true if the item could be added to the collection within the specified time;
        //     otherwise, false. If the item is a duplicate, and the underlying collection
        //     does not accept duplicate items, then an System.InvalidOperationException
        //     is thrown.
        //
        // Exceptions:
        //   System.OperationCanceledException:
        //     If the System.Threading.CancellationToken is canceled.
        //
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed
        //     or the underlying System.Threading.CancellationTokenSource has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents
        //     an infinite time-out.
        //
        //   System.InvalidOperationException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been marked as
        //     complete with regards to additions.-or-The underlying collection didn't accept
        //     the item.
        bool TryAdd(MDMMessagePackage item, int millisecondsTimeout, CancellationToken cancellationToken);
        
        // Summary:
        //     Attempts to remove an item from the System.Collections.Concurrent.BlockingCollection<T>.
        //
        // Parameters:
        //   item:
        //     The item removed from the collection.
        //
        // Returns:
        //     true if an item could be removed.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance.
        new bool TryTake(out MDMMessagePackage item);
        
        // Summary:
        //     Attempts to remove an item from the System.Collections.Concurrent.BlockingCollection<T>
        //     in the specified time period.
        //
        // Parameters:
        //   item:
        //     The item removed from the collection.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite
        //     (-1) to wait indefinitely.
        //
        // Returns:
        //     true if an item could be removed from the collection within the specified
        //     time; otherwise, false.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents
        //     an infinite time-out.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance.
        bool TryTake(out MDMMessagePackage item, int millisecondsTimeout);
        
        // Summary:
        //     Attempts to remove an item from the System.Collections.Concurrent.BlockingCollection<T>
        //     in the specified time period.
        //
        // Parameters:
        //   item:
        //     The item removed from the collection.
        //
        //   timeout:
        //     A System.TimeSpan that represents the number of milliseconds to wait, or
        //     a System.TimeSpan that represents -1 milliseconds to wait indefinitely.
        //
        // Returns:
        //     true if an item could be removed from the collection within the specified
        //     time; otherwise, false.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     timeout is a negative number other than -1 milliseconds, which represents
        //     an infinite time-out -or- timeout is greater than System.Int32.MaxValue.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance.
        bool TryTake(out MDMMessagePackage item, TimeSpan timeout);
        
        // Summary:
        //     Attempts to remove an item from the System.Collections.Concurrent.BlockingCollection<T>
        //     in the specified time period while observing a cancellation token.
        //
        // Parameters:
        //   item:
        //     The item removed from the collection.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite
        //     (-1) to wait indefinitely.
        //
        //   cancellationToken:
        //     A cancellation token to observe.
        //
        // Returns:
        //     true if an item could be removed from the collection within the specified
        //     time; otherwise, false.
        //
        // Exceptions:
        //   System.OperationCanceledException:
        //     If the System.Threading.CancellationToken is canceled.
        //
        //   System.ObjectDisposedException:
        //     The System.Collections.Concurrent.BlockingCollection<T> has been disposed
        //     or the underlying System.Threading.CancellationTokenSource has been disposed.
        //
        //   System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents
        //     an infinite time-out.
        //
        //   System.InvalidOperationException:
        //     The underlying collection was modified outside of this System.Collections.Concurrent.BlockingCollection<T>
        //     instance.
        bool TryTake(out MDMMessagePackage item, int millisecondsTimeout, CancellationToken cancellationToken);
    }
}
