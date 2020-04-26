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
        ScaleGrid();
    }

    public void ScaleGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(waypoint, transform.position + new Vector3(i, 0, j) * 10, Quaternion.identity, map);
            }
        }

        Camera.main.transform.position += new Vector3(width/2, height, 1) * 10;
    }

}
