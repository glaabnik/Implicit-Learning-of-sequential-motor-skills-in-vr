using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text[] text;
    public PointGainedVisualization pgv;
    private int highScore = 0;

    public void updateHighscore(int pointsEarned)
    {
        highScore += pointsEarned;
        updateText();
        pgv.visualizePointsGained(pointsEarned);
    }

    private void updateText()
    {
        Text t = gameObject.GetComponent<Text>();
        t.text = "Highscore: " + highScore;
        foreach(Text tex in text)
        tex.text = "Highscore: " + highScore;
    }

}
