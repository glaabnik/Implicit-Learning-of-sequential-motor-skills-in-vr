using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DelegateButtonCalls : MonoBehaviour
{
    public SpawnCubes sc;
    public Slider reaction, sphere, scale, pointsMultiplicator;
    private DifficultyManager dm;

    public void Start()
    {
        dm = DifficultyManager.Instance;
    }

    public void button_Just_Fun()
    {
        dm.setDifficulty(Difficulty.Just_Fun);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    public void button_Easy()
    {
        dm.setDifficulty(Difficulty.Easy);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    public void button_Middle()
    {
        dm.setDifficulty(Difficulty.Middle);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    public void button_Hard()
    {
        dm.setDifficulty(Difficulty.Hard);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    public void button_Very_Hard()
    {
        dm.setDifficulty(Difficulty.Very_Hard);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    public void button_Extreme()
    {
        dm.setDifficulty(Difficulty.Extreme);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    private void updateSliderValues()
    {
        reaction.value = dm.getTimeToHitObjects();
        sphere.value = dm.getSphereRadius();
        scale.value = dm.getScaleObjects();
        pointsMultiplicator.value = dm.getPointModifier();
    }

    public void button_Custom()
    {
        dm.setDifficulty(Difficulty.Custom);
        pointsMultiplicator.value = dm.getPointModifier();
        sc.updateDifficultyParameters(reaction.value, scale.value, sphere.value);
    }
}
