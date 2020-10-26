using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDisablerOnStart : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        pauseGame();
    }

    public void enableCanvas()
    {
        canvas.enabled = true;
    }

    public void disableCanvas()
    {
        canvas.enabled = false;
    }

    public void pauseGame()
    {
       DifficultyManager.Instance.pauseGame();
    }

    public void resumeGame()
    {
        DifficultyManager.Instance.resumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
