using RoutingAlgorithmProject.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Routing.Models.PriorityQueues
{

    public class ApproximateBucketQueue<T> where T: Vertex
    {
        public static int MaxDistance = 1000000;
        private int numberOfBuckets = (int)Math.Sqrt(MaxDistance) +1;
        private FIFOQueue[] buckets;
        private int _numNodes;

        public ApproximateBucketQueue()
        {
            buckets = new FIFOQueue[numberOfBuckets];
            for(int i = 0; i < numberOfBuckets; i++)
            {
                buckets[i] = new FIFOQueue();
            }
            _numNodes = 0;
        }

        private int GetBucketIndex(float priority)
        {
            return (int)(priority / numberOfBuckets);
        }

        public int Count
        {
            get
            {
                return _numNodes;
            }
        }

        public void Enqueue(T node, float priority)
        {
            node.Priority = priority;
            _numNodes++;
            var bucketIndex = GetBucketIndex(priority);
            buckets[bucketIndex].Enqueue((Vertex)node);
            node.QueueIndex = bucketIndex;
            node.inQueue = true;
        }

        public T Dequeue()
        {
            foreach(var q in buckets)
            {
                if (q.Count > 0)
                {
                    _numNodes--;
                    return (T)q.Dequeue();
                }
            }
            return null;
        }

        public bool Contains(T node)
        {
            return node.inQueue;
        }

        public void UpdatePriority(T node, float priority)
        {
            Remove(node);
            node.Priority = priority;
            Enqueue(node, priority);
        }

        private void Remove(T node)
        {
            if(findNode(node)!=node.QueueIndex)
            {
                var lk = 1;
            }
            buckets[node.QueueIndex].Remove(node);
            _numNodes--;
        }

        private int findNode(T node)
        {
            for(int i = 0;i<buckets.Length;i++)
            {
                if (buckets[i].findNode(node))
                    return i;
            }
            return -1;
        }

        
    }
}
