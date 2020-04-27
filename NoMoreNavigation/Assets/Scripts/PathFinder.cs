using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathFindAlgorithm
{
    BestFirstSearch = 0,
    Astar = 1,
    BreadthFirstSearch = 2,
    DepthFirstSearch =3
}

public enum Heuristic
{
    ManhattanDistance = 0,
    EuclidianDistance = 1
}
public class PathFinder : MonoBehaviour
{
    public PathFindAlgorithm pathFindAlgorithm;
    public Heuristic heuristic;

    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    [SerializeField] Waypoint startWaypoint;
    [SerializeField] Waypoint finishWaypoint;


    public List<Waypoint> GetPath(Waypoint startWaypoint, Waypoint finishWaypoint, int pathFindAlgorithm, int heuristic)
    {
        this.startWaypoint = startWaypoint;
        this.finishWaypoint = finishWaypoint;
        this.pathFindAlgorithm = (PathFindAlgorithm)pathFindAlgorithm;
        this.heuristic = (Heuristic)heuristic;
        LoadGrid();
        //ColorStartAndEnd();

        return PathFind();
    }


    private void LoadGrid()
    {
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            Vector2Int gridPos = waypoint.GetGridPos();
            waypoint.isExplored = false;
            waypoint.UnPaint();

            if (grid.ContainsKey(gridPos))
            {
                print("Skipping Overlapped Block: " + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
        }
    }

    private void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.red);
        finishWaypoint.SetTopColor(Color.green);
    }

    private List<Waypoint> PathFind()
    {
        startWaypoint.isBlocked = false;
        finishWaypoint.isBlocked = false;
        if (pathFindAlgorithm == PathFindAlgorithm.BestFirstSearch)
            return BestFirstSearch();
        else if (pathFindAlgorithm == PathFindAlgorithm.Astar)
            return Astar();
        else if (pathFindAlgorithm == PathFindAlgorithm.BreadthFirstSearch)
            return BreadthFirstSearch();
        else
            return DepthFirstSearch();



    }

    private List<Waypoint> BestFirstSearch()
    {
        MinHeap waypoints = new MinHeap();
        Evaluate(startWaypoint);
        waypoints.Insert(startWaypoint);

        while(waypoints.list.Count > 0)
        {
            Waypoint waypoint = waypoints.PullMin();
            if(waypoint == finishWaypoint)
            {
                return MakePath(waypoint);
            }
            ExploreNeighbours(waypoints, waypoint);
        }
        return null;
    }



    private List<Waypoint> Astar()
    {
        MinHeap waypoints = new MinHeap();
        Evaluate(startWaypoint);
        waypoints.Insert(startWaypoint);

        while (waypoints.list.Count > 0)
        {
            Waypoint waypoint = waypoints.PullMin();
            if (waypoint == finishWaypoint)
            {
                return MakePath(waypoint);
            }
            ExploreNeighbours(waypoints, waypoint);
        }
        return null;
    }

    private List<Waypoint> BreadthFirstSearch()
    {
        Queue<Waypoint> waypoints = new Queue<Waypoint>();
        waypoints.Enqueue(startWaypoint);
        startWaypoint.isExplored = true;
        while (waypoints.Count > 0)
        {
            Waypoint waypoint = waypoints.Dequeue();
            if (waypoint == finishWaypoint)
            {
                return MakePath(waypoint);
            }
            ExploreNeighbours(waypoints, waypoint);
        }
        return null;
    }

    private List<Waypoint> DepthFirstSearch()
    {
        Stack<Waypoint> waypoints = new Stack<Waypoint>();
        waypoints.Push(startWaypoint);
        startWaypoint.isExplored = true;
        while (waypoints.Count > 0)
        {
            Waypoint waypoint = waypoints.Pop();
            if (waypoint == finishWaypoint)
            {
                return MakePath(waypoint);
            }
            ExploreNeighbours(waypoints, waypoint);
        }
        return null;
    }


    private void ExploreNeighbours(Queue<Waypoint> waypoints, Waypoint waypoint)
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int index = waypoint.GetGridPos() + direction;
            if (grid.ContainsKey(index))
            {
                Waypoint neighbour = grid[index];
                if (neighbour.CanExplore())
                {
                    waypoints.Enqueue(neighbour);
                    neighbour.isExplored = true;
                    neighbour.parent = waypoint;
                }
            }
        }
    }

    private void ExploreNeighbours(Stack<Waypoint> waypoints, Waypoint waypoint)
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int index = waypoint.GetGridPos() + direction;
            if (grid.ContainsKey(index))
            {
                Waypoint neighbour = grid[index];
                if (neighbour.CanExplore())
                {
                    waypoints.Push(neighbour);
                    neighbour.isExplored = true;
                    neighbour.parent = waypoint;
                }
            }
        }
    }

    private void ExploreNeighbours(MinHeap waypoints, Waypoint waypoint)
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int index = waypoint.GetGridPos() + direction;
            if (grid.ContainsKey(index))
            {
                Waypoint neighbour = grid[index];
                if (neighbour.CanExplore())
                {
                    neighbour.parent = waypoint;
                    Evaluate(neighbour);
                    if(pathFindAlgorithm == PathFindAlgorithm.Astar)
                        GiveWeight(neighbour);
                    waypoints.Insert(neighbour);
                    neighbour.isExplored = true;
                }
            }
        }
    }

    private void Evaluate(Waypoint waypoint)
    {
        if (heuristic == Heuristic.ManhattanDistance)
            waypoint.h = AbsSum(finishWaypoint.GetGridPos() - waypoint.GetGridPos());
        else if (heuristic == Heuristic.EuclidianDistance)
            waypoint.h = Vector2Int.Distance(finishWaypoint.GetGridPos(), waypoint.GetGridPos());
    }

    private void GiveWeight(Waypoint waypoint)
    {
        waypoint.g = waypoint.parent.g + 1;
    }

    private float AbsSum (Vector2Int vector)
    {
        Vector2Int vectorAbs = new Vector2Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        return vectorAbs.x + vectorAbs.y;
    }

private List<Waypoint> MakePath(Waypoint waypoint)
    {
        List<Waypoint> path = new List<Waypoint>();

        path.Add(finishWaypoint);
        waypoint = waypoint.parent;
        while (waypoint != startWaypoint)
        {
            path.Add(waypoint);
            waypoint.SetTopColor(Color.magenta);
            waypoint = waypoint.parent;
        }
        path.Add(startWaypoint);
        path.Reverse();
        return path;
    }



}
