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


    [SerializeField] GameObject buildingPrefab;
    [SerializeField] GameObject wallPrefab;

    public float h { get; set; }
    public float g { get; set; }
    public float f => g + h;

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

    public bool isEqual(Waypoint waypoint)
    {
        if (GetGridPos() == waypoint.GetGridPos()) { return true; }
        else { return false; }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isPlaceable)
        {
            Instantiate(buildingPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
            isPlaceable = false;
            isBlocked = true;
        }
        else if (Input.GetMouseButtonDown(1) && isPlaceable)
        {
            Instantiate(wallPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
            isPlaceable = false;
            isBlocked = true;
        }
    }

    public bool CanExplore()
    {
        return !isBlocked && !isExplored;
    }

}
