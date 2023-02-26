using System.Collections.Generic;
using System.Threading.Tasks;
using Level.Views;
using UnityEngine;

namespace Level.Services
{
    public class PathfindingService : MonoBehaviour, IPathfindingService
    {
        public async Task<List<INode>> FindPath(IFieldService field)
        {
            //Nodes which are in queue to be calculated
            var openedList = new List<INode>();
            //Nodes which has been already used as a point and calculated costs with its neighbours
            var closedList = new HashSet<INode>();
            
            openedList.Add(field.start);

            while (openedList.Count > 0)
            {
                var currentNode = FindCheapestNode(openedList);
                
                //Remove cheapest node from opened
                openedList.Remove(currentNode);
                closedList.Add(currentNode);

                //If we found a way
                if (currentNode == field.finish) return RetracePath(field.start, field.finish);

                foreach (var neighbourNode in GetNeighbors(currentNode, field.grid))
                {
                    //if not walkable or our previous point just skip them
                    if (!neighbourNode.isWalkable || closedList.Contains(neighbourNode)) continue;

                    int neighbourGCost = currentNode.gCost + GetDistance(currentNode, neighbourNode);

                    /*
                     * The gCost of node could be recalculated when the node is neighbour for several nodes
                     * at once! And cost may differ depends on where it's neighbour located (diagonal or not) 
                     */
                    
                    //if it is the first time we touch a node or it's gCost less than currently calculated
                    if (openedList.Contains(neighbourNode) && neighbourGCost >= neighbourNode.gCost) continue;
                    
                    neighbourNode.gCost = neighbourGCost;
                    neighbourNode.hCost = GetDistance(neighbourNode, field.finish);
                    neighbourNode.parent = currentNode;

                    if (!openedList.Contains(neighbourNode))
                        openedList.Add(neighbourNode);

                    await Task.Delay(2000);
                }
            }

            return null;
        }

        private INode FindCheapestNode(IReadOnlyList<INode> nodes)
        {
            var bestNode = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                //First of all check final cost. If it's the
                //same lets check heuristic to find the best route
                if (nodes[i].fCost < bestNode.fCost||
                    nodes[i].fCost == bestNode.fCost && 
                    nodes[i].hCost < bestNode.hCost)
                {
                    bestNode = nodes[i];
                }
            }

            return bestNode;
        }
        
        private static List<INode> RetracePath(INode startNode, INode endNode)
        {
            var path = new List<INode>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }
    
        private static int GetDistance(INode nodeA, INode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.coords.x - nodeB.coords.x);
            int dstY = Mathf.Abs(nodeA.coords.y - nodeB.coords.y);

            if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    
        private static List<INode> GetNeighbors(INode node, INode[,] grid)
        {
            var neighbors = new List<INode>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = node.coords.x + x;
                    int checkY = node.coords.y + y;
                    
                    if (checkX >= 0 && checkX < grid.GetLength(0) &&
                        checkY >= 0 && checkY < grid.GetLength(1))
                    {
                        neighbors.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }
    }
}
