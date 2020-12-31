using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP77Tools.UI.Functionality
{

    public class TaskQueue
    {
        
    }


    public sealed class ItemDequeuedEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }

    public sealed class EventfulConcurrentQueue<T>
    {
        private ConcurrentQueue<T> _queue;

        public EventfulConcurrentQueue()
        {
            _queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            OnItemEnqueued();
        }

        public bool TryDequeue(out T result)
        {
            var success = _queue.TryDequeue(out result);

            if (success)
            {
                OnItemDequeued(result);
            }
            return success;
        }

        public event EventHandler ItemEnqueued;
        public event EventHandler<ItemDequeuedEventArgs<T>> ItemDequeued;

        void OnItemEnqueued()
        {
            ItemEnqueued?.Invoke(this, EventArgs.Empty);
        }

        void OnItemDequeued(T item)
        {
            ItemDequeued?.Invoke(this, new ItemDequeuedEventArgs<T> { Item = item });
        }


    }

}
