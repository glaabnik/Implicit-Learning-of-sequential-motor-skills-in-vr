using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticipitationScript : MonoBehaviour
{
    public BlockSequence sequenceToUse;
    public SpawnCubes spawnCubes;
    public SphereToSpawnGreyCube sphere;
    public DelegateButtonCallsChoiceMenu menu;
    public GameObject cubeRedNeutral, cubeBlueNeutral;
    // Start is called before the first frame update
    private SphereCoordinates[] sphereCoordinates;
    private bool sphereCoordinatesLoad = false;
    private bool cubePairSpawned = false;
    private int sphereCoordinatesIndex = 0;
    private bool cubePairPointedSpawned = true;
    private bool choiceDialogeMade = true;
    float timer = 0f;
    
    public void Start()
    {
        
    }

    private void getLoadedSphereCoordinates()
    {
        if(!sphereCoordinatesLoad)
        {
            sphereCoordinates = sequenceToUse.twoRandomSphereCoordinatesPairsForWholeSequence();
            sphereCoordinatesLoad = true;
        }
       
    }

    // Update is called once per frame
    public void Update()
    {
        if (DifficultyManager.Instance == null || DifficultyManager.Instance.gamePaused) return;

        getLoadedSphereCoordinates();
        if (!cubePairSpawned && cubePairPointedSpawned && choiceDialogeMade)
        {
            spawnCubes.spawnCubesForSphereCoordinates(sphereCoordinates[sphereCoordinatesIndex++]);
            cubePairSpawned = true;
            cubePairPointedSpawned = false;
            choiceDialogeMade = false;
        }
        if(cubePairSpawned && spawnCubes.bothCubesDestroyedOrHit() && !cubePairPointedSpawned)
        {
            activateUIPointer();
            sphere.activateSpawningAbility();
            sphere.setZRotationRight( (int) sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            sphere.setZRotationLeft((int)sphereCoordinates[sphereCoordinatesIndex].rotationZ2);
            cubePairPointedSpawned = true;
        }
        if(!choiceDialogeMade && sphere.bothCubesSpawned())
        {
            deactivateUIPointer();
            spawnCubes.spawnGameObject(false, sphereCoordinates[sphereCoordinatesIndex], cubeRedNeutral);
            spawnCubes.spawnGameObject(true, sphereCoordinates[sphereCoordinatesIndex], cubeBlueNeutral);
            activateChoiceUI(sphereCoordinates[sphereCoordinatesIndex++]);
            choiceDialogeMade = true;
        }
        if(menu.choiceWasMade())
        {
            timer += Time.deltaTime;
        }
        if(timer > 3.5)
        {
            menu.Reset();
            menu.toggleUI();
            sphere.reset();
            cubePairSpawned = false;
            timer = 0f;
        }
    }

    private void activateUIPointer()
    {
        sphere.activatePointer();
    }

    private void deactivateUIPointer()
    {
        sphere.deactivatePointer();
    }

    private void activateChoiceUI(SphereCoordinates sc)
    {
        menu.toggleUI();
        menu.setRotationZRed(sc.rotationZ);
        menu.setRotationZBlue(sc.rotationZ2);
    }
}
