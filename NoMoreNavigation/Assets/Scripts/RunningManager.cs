using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningManager : MonoBehaviour
{

    public CarMovement carPrefab;
    private CarMovement car;
    public Waypoint startWaypoint;
    public Waypoint finishWaypoint;
    private List<Waypoint> path;

    public bool isCompareHeuristics;

    private bool onRun;
    private bool onNextStep;
    private Coroutine stepCoroutine;
    private Coroutine runCoroutine;

    public event Action onResetMap;
    public event Action<int> onNextHeuristic;
    private int heuristicInd;
    private int pathFindAlgorithm;
    private PathFinder pathFinder;

    public void SetStartFinish(Waypoint startWaypoint, Waypoint finishWaypoint,int pathFindAlgorithm, int heuristic)
    {
        if(isCompareHeuristics && onNextHeuristic != null)
        {
            onNextHeuristic(heuristicInd);
        }
        this.startWaypoint = startWaypoint;
        this.finishWaypoint = finishWaypoint;
        this.pathFindAlgorithm = pathFindAlgorithm;
        pathFinder = FindObjectOfType<PathFinder>();
        if(isCompareHeuristics)
            path = pathFinder.GetPath(startWaypoint, finishWaypoint, pathFindAlgorithm, heuristicInd);
        else
            path = pathFinder.GetPath(startWaypoint, finishWaypoint, pathFindAlgorithm, heuristic);
    }

    public void MakeFaster()
    {
        car.GoFaster(2);
    }

    public void MakeSlower()
    {
        car.GoSlower(2);
    }

    public void NextStep()
    {
        if (car == null)
        {
            car = Instantiate(carPrefab, startWaypoint.transform.position, Quaternion.identity);
            car.onFinishedPath += OnFinishedPath;
            car.nextStep = true;
            stepCoroutine = StartCoroutine(car.FollowPathStep(path));
        }
        else
        {
            car.nextStep = true;
        }
        onNextStep = true;

    }

    public void Run()
    {
        if (car == null)
        {
            car = Instantiate(carPrefab, startWaypoint.transform.position, Quaternion.identity);
            car.onFinishedPath += OnFinishedPath;
        }
        if (onNextStep)
        {
            onNextStep = false;
            StopCoroutine(stepCoroutine);
        }
        runCoroutine = StartCoroutine(car.FollowPath(path));
        onRun = true;
    }

    private void OnFinishedPath()
    {
        if (onNextStep)
        {
            car = null;
            StopCoroutine(stepCoroutine);
        }
        if (onRun)
        {
            car = null;
            StopCoroutine(runCoroutine);
        }

        if (!isCompareHeuristics)
        {
            ResetMap();
        }
        else
        {
            heuristicInd++;
            if (heuristicInd == Enum.GetNames(typeof(Heuristic)).Length)
            {
                ResetMap();
            }
            else
            {
                if(onNextHeuristic != null)
                {
                    onNextHeuristic(heuristicInd);
                }
                path = pathFinder.GetPath(startWaypoint, finishWaypoint, pathFindAlgorithm, heuristicInd);
                if (onRun)
                    Run();
                else if (onNextStep)
                    NextStep();
            }

        }
    }

    private void ResetMap()
    {
        if(onResetMap != null)
        {
            onResetMap();
        }
    }
}
