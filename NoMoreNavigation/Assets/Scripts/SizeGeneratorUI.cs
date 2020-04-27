using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SizeGeneratorUI : MonoBehaviour
{
    public TMP_InputField widthText;
    public TMP_InputField heightText;
    public Button generateButton;

    public GridScaler gridScaler;

    private void Start()
    {
        generateButton.onClick.AddListener(() => GenerateSize());
    }

    public void GenerateSize()
    {
        try
        {
            gridScaler.ScaleGrid(Convert.ToInt32(widthText.text), Convert.ToInt32(heightText.text));
        }
        catch (Exception)
        {

        }
    }


}
