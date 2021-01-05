using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DelegateButtonCalls : MonoBehaviour
{
    public SpawnCubes sc;
    public Slider reaction, sphere, scale, pointsMultiplicator;
    public Dropdown dropdownDifficulty;
    public CanvasDisablerOnStart cdos;
    private DifficultyManager dm;

    public void Start()
    {
        dm = DifficultyManager.Instance;
    }

    public void delegate_Difficulty_Dropdown()
    {
        switch(dropdownDifficulty.value)
        {
            case 0: button_Just_Fun(); break;
            case 1: button_Easy(); break;
            case 2: button_Middle(); break;
            case 3: button_Hard(); break;
            case 4: button_Very_Hard(); break;
            case 5: button_Extreme(); break;
            case 6: button_Extra_Extreme(); break;
            default: break;
        }
    }

    public void startOrResumeGame()
    {
        cdos.resumeGame();
        cdos.disableCanvas();
        if(reaction.value != DifficultyManager.Instance.getTimeToHitObjects()
            || scale.value != DifficultyManager.Instance.getScaleObjects())
        {
            button_Custom();
        }
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

    public void button_Extra_Extreme()
    {
        dm.setDifficulty(Difficulty.Extra_Extreme);
        sc.updateDifficultyParameters();
        updateSliderValues();
    }

    private void updateSliderValues()
    {
        reaction.value = dm.getTimeToHitObjects();
        scale.value = dm.getScaleObjects();
        pointsMultiplicator.value = dm.getPointModifier();
    }

    public void updateSphereRadius()
    {
        sc.updateSphereRadius(sphere.value);
    }

    public void button_Custom()
    {
        dm.setDifficulty(Difficulty.Custom);
        pointsMultiplicator.value = dm.getPointModifier();
        sc.updateDifficultyParameters(reaction.value, scale.value);
    }
}
