using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{



    TextMesh textMesh;
    Waypoint waypoint;
    int gridSize;
    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }
    private void Start()
    {
        gridSize = waypoint.GetGridSize();
        textMesh = GetComponentInChildren<TextMesh>();
    }
    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        string labelText = waypoint.GetGridPos().x + "," + waypoint.GetGridPos().y;
        textMesh.text = labelText;
        gameObject.name = labelText;
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(waypoint.GetGridPos().x * gridSize, 0f, waypoint.GetGridPos().y * gridSize);
    }

}
