using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomObjectPlacerUI : MonoBehaviour
{

    public TMP_InputField buildingNumberText;
    public TMP_InputField wallNumberText;
    public Button randomPlacementButton;
    public RandomObjectPlacer randomObjectPlacer;

    private void Start()
    {
        randomPlacementButton.onClick.AddListener(() => RequestRandomObjectPlacement());
    }

    private void RequestRandomObjectPlacement()
    {
        try
        {
            randomObjectPlacer.PlaceRandomObjects(Convert.ToInt32(buildingNumberText.text), Convert.ToInt32(wallNumberText.text));
        }
        catch (Exception)
        {

        }
    }
}
