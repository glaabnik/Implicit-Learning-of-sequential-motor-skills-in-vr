using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SerializeData;

public class AnticipitationScript : MonoBehaviour
{
    public BlockSequence sequenceToUse;
    public SpawnCubes spawnCubes;
    public SphereToSpawnGreyCube sphere;
    public GameObject menuFollower;
    private DelegateButtonCallsChoiceMenu choiceMenu;
    public GameObject cubeRedNeutral, cubeBlueNeutral;
    public Canvas notificationToSpawn;
    public float distanceTolerance = 1.0f;
    public bool showResultsChoiceMenu = false;
    public bool spawnActualCubePositions = false;
    // Start is called before the first frame update
    private SphereCoordinates[] sphereCoordinates;
    private bool sphereCoordinatesLoad = false;
    private bool cubePairSpawned = false;
    private int sphereCoordinatesIndex = 0;
    private bool cubePairPointedSpawned = true;
    private bool choiceDialogeMade = true;
    private bool rotationZChoosenAdded = false;
    private bool fileWritten = false;
    private bool menuTimerStarted = false;
    private bool sphereMoved = false;
    private float timeToWaitAfterChoice;
    float timer = 0f;
    float startTimer = 0f;
    float menuTimer = 0f;

    public List<TransformData> transformHitLeft, transformHitRight;
    public List<TransformData> transformLeft, transformRight;
    public List<TransformData> transformPointedLeft, transformPointedRight;
    public List<float> rotationZLeft, rotationZRight;
    public List<float> rotationZChoosenLeft, rotationZChoosenRight;

    public void Start()
    {
        choiceMenu = menuFollower.GetComponentInChildren<DelegateButtonCallsChoiceMenu>();
        notificationToSpawn.enabled = false;
        transformLeft = new List<TransformData>();
        transformRight = new List<TransformData>();
        transformPointedLeft = new List<TransformData>();
        transformPointedRight = new List<TransformData>();
        transformHitLeft = new List<TransformData>();
        transformHitRight = new List<TransformData>();
        rotationZLeft = new List<float>();
        rotationZRight = new List<float>();
        rotationZChoosenLeft = new List<float>();
        rotationZChoosenRight = new List<float>();
        getLoadedSphereCoordinates();
        sphere.Initialise();
        if (showResultsChoiceMenu) timeToWaitAfterChoice = 3.5f;
        else timeToWaitAfterChoice = 0.2f;
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
        startTimer += Time.deltaTime;
        if (DifficultyManager.Instance == null || DifficultyManager.Instance.gamePaused || startTimer < 1.5f)
        {
            sphere.activatePointer(true);
            sphere.activatePointer(false);
            return;
        }
       
        if (!cubePairSpawned && cubePairPointedSpawned && choiceDialogeMade)
        {
            if (sphereCoordinatesIndex >= sphereCoordinates.Length)
            {
                if(!fileWritten)
                {
                    fileWritten = true;
                    SerializeData.SerializeAntizipationTestData("antizipationtest", transformLeft, transformRight, transformPointedLeft, transformPointedRight, transformHitLeft, transformHitRight,
                        rotationZLeft, rotationZRight, rotationZChoosenLeft, rotationZChoosenRight, choiceMenu.getScoreRightChoices());
                }
                return;
            }
            spawnCubes.spawnCubesForSphereCoordinates(sphereCoordinates[sphereCoordinatesIndex++]);
            transformHitLeft.Add(spawnCubes.getTransformDataLastLeftCube());
            transformHitRight.Add(spawnCubes.getTransformDataLastRightCube());
            rotationZLeft.Add(sphereCoordinates[sphereCoordinatesIndex].rotationZ2);
            rotationZRight.Add(sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            cubePairSpawned = true;
            cubePairPointedSpawned = false;
            choiceDialogeMade = false;
        }
        if(cubePairSpawned && spawnCubes.bothCubesDestroyedOrHit() && !cubePairPointedSpawned)
        {
            sphere.moveMeshColliderUp();
            if (sphere.getPosition().y >= 0) sphereMoved = true;
        
        }
        if(sphereMoved)
        {
            cubePairPointedSpawned = true;
            sphere.activateSpawningAbility();
            /*sphere.setZRotationRight( (int) sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            sphere.setZRotationLeft((int)sphereCoordinates[sphereCoordinatesIndex].rotationZ2);*/
            sphereMoved = false;
        }
        if(!choiceDialogeMade && sphere.bothCubesSpawned())
        {
            if(spawnActualCubePositions) // Feedback is now optional code which is by default not executed
            {
                spawnCubes.spawnGameObject(false, sphereCoordinates[sphereCoordinatesIndex], cubeRedNeutral);
                GameObject correctRightHandTargetSpawn = spawnCubes.getLastRightHandTarget().gameObject;
                sphere.colorRightSpawnedCube(correctRightHandTargetSpawn, distanceTolerance);
                spawnCubes.spawnGameObject(true, sphereCoordinates[sphereCoordinatesIndex], cubeBlueNeutral);
                GameObject correctLeftHandTargetSpawn = spawnCubes.getLastLeftHandTarget().gameObject;
                sphere.colorLeftSpawnedCube(correctLeftHandTargetSpawn, distanceTolerance);
                transformLeft.Add(spawnCubes.getTransformDataLastLeftCube());
                transformRight.Add(spawnCubes.getTransformDataLastRightCube());
            }
            else
            {
                transformLeft.Add(new TransformData(spawnCubes.sphereToCartesianCoordinate(sphereCoordinates[sphereCoordinatesIndex].theta2, sphereCoordinates[sphereCoordinatesIndex].phi2, spawnCubes.sphereRadius)));
                transformRight.Add(new TransformData(spawnCubes.sphereToCartesianCoordinate(sphereCoordinates[sphereCoordinatesIndex].theta, sphereCoordinates[sphereCoordinatesIndex].phi, spawnCubes.sphereRadius)));
            }
            
            transformPointedLeft.Add(new TransformData(sphere.getTransformLeft()));
            transformPointedRight.Add(new TransformData(sphere.getTransformRight()));
            sphere.activatePointer(true);
            sphere.activatePointer(false);
            sphere.moveMeshColliderDown();
            menuTimerStarted = true;
            choiceDialogeMade = true;
        }
        if(menuTimerStarted)
        {
            menuTimer += Time.deltaTime;
            if(menuTimer > 1.2)
            {
                menuTimerStarted = false;
                activateChoiceUI(sphereCoordinates[sphereCoordinatesIndex++]);
            }
        }
        if(choiceMenu.choiceWasMade())
        {
            if (!rotationZChoosenAdded)
            {
                rotationZChoosenRight.Add(choiceMenu.getRotationChoosenRed());
                rotationZChoosenLeft.Add(choiceMenu.getRotationChoosenBlue());
                rotationZChoosenAdded = true;
            }
            timer += Time.deltaTime;
            sphere.deactivatePointer(false);
            sphere.deactivatePointer(true);
        }
        if(timer > timeToWaitAfterChoice)
        {
            timer = 0f;
            cubePairSpawned = false;
            rotationZChoosenAdded = false;
            spawnCubes.deleteAllSpawnedCubes();
            choiceMenu.Reset();
            choiceMenu.toggleUI();
            sphere.reset();
        }
    }

    private void activateChoiceUI(SphereCoordinates sc)
    {
        choiceMenu.toggleUI();
        choiceMenu.setRotationZRed(sc.rotationZ);
        choiceMenu.setRotationZBlue(sc.rotationZ2);
    }
}
