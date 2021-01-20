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
    public Canvas notificationToSpawn;
    public float distanceTolerance = 1.0f;
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
        notificationToSpawn.enabled = false;
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
            sphere.activateSpawningAbility();
            sphere.setZRotationRight( (int) sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            sphere.setZRotationLeft((int)sphereCoordinates[sphereCoordinatesIndex].rotationZ2);
            cubePairPointedSpawned = true;
        }
        if(!choiceDialogeMade && sphere.bothCubesSpawned())
        {
            spawnCubes.spawnGameObject(false, sphereCoordinates[sphereCoordinatesIndex], cubeRedNeutral);
            GameObject correctRightHandTargetSpawn = spawnCubes.getLastRightHandTarget().gameObject;
            sphere.colorRightSpawnedCube(correctRightHandTargetSpawn, distanceTolerance);
            spawnCubes.spawnGameObject(true, sphereCoordinates[sphereCoordinatesIndex], cubeBlueNeutral);
            GameObject correctLeftHandTargetSpawn = spawnCubes.getLastLeftHandTarget().gameObject;
            sphere.colorLeftSpawnedCube(correctLeftHandTargetSpawn, distanceTolerance);
            activateChoiceUI(sphereCoordinates[sphereCoordinatesIndex++]);
            choiceDialogeMade = true;
        }
        if(menu.choiceWasMade())
        {
            timer += Time.deltaTime;
            Debug.Log("Timer got increased: " + timer);
            Debug.Log("Game Paused?: " + DifficultyManager.Instance.gamePaused);
        }
        if(timer > 3.5f)
        {
            spawnCubes.deleteAllSpawnedCubes();
            menu.Reset();
            menu.toggleUI();
            sphere.reset();
            cubePairSpawned = false;
            timer = 0f;
        }
    }

    private void activateChoiceUI(SphereCoordinates sc)
    {
        menu.toggleUI();
        menu.setRotationZRed(sc.rotationZ);
        menu.setRotationZBlue(sc.rotationZ2);
    }
}
