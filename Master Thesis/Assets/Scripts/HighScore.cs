using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private int highScore = 0;

    public void updateHighscore(int pointsEarned)
    {
        highScore += pointsEarned;
        updateText();
    }

    private void updateText()
    {
        Text t = gameObject.GetComponent<Text>();
        t.text = "Highscore: " + highScore;
    }

}
