using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScaler : MonoBehaviour
{

    public int width;
    public int height;
    public Transform map;
    public Waypoint waypoint;

    private void Start()
    {
        ScaleGrid(width,height);
    }

    public void ScaleGrid(int width, int height)
    {
        DeleteWaypoints();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(waypoint, transform.position + new Vector3(i, 0, j) * 10, Quaternion.identity, map);
            }
        }

        Camera.main.transform.position = new Vector3(width/2.2f, height, 1) * 10;
    }

    private void DeleteWaypoints()
    {
        foreach(Transform waypoint in map)
        {
            Destroy(waypoint.gameObject);
        }
    }
}
