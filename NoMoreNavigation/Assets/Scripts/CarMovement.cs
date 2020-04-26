using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    private List<Waypoint> path;
    // Start is called before the first frame update
    void Start()
    {
        PathFinder pathFinder = FindObjectOfType<PathFinder>();
        path = pathFinder.GetPath();
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        print("Starting Patrol...");
        foreach (Waypoint waypoint in path)
        {
            print("Visiting: " + waypoint.name);
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        print("Ending Patrol...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
