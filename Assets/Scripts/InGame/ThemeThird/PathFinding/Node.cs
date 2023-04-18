using UnityEngine;

namespace HughPathFinding
{
    public class Node : IHeapItem<Node>
    {
        public int gridX;
        public int gridY;

        public bool isWalkable;
        public Vector3 nodePosition;

        public int moveCost; //다음 노드로 이동하는 비용, gCost의미
        public int disCost; //현재 노드에서 도착지점까지의 거리, hCost의미
        public int totalCost { get { return (this.moveCost + this.disCost); } } //fCost의미
        public Node Parent;

        private int heapIndex;
        public int HeapIndex 
        { 
            get 
            { 
                return this.heapIndex; 
            }
            set 
            { 
                this.heapIndex = value; 
            } 
        }

        public Node(int gX, int gY, bool walkAble, Vector3 position)
        {
            this.gridX = gX;
            this.gridY = gY;
            this.isWalkable = walkAble;
            this.nodePosition = position;
        }

        public int CompareTo(Node other)
        {
            int result = totalCost.CompareTo(other.totalCost);
            if (result == 0)
            {
                result = disCost.CompareTo(other.disCost);
            }
            return -result;
        }
    }
}
