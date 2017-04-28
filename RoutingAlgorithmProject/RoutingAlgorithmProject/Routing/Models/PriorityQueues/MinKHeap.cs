using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Routing.Models.PriorityQueues
{
    class MinKHeap<T> where T : PriorityQueueNode
    {
        private int _numNodes;
        private T[] _nodes;

        public MinKHeap(int size)
        {
            _numNodes = 0;
            _nodes = new T[size + 1];
        }

        public int Count
        {
            get
            {
                return _numNodes;
            }
        }

        public bool Contains(T node)
        {
            return (_nodes[node.QueueIndex] == node);
        }

        public void Enqueue(T node, float priority)
        {
            node.Priority = priority;
            _numNodes++;
            _nodes[_numNodes] = node;
            node.QueueIndex = _numNodes;
            IncreaseKey(_nodes[_numNodes]);
        }

        private void Swap(T node1, T node2)
        {
            //Swap the nodes
            _nodes[node1.QueueIndex] = node2;
            _nodes[node2.QueueIndex] = node1;

            //Swap their indicies
            int temp = node1.QueueIndex;
            node1.QueueIndex = node2.QueueIndex;
            node2.QueueIndex = temp;
        }

        private void IncreaseKey(T node)
        {
            int parent = node.QueueIndex / 2;
            while (parent >= 1)
            {
                T parentNode = _nodes[parent];
                if (HasHigherPriority(parentNode, node))
                    break;

                //Node has lower priority value, so move it up the heap
                Swap(node, parentNode); 

                parent = node.QueueIndex / 2;
            }
        }

        private void DecreaseKey(T node)
        {
            T newParent;
            int finalQueueIndex = node.QueueIndex;
            while (true)
            {
                newParent = node;
                int childLeftIndex = 2 * finalQueueIndex;

                //Check if the left-child is higher-priority than the current node
                if (childLeftIndex > _numNodes)
                {
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                    break;
                }

                T childLeft = _nodes[childLeftIndex];
                if (HasHigherPriority(childLeft, newParent))
                {
                    newParent = childLeft;
                }

                //Check if the right-child is higher-priority than either the current node or the left child
                int childRightIndex = childLeftIndex + 1;
                if (childRightIndex <= _numNodes)
                {
                    T childRight = _nodes[childRightIndex];
                    if (HasHigherPriority(childRight, newParent))
                    {
                        newParent = childRight;
                    }
                }

                //If either of the children has higher (smaller) priority, swap and continue cascading
                if (newParent != node)
                {
                    _nodes[finalQueueIndex] = newParent;

                    int temp = newParent.QueueIndex;
                    newParent.QueueIndex = finalQueueIndex;
                    finalQueueIndex = temp;
                }
                else
                {
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                    break;
                }
            }
        }

        private bool HasHigherPriority(T higher, T lower)
        {
            return (higher.Priority < lower.Priority);
        }

        /// <summary>
        /// Removes the head of the queue and returns it.
        /// O(log n)
        /// </summary>
        public T Dequeue()
        {
            T result = _nodes[1];
            Remove(result);
            return result;
        }

        /// <summary>
        /// Updates the priority of a node
        /// O(log n)
        /// </summary>
        public void UpdatePriority(T node, float priority)
        {
            node.Priority = priority;
            OnNodeUpdated(node);
        }

        private void OnNodeUpdated(T node)
        {
            int parentIndex = node.QueueIndex / 2;
            T parentNode = _nodes[parentIndex];

            if (parentIndex > 0 && HasHigherPriority(node, parentNode))
            {
                IncreaseKey(node);
            }
            else
            {
                DecreaseKey(node);
            }
        }

        /// <summary>
        /// Removes a node from the queue.  
        /// O(log n)
        /// </summary>
        public void Remove(T node)
        {
            if (node.QueueIndex == _numNodes)
            {
                _nodes[_numNodes] = null;
                _numNodes--;
                return;
            }

            //Swap the node with the last node
            T formerLastNode = _nodes[_numNodes];
            Swap(node, formerLastNode);
            _nodes[_numNodes] = null;
            _numNodes--;

            OnNodeUpdated(formerLastNode);
        }

    }
}
