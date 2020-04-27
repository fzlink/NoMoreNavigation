using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    //private List<Waypoint> path;
    private float waitSeconds = 0.25f;

    public bool nextStep;

    public event Action onFinishedPath;

    //void Start()
    //{
    //    PathFinder pathFinder = FindObjectOfType<PathFinder>();
    //    path = pathFinder.GetPath();
    //}


    public IEnumerator FollowPath(List<Waypoint> path)
    {
        print("Starting Patrol...");
        foreach (Waypoint waypoint in path)
        {
            print("Visiting: " + waypoint.name);
            transform.rotation = Quaternion.LookRotation(waypoint.transform.position - transform.position, Vector3.up);
            transform.position = waypoint.transform.position;
            waypoint.Paint();
            yield return new WaitForSeconds(waitSeconds);
        }
        FinishedPath();
        print("Ending Patrol...");
    }

    public IEnumerator FollowPathStep(List<Waypoint> path)
    {
        print("Starting Patrol...");
        foreach (Waypoint waypoint in path)
        {
            print("Visiting: " + waypoint.name);
            transform.rotation = Quaternion.LookRotation(waypoint.transform.position - transform.position, Vector3.up);
            transform.position = waypoint.transform.position;
            waypoint.Paint();
            yield return new WaitWhile(() => nextStep == false);
            nextStep = false;
        }
        FinishedPath();
        print("Ending Patrol...");
    }

    private void FinishedPath()
    {
        Destroy(gameObject);
        if(onFinishedPath != null)
        {
            onFinishedPath();
        }
    }

    public void GoSlower(float factor)
    {
        waitSeconds *= factor;
    }
    public void GoFaster(float factor)
    {
        waitSeconds /= factor;
    }

}
