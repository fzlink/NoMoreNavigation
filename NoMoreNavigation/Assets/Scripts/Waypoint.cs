using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    const int gridSize = 10;
    public Waypoint parent { get; set; }
    public bool isExplored { get; set; }
    public bool isBlocked { get; set; }
    public bool isPlaceable { get; set; } = true;

    public Renderer renderer;

    [SerializeField] GameObject buildingPrefab;
    [SerializeField] GameObject wallPrefab;

    public float h { get; set; }
    public float g { get; set; }
    public float f => g + h;

    public GameObject building;
    public GameObject wall;

    public int GetGridSize()
    {
        return gridSize;
    }
    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize));
    }
    public void SetTopColor(Color color)
    {
        MeshRenderer meshRenderer = transform.Find("Top").GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
    }

    public void Paint()
    {
        renderer.material.color = Color.blue;
    }

    public void UnPaint()
    {
        renderer.material.color = Color.white;
    }

    public bool isEqual(Waypoint waypoint)
    {
        if (GetGridPos() == waypoint.GetGridPos()) { return true; }
        else { return false; }
    }

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0) && isPlaceable)
    //    {
    //        PlaceBuilding();
    //    }
    //    else if (Input.GetMouseButtonDown(1) && isPlaceable)
    //    {
    //        PlaceWall();
    //    }
    //}

    public void HighlightBuilding()
    {
        if(building != null)
        {
            building.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }

    public bool PlaceBuilding()
    {
        if (isPlaceable)
        {
            building = Instantiate(buildingPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity,transform);
            isPlaceable = false;
            isBlocked = true;
            return true;
        }
        return false;
    }

    public bool PlaceWall()
    {
        if (isPlaceable)
        {
            wall = Instantiate(wallPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity,transform);
            isPlaceable = false;
            isBlocked = true;
            return true;
        }
        return false;
    }

    public void DeletePlacement()
    {
        if(building != null)
        {
            Destroy(building);
        }
        else if(wall != null)
        {
            Destroy(wall);
        }
        isPlaceable = true;
        isBlocked = false;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if (transform.GetChild(i).tag == ("Building") || transform.GetChild(i).tag == ("Wall"))
        //    {
        //        Destroy(transform.GetChild(i).gameObject);
        //        isPlaceable = true;
        //        isBlocked = false;
        //        break;
        //    }
        //}
    }

    public bool CanExplore()
    {
        return !isBlocked && !isExplored;
    }

}
