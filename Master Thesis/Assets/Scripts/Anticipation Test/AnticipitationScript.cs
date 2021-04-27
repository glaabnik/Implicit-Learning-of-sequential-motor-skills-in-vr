using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SerializeData;

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
    private bool rotationZChoosenAdded = false;
    private bool fileWritten = false;
    float timer = 0f;
    float startTimer = 0f;

    public List<TransformData> transformLeft, transformRight;
    public List<TransformData> transformPointedLeft, transformPointedRight;
    public List<float> rotationZLeft, rotationZRight;
    public List<float> rotationZChoosenLeft, rotationZChoosenRight;

    public void Start()
    {
        notificationToSpawn.enabled = false;
        transformLeft = new List<TransformData>();
        transformRight = new List<TransformData>();
        transformPointedLeft = new List<TransformData>();
        transformPointedRight = new List<TransformData>();
        rotationZLeft = new List<float>();
        rotationZRight = new List<float>();
        rotationZChoosenLeft = new List<float>();
        rotationZChoosenRight = new List<float>();
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
       
        getLoadedSphereCoordinates();
        if (!cubePairSpawned && cubePairPointedSpawned && choiceDialogeMade)
        {
            if (sphereCoordinatesIndex >= sphereCoordinates.Length)
            {
                if(!fileWritten)
                {
                    fileWritten = true;
                    SerializeData.SerializeAntizipationTestData("antizipationtest", transformLeft, transformRight, transformPointedLeft, transformPointedRight,
                        rotationZLeft, rotationZRight, rotationZChoosenLeft, rotationZChoosenRight);
                }
                return;
            }
            rotationZLeft.Add(sphereCoordinates[sphereCoordinatesIndex].rotationZ2);
            rotationZRight.Add(sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            spawnCubes.spawnCubesForSphereCoordinates(sphereCoordinates[sphereCoordinatesIndex++]);
            cubePairSpawned = true;
            cubePairPointedSpawned = false;
            choiceDialogeMade = false;

            transformLeft.Add(new TransformData(spawnCubes.getTransformLastLeftCube()));
            transformRight.Add(new TransformData(spawnCubes.getTransformLastRightCube()));
        }
        if(cubePairSpawned && spawnCubes.bothCubesDestroyedOrHit() && !cubePairPointedSpawned)
        {
            sphere.activateSpawningAbility();
            /*sphere.setZRotationRight( (int) sphereCoordinates[sphereCoordinatesIndex].rotationZ);
            sphere.setZRotationLeft((int)sphereCoordinates[sphereCoordinatesIndex].rotationZ2);*/
            cubePairPointedSpawned = true;
        }
        if(!choiceDialogeMade && sphere.bothCubesSpawned())
        {
            /*spawnCubes.spawnGameObject(false, sphereCoordinates[sphereCoordinatesIndex], cubeRedNeutral);
            GameObject correctRightHandTargetSpawn = spawnCubes.getLastRightHandTarget().gameObject;
            sphere.colorRightSpawnedCube(correctRightHandTargetSpawn, distanceTolerance);
            spawnCubes.spawnGameObject(true, sphereCoordinates[sphereCoordinatesIndex], cubeBlueNeutral);
            GameObject correctLeftHandTargetSpawn = spawnCubes.getLastLeftHandTarget().gameObject;
            sphere.colorLeftSpawnedCube(correctLeftHandTargetSpawn, distanceTolerance);
            */ // Feedback Code no longer used instead TO DO Logging of correct values + values taken from participant
            transformPointedLeft.Add(new TransformData(sphere.getTransformLeft()));
            transformPointedRight.Add(new TransformData(sphere.getTransformRight()));
            sphere.activatePointer(true);
            sphere.activatePointer(false);
            activateChoiceUI(sphereCoordinates[sphereCoordinatesIndex++]);
            choiceDialogeMade = true;
        }
        if(menu.choiceWasMade())
        {
            if(!rotationZChoosenAdded)
            {
                rotationZChoosenRight.Add(menu.getRotationChoosenRed());
                rotationZChoosenLeft.Add(menu.getRotationChoosenBlue());
                rotationZChoosenAdded = true;
            }
            timer += Time.deltaTime;
            sphere.deactivatePointer(false);
            sphere.deactivatePointer(true);
        }
        if(timer > 3.5f)
        {
            timer = 0f;
            cubePairSpawned = false;
            rotationZChoosenAdded = false;
            spawnCubes.deleteAllSpawnedCubes();
            menu.Reset();
            menu.toggleUI();
            sphere.reset();
        }
    }

    private void activateChoiceUI(SphereCoordinates sc)
    {
        menu.toggleUI();
        menu.setRotationZRed(sc.rotationZ);
        menu.setRotationZBlue(sc.rotationZ2);
    }
}
