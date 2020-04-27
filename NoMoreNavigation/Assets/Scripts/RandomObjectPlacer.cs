using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectPlacer : MonoBehaviour
{
    public Transform map;

    public void PlaceRandomObjects(int buildingNum, int wallNum)
    {
        foreach(Transform waypoint in map)
        {
            waypoint.GetComponent<Waypoint>().DeletePlacement();
        }

        int waypointCount = map.childCount;

        int randomIndex;
        int i = 0;
        while (i < buildingNum)
        {
            randomIndex = UnityEngine.Random.Range(0, waypointCount);
            if (map.GetChild(randomIndex).GetComponent<Waypoint>().PlaceBuilding())
                i++;
        }
        i = 0;
        while (i < wallNum)
        {
            randomIndex = UnityEngine.Random.Range(0, waypointCount);
            if (map.GetChild(randomIndex).GetComponent<Waypoint>().PlaceWall())
                i++;
        }
    }

}
