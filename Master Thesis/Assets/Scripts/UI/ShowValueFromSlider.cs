using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ShowValueFromSlider : MonoBehaviour
{
    public Text label;

    private void Start()
    {
        label = GetComponent<Text>();
    }

    public void updateText(float sliderValue)
    {
        label.text = (Mathf.Round(sliderValue * 10.0f) * 0.1f).ToString();
    }
}
