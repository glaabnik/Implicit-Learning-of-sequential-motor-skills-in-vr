using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionDifficultyValue : MonoBehaviour
{
    Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        Difficulty difficulty = DifficultyManager.Instance.difficulty;
        if (difficulty == Difficulty.Just_Fun) dropdown.value = 0;
        if (difficulty == Difficulty.Easy) dropdown.value = 1;
        if (difficulty == Difficulty.Middle) dropdown.value = 2;
        if (difficulty == Difficulty.Hard) dropdown.value = 3;
        if (difficulty == Difficulty.Very_Hard) dropdown.value = 4;
        if (difficulty == Difficulty.Extreme) dropdown.value = 5;
        if (difficulty == Difficulty.Extra_Extreme) dropdown.value = 6;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
