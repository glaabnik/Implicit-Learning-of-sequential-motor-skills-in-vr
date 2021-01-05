using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text[] text;
    public Text[] textActIteration;
    public Text[] textBestIteration;
    public Text[] textAvgIteration;
    public PointGainedVisualization pgv;
    public SpawnCubes spawnCubes;
    private int highScore = 0;

    public void updateHighscore(int pointsEarned, Color color)
    {
        highScore += pointsEarned;
        spawnCubes.addToPointScoreInBlockSequence(pointsEarned);
        updateText();
        if (spawnCubes.getActBlockSequence() != null) updateTextBlockSequence();
        pgv.visualizePointsGained(pointsEarned, color);
    }

    private void updateText()
    {
        Text t = gameObject.GetComponent<Text>();
        t.text = "Highscore: " + highScore;
        foreach(Text tex in text)
        tex.text = "Highscore: " + highScore;
    }

    private void updateTextBlockSequence()
    {
        BlockSequence bs = spawnCubes.getActBlockSequence();

        int pointScoreAct = bs.pointScoreActIteration;
        int pointScoreMax = bs.maxPointScoreOneIteration;
        int pointScoreAvg = (int) bs.getAvgPointScoreOneIteration();

        foreach (Text tex in textActIteration)
            tex.text = "Highscore act Iteration: " + pointScoreAct;
        foreach (Text tex in textBestIteration)
             tex.text = pointScoreMax != 0 ? ("Highscore best Iteration: " + pointScoreMax) : ("Highscore best Iteration: " + pointScoreAct);
        foreach (Text tex in textAvgIteration)
            tex.text = pointScoreAvg != 0 ? "Avg Highscore of Iterations: " + pointScoreAvg : "Avg Highscore of Iterations: ---";
    }

}
