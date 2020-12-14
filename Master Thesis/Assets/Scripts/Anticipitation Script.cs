using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticipitationScript : MonoBehaviour
{
    public BlockSequence sequenceToUse;
    public SpawnCubes spawnCubes;
    // Start is called before the first frame update
    private SphereCoordinates[] sphereCoordinates;
    private bool cubePairSpawned = false;
    private int sphereCoordinatesIndex = 0;
    private bool cubePairPointedSpawned = true;
    private bool choiceDialogeMade = true;
    void Start()
    {
        sphereCoordinates = sequenceToUse.twoRandomSphereCoordinatesPairsForWholeSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if(!cubePairSpawned && cubePairPointedSpawned && choiceDialogeMade)
        {
            spawnCubes.spawnCubesForSphereCoordinates(sphereCoordinates[sphereCoordinatesIndex++]);
            cubePairSpawned = true;
            cubePairPointedSpawned = false;
            choiceDialogeMade = false;
        }
        if(cubePairSpawned && spawnCubes.bothCubesDestroyedOrHit() && !cubePairPointedSpawned)
        {
            activateUIPointer();
            cubePairPointedSpawned = true;
        }
        if(!choiceDialogeMade && cubePairPointedSpawned)
        {
            activateChoiceUI(sphereCoordinates[sphereCoordinatesIndex++]);
            choiceDialogeMade = true;
        }
    }

    private void activateUIPointer()
    {

    }

    private void activateChoiceUI(SphereCoordinates sc)
    {

    }
}
