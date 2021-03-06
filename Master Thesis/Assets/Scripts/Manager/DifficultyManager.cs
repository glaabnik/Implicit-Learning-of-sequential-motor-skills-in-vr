using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Just_Fun,
    Easy,
    Middle,
    Hard,
    Very_Hard,
    Extreme,
    Extra_Extreme,
    Custom
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance = null;
    public Difficulty difficulty = Difficulty.Middle;
    public bool gamePaused = false;
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void pauseGame()
    {
        gamePaused = true;
    }

    public void resumeGame()
    {
        gamePaused = false;
    }

    public void setDifficulty(Difficulty difficultyN)
    {
        difficulty = difficultyN;
    }

    public float getPointModifier()
    {
        switch(difficulty)
        {
            case Difficulty.Just_Fun: return 0.25f;
            case Difficulty.Easy: return 0.5f;
            case Difficulty.Middle: return 1.0f;
            case Difficulty.Hard: return 1.2f;
            case Difficulty.Very_Hard: return 1.5f;
            case Difficulty.Extreme: return 2.0f;
            case Difficulty.Extra_Extreme: return 3.0f;
            default: return 1.0f;
        }
    }

    public float getTimeToHitObjects()
    {
        switch (difficulty)
        {
            case Difficulty.Just_Fun: return 10f;
            case Difficulty.Easy: return 5f;
            case Difficulty.Middle: return 3f;
            case Difficulty.Hard: return 2.5f;
            case Difficulty.Very_Hard: return 2.0f;
            case Difficulty.Extreme: return 1.5f;
            case Difficulty.Extra_Extreme: return 0.7f;
            default: return 10f;
        }
    }

    public float getScaleObjects()
    {
        switch (difficulty)
        {
            case Difficulty.Just_Fun: return 0.38f;
            case Difficulty.Easy: return 0.35f;
            case Difficulty.Middle: return 0.3f;
            case Difficulty.Hard: return 0.25f;
            case Difficulty.Very_Hard: return 0.2f;
            case Difficulty.Extreme: return 0.18f;
            case Difficulty.Extra_Extreme: return 0.165f;
            default: return 0.4f;
        }
    }

    public float getSphereRadius()
    {
        switch (difficulty)
        {
            case Difficulty.Just_Fun: return 1.15f;
            case Difficulty.Easy: return 1.2f;
            case Difficulty.Middle: return 1.25f;
            case Difficulty.Hard: return 1.3f;
            case Difficulty.Very_Hard: return 1.5f;
            case Difficulty.Extreme: return 1.5f;
            case Difficulty.Extra_Extreme: return 1.5f;
            default: return 0.4f;
        }
    }
}
