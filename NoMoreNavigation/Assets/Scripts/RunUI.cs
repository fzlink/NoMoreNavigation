using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class RunUI : MonoBehaviour
{
    public TMP_Dropdown pathFindingAlgorithmDropdown;
    public TMP_Dropdown heuristicDropdown;
    public TMP_Text selectBuildingsHint;
    public TMP_Text heuristicText;
    public Toggle compareHeuristicsToggle;

    public Button selectBuildingsAndRunButton;
    public Button nextStepButton;
    public Button runButton;
    public Button slowerButton;
    public Button fasterButton;
    public Button resetMapButton;
    public GameObject topBar;
    public GameObject fasterSlowerContainer;
    public GameObject nextStepRunContainer;

    public Waypoint[] startFinish;

    public RunningManager runningManager;

    RaycastHit hit;
    Ray ray;
    private bool canPlace = true;
    private bool canSelect;

    private void Start()
    {
        runningManager.onResetMap += OnResetRequestMap;
        runningManager.onNextHeuristic += OnNextHeuristic;
        startFinish = new Waypoint[2];
        selectBuildingsAndRunButton.onClick.AddListener(() => SelectBuildings());
        nextStepButton.onClick.AddListener(() => NextStep());
        runButton.onClick.AddListener(() => Run());
        slowerButton.onClick.AddListener(() => MakeSlower());
        fasterButton.onClick.AddListener(() => MakeFaster());
        resetMapButton.onClick.AddListener(() => ResetMap());
    }

    private void ResetMap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNextHeuristic(int heuristicInd)
    {
        heuristicText.gameObject.SetActive(true);
        string heuristicName = ((Heuristic)heuristicInd).ToString();
        heuristicName = heuristicName.Replace("Distance", " Distance");
        heuristicText.text = heuristicName;
    }

    private void MakeFaster()
    {
        runningManager.MakeFaster();
    }

    private void MakeSlower()
    {
        runningManager.MakeSlower();
    }

    private void NextStep()
    {
        runningManager.NextStep();
    }

    private void Run()
    {
        nextStepRunContainer.SetActive(false);
        fasterSlowerContainer.SetActive(true);
        runningManager.Run();
    }

    private void OnResetRequestMap()
    {
        fasterSlowerContainer.SetActive(false);
        nextStepRunContainer.SetActive(false);
        resetMapButton.gameObject.SetActive(true);
    }

    private void SelectBuildings()
    {
        runningManager.isCompareHeuristics = compareHeuristicsToggle.isOn;
        selectBuildingsHint.gameObject.SetActive(true);
        canPlace = false;
        canSelect = true;
        topBar.SetActive(false);
        selectBuildingsAndRunButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // GUI Action
            return;
        }
        if (canPlace)
            PlaceObject();
        if (canSelect)
            PointToBuildings();
    }

    private void PointToBuildings()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if(hit.transform.GetComponent<Waypoint>() != null)
                {
                    if(startFinish[0] == null)
                    {
                        if(hit.transform.GetComponent<Waypoint>().building != null)
                        {
                            startFinish[0] = hit.transform.GetComponent<Waypoint>();
                            startFinish[0].HighlightBuilding();
                        }
                    }
                    else
                    {
                        if (hit.transform.GetComponent<Waypoint>().building != null)
                        {
                            startFinish[1] = hit.transform.GetComponent<Waypoint>();
                            startFinish[1].HighlightBuilding();
                            FinishSelecting();
                        }
                    }
                }
            }
        }
    }

    private void FinishSelecting()
    {
        canSelect = false;
        selectBuildingsHint.gameObject.SetActive(false);
        nextStepRunContainer.SetActive(true);
        runningManager.SetStartFinish(startFinish[0], startFinish[1], pathFindingAlgorithmDropdown.value, heuristicDropdown.value);
    }


    private void PlaceObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if(hit.transform.GetComponent<Waypoint>() != null)
                    hit.transform.GetComponent<Waypoint>().PlaceBuilding();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.GetComponent<Waypoint>() != null)
                    hit.transform.GetComponent<Waypoint>().PlaceWall();
            }
        }
    }
}
