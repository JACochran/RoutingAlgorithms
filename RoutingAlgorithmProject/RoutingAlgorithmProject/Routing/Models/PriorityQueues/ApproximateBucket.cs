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
        private int numberOfBuckets = (int)Math.Sqrt(MaxDistance) + 1;
        private Queue<T>[] buckets;
        private int _numNodes;

        public ApproximateBucketQueue()
        {
            buckets = new Queue<T>[numberOfBuckets];
            for (int i = 0; i < numberOfBuckets; i++)
            {
                buckets[i] = new Queue<T>();
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
            buckets[bucketIndex].Enqueue(node);
            node.QueueIndex = bucketIndex;
            node.InQueue = true;
        }

        public T Dequeue()
        {
            foreach (var q in buckets)
            {
                if (q.Count > 0)
                {
                    //_numNodes--;
                    //T node = (T)q.Dequeue();
                    T node = null;
                    float min = float.MaxValue;
                    foreach(var n in q)
                    {
                        if (n.Priority < min)
                        {
                            node = n;
                            min = n.Priority;
                        }
                    }
                    Remove(node);
                    return node;
                }
            }
            return null;
        }

        public bool Contains(T node)
        {
            return node.InQueue;
            //bool result = false;
            //int index = 0;
            //foreach(var b in buckets)
            //{
            //    if (b.Contains(node))
            //    {
            //        result = true;
            //        break;
            //    }
            //    index++;
            //}
            //if((index!=node.QueueIndex && index!=buckets.Length) || result != node.inQueue)
            //{
            //    var lkj = 1;
            //}
            //return result;
        }

        public void UpdatePriority(T node, float priority)
        {
            Remove(node);
            node.Priority = priority;
            Enqueue(node, priority);
        }

        private void Remove(T node)
        {
            List<T> list = new List<T>();
            T temp = buckets[node.QueueIndex].Dequeue();
            while (temp != node)
            {
                list.Add(temp);
                temp = buckets[node.QueueIndex].Dequeue();
            }
            // readdd nodes
            for (int i = list.Count - 1; i >= 0; i--)
            {
                buckets[node.QueueIndex].Enqueue(list[i]);
            }
            node.InQueue = false;
            //buckets[node.QueueIndex].Remove(node);
            _numNodes--;
        }

        //private int findNode(T node)
        //{
        //    for(int i = 0;i<buckets.Length;i++)
        //    {
        //        if (buckets[i].findNode(node))
        //            return i;
        //    }
        //    return -1;
        //}


    }
}
