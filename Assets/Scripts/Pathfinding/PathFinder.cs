using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinate;
    public Vector2Int StartCoordinate { get => startCoordinate; set => startCoordinate = value; }
    [SerializeField] Vector2Int destinationCoordinate;
    public Vector2Int DestinationCoordinate { get => destinationCoordinate; set => destinationCoordinate = value; }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;


    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            startNode = gridManager.Grid[StartCoordinate];
            destinationNode = gridManager.Grid[DestinationCoordinate];
        }
    }
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinate);
    }
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNode();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoords = currentSearchNode.coordinates + direction;

            if (gridManager.GetNode(neighbourCoords) != null)
            {
                neighbours.Add(gridManager.GetNode(neighbourCoords));
            }
        }

        foreach (Node neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo = currentSearchNode;
                reached.Add(neighbour.coordinates, neighbour);
                frontier.Enqueue(neighbour);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(gridManager.Grid[coordinates]);
        reached.Add(coordinates, gridManager.Grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();

            if (currentSearchNode.coordinates == DestinationCoordinate)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();

        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;

        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (gridManager.Grid.ContainsKey(coordinates))
        {
            bool previousState = gridManager.Grid[coordinates].isWalkable;
            gridManager.Grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            gridManager.Grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    public void NotifyRecievers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
