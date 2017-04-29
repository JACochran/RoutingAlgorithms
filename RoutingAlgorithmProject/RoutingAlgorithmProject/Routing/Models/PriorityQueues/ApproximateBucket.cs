using RoutingAlgorithmProject.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Routing.Models.PriorityQueues
{

    public class ApproximateBucketQueue<T> where T : Vertex
    {
        public static int MaxDistance = 1000000;
        private static int numberOfBuckets = 32768;// (int)Math.Sqrt(MaxDistance) + 1;
        private static int distPerBucket = (int) MaxDistance / numberOfBuckets;
        private FIFOQueue[] buckets;
        private int _numNodes;
        private int minBucketIndex;

        public ApproximateBucketQueue()
        {
            buckets = new FIFOQueue[numberOfBuckets+1];
            for (int i = 0; i < numberOfBuckets+1; i++)
            {
                buckets[i] = new FIFOQueue();
            }
            _numNodes = 0;
            minBucketIndex = numberOfBuckets;
        }

        private int GetBucketIndex(float priority)
        {
            if (priority == float.MaxValue)
            {
                return numberOfBuckets;
            }
            int index = (int)priority / distPerBucket;
            return Math.Min(index, numberOfBuckets - 1);
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
            try
            {
                node.Priority = priority;
                _numNodes++;
                var bucketIndex = GetBucketIndex(priority);
                if(bucketIndex < minBucketIndex)
                {
                    minBucketIndex = bucketIndex;
                }
                buckets[bucketIndex].Enqueue(node);
                node.QueueIndex = bucketIndex;
            }catch (Exception ex)
            {
                var lkj = 1;
            }
        }

        public T Dequeue()
        {
            //foreach (var q in buckets)
            //{
            //    if (q.Count > 0)
            //    {
            _numNodes--;
            T node = (T)buckets[minBucketIndex].Dequeue();
            for (; buckets[minBucketIndex].Count == 0 && (minBucketIndex < numberOfBuckets + 1); ++minBucketIndex) ;
            
            return node;
            //    }
            //}
            //return null;
        }

        public bool Contains(T node)
        {
            return node.InQueue;
        }

        public void UpdatePriority(T node, float priority)
        {
            int oldIndex = GetBucketIndex(node.Priority);
            int newIndex = GetBucketIndex(priority);
            if (oldIndex != newIndex)
            {
                Remove(node);
                node.Priority = priority;
                Enqueue(node, priority);
            }
            else
            {
                node.Priority = priority;
            }
        }

        private void Remove(T node)
        {
            buckets[node.QueueIndex].Remove(node);
            _numNodes--;
        }

    }
}
