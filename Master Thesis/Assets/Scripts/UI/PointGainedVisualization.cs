using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointGainedVisualization : MonoBehaviour
{
    public int initialFontSize;
    private Text text;
    private int pointsGained = 0;
    private float frameCounter;
    private int frameCount;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (frameCounter > 0)
        {
            if (frameCounter > 2)
            {
                if(frameCount % 5 == 0) text.fontSize = text.fontSize + 2;
            }
            else if(frameCounter <= 1)
            {
                if(frameCount % 5 == 0) text.fontSize = text.fontSize - 2;
            }
            frameCounter -= Time.deltaTime;
            frameCount = frameCount + 1;
            if (frameCounter <= 0) text.text = "";
        }
    }
    public void visualizePointsGained(int pointsEarned, Color color)
    {
        text.fontSize = initialFontSize;
        text.color = color;
        pointsGained = pointsEarned;
        frameCounter = 3.0f;
        frameCount = 0;
        updateText();
    }

    private void updateText()
    {
        text.text = "+ " + pointsGained;
    }

}
