using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakWindow : MonoBehaviour
{
    public Text breakInfo;
    public Text breakInfo2;
    public Text textTime;
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        disableWindow();
    }

    public void messageGameFinished()
    {
        breakInfo.text = "Game Completed";
        breakInfo.color = new Color(229 / 255.0f, 103 / 255.0f, 23 / 255.0f);
        breakInfo2.text = "Thanks for playing the game :)";
        breakInfo2.color = new Color(255 / 255.0f, 132 / 255.0f, 0 / 255.0f);
        textTime.text = "Next step will be the anticipation test";
        textTime.color = new Color(255 / 255.0f, 132 / 255.0f, 0 / 255.0f);
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
        textTime.text = "Remaining time: " + (Mathf.Round(time_left * 10.0f) * 0.1f).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
