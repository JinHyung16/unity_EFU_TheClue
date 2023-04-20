using UnityEngine;

namespace HughPathFinding
{
    public class Node : IHeapItem<Node>
    {
        public int gridX;
        public int gridY;

        public bool isWalkable;
        public Vector3 nodePosition;

        public int gCost; //시작점에서 현재 노드까지 경로 따라 이동하는데 소요되는 비용
        public int hCost; //현재 노드에서 목적지 노드까지의 예상 이동 비용
        public int FCost { get { return (this.gCost + this.hCost); } } //현재까지 이동하는데 걸린 비용과 예상 비용을 합친 총 비용
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
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return -result;
        }
    }
}
