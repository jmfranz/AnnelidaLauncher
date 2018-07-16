using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnelidaLauncher.Model
{
    public class CircularBuffer<T>: Queue<T>
    {
        private int capacity;

        public CircularBuffer(int capacity) : base(capacity)
        {
            this.capacity = capacity;
        }


        public new void Enqueue(T item)
        {
            if (Count == capacity)
            {
                base.Dequeue();
                base.Enqueue(item);
            }
            else
                base.Enqueue(item);
        }
    }
}
