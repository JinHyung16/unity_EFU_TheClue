using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughPathFinding
{
    public class AstarGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius; //0.5f 추천
        [SerializeField] private float distanceBetweenNodes; //0.1f 추천

        private Node[,] nodeArray; //2차원 배열
        [HideInInspector] public List<Node> finalPath;

        private float nodeDiameter; //node의 지름
        private int gridSizeX;
        private int gridSizeY;

        public int MaxSize
        {
            get
            {
                return (this.gridSizeX * this.gridSizeY);
            }
        }
        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            DrawGrid();
        }

        private void DrawGrid()
        {
            nodeArray = new Node[gridSizeX, gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkAble = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleLayerMask);
                    nodeArray[x, y] = new Node(x, y, walkAble, worldPoint);
                }
            }
        }

        public Node NodeFrowmWorldPosition(Vector3 worldPosition)
        {
            float xPosition = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float yPosition = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; //wordPosition의 z값이 2차원에선 y값이다.
            xPosition = Mathf.Clamp01(xPosition);
            yPosition = Mathf.Clamp01(yPosition);

            int x = Mathf.RoundToInt((gridSizeX - 1) * xPosition);
            int y = Mathf.RoundToInt((gridSizeY - 1) * yPosition);

            return nodeArray[x, y];
        }

        public List<Node> GetNeighborhoodNodes(Node node)
        {
            List<Node> neighborhoodNodeList = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (0 <= checkX && checkX < gridSizeX && 0 <= checkY && checkY < gridSizeY)
                    {
                        neighborhoodNodeList.Add(nodeArray[checkX, checkY]);
                    }
                }
            }
            return neighborhoodNodeList;
        }     
    }
}