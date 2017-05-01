using RoutingAlgorithmProject.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Routing.Models.PriorityQueues
{
    class FIFOQueue
    {
        private Vertex entry;
        private int size;

        public FIFOQueue()
        {
            size = 0;
        }

        public void Enqueue(Vertex node)
        {
            if (entry == null)
            {
                entry = node;
                entry.FIFOnext = entry;
                entry.FIFOprev = entry;
            }
            else
            {
                entry.FIFOprev.FIFOnext = node;
                node.FIFOprev = entry.FIFOprev;
                node.FIFOnext = entry;
                entry.FIFOprev = node;
            }
            node.InQueue = true;
            size++;
        }

        public Vertex Dequeue()
        {
            Vertex node = entry;
            if (node.FIFOnext != node && node.Priority != float.MaxValue)
            {
                // find min node
                float min = entry.Priority;
                var curr = entry.FIFOnext;
                while (curr != entry)
                {
                    if (curr.Priority < min)
                    {
                        min = curr.Priority;
                        node = curr;
                    }
                    curr = curr.FIFOnext;
                }
            }
            Remove(node);
            return node;
        }

        public void Remove(Vertex v)
        {
            v.InQueue = false;
            v.QueueIndex = -1;
            if (v.FIFOnext == v)
            {
                entry = null;
            }
            else
            {
                v.FIFOprev.FIFOnext = v.FIFOnext;
                v.FIFOnext.FIFOprev = v.FIFOprev;
                if (entry == v)
                {
                    entry = v.FIFOnext;
                }
            }
            size--;
            v.FIFOnext = null;
            v.FIFOprev = null;
        }

        internal bool findNode<T>(T node) where T : Vertex
        {
            if (entry == null)
                return false;
            if (entry == node)
                return true;

            if (entry.FIFOnext == entry || entry.FIFOprev == entry)
            {
                return false;
            }

            var curr = entry.FIFOnext;

            while (curr != entry)
            {
                if (curr == node)
                    return true;
                curr = curr.FIFOnext;
            }
            return false;
        }

        public int Count
        {
            get
            {
                return this.size;
            }
        }

        private bool IsValid()
        {
            if (checkQueueState())
                return true;
            else
                return false;
        }

        private bool checkQueueState()
        {

            if (entry == null)
                return size == 0;

            if (entry.FIFOnext == entry || entry.FIFOprev == entry)
            {
                return size == 1;
            }

            var curr = entry.FIFOnext;
            int counter = 1;

            while (curr != entry)
            {
                counter++;
                curr = curr.FIFOnext;
            }
            return counter == Count;
        }
    }


}
