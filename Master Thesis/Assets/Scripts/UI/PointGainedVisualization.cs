using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointGainedVisualization : MonoBehaviour
{
    public int initialFontSize;
    public Text labelSword;
    private Text text;
    private int pointsGained = 0;
    private float frameCounter;
    private int frameCount;
    private string labelString;

    void Start()
    {
        text = GetComponent<Text>();
        labelString = labelSword.text;
        labelSword.text = "";
        text.text = "";
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
            if (frameCounter <= 0)
            {
                labelSword.text = "";
                text.text = "";
            }
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
        labelSword.text = labelString;
        text.text = "+ " + pointsGained;
    }

}
