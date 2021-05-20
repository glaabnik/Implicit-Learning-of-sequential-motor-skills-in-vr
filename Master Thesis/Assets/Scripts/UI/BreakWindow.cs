using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakWindow : MonoBehaviour
{
    public Text text;
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        disableWindow();
    }

    public void enableWindow()
    {
        canvas.enabled = true;
    }

    public void disableWindow()
    {
        canvas.enabled = false;
    }

    public void updateRemainingTime(float time_left)
    {
        text.text = "Remaining time: " + (Mathf.Round(time_left * 10.0f) * 0.1f).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
