using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SerializeData;

public class SphereCoordinates
{
    public float phi;
    public float theta;
    public float phi2;
    public float theta2;
    public float radius;
    public float radius2;
    public float rotationZ;
    public float rotationZ2;
    public Vector3 scale;
    public Vector3 scale2;
    public SphereCoordinates(float p, float t, float p2, float t2)
    {
        phi = p;
        theta = t;
        phi2 = p2;
        theta2 = t2;
    }
    public SphereCoordinates()
    {
        phi = 0.0f;
        theta = 0.0f;
        phi2 = 0.0f;
        theta2 = 0.0f;
    }
}

public class SpawnCubes : MonoBehaviour
{
    public Transform hmd_transform;
    public GameObject leftHandController;
    public GameObject rightHandController;
    public GameObject rightHandGameObject;
    public GameObject leftHandGameObject;
    public GameObject rightHandGameObjectDiagonal;
    public GameObject leftHandGameObjectDiagonal;
    public BreakWindow breakWindow;
    public CanvasDisablerOnStart difficultySettingsMenu;
    public float sphereRadius;
    public float timeToHitGameObjects;
    public float timeToWaitBetween;
    public float scaleSpawnedGameObjects = 1.0f;
    public bool animatedSpawning = true;
    public bool useScaleFromSphereCoordinates = false;
    public bool turnLoadedSequenceTowardsPlayer = false;
    public bool playBackgroundMusic = true;
    public bool noAutomatedSpawning = false;
    public bool makeBreakBetweenBlocks = true;
    public bool p_key_to_pause_game = false;
    public int breakTimeInSeconds = 30;
    public BlockSequence[] blockSequences;

    // Debug Variables
    public Vector3 forwardVectorTest;

    private HitInteractable leftHand;
    private HitInteractable rightHand;
    private int blockSequenceIndex = 0;
    private float timeCounter;
    private float instantiateTimeCounter;
    private float breakTimer = 30.0f;
    private bool instantiated;
    private SpawnedInteractable lastLeftHandTarget;
    private SpawnedInteractable lastRightHandTarget;
    private int objectsSpawned = 0;
    private int roundGenerated = 0;
    private bool fileWritten = false;
    private List<SpawnedInteractable> listOfSpawnedCubes;
    private TransformData transformLeftCube, transformRightCube;
    private List<int> listCubePairCount, listIterationCount, listBestPointScore, listPointScoreAllIterations, listAvgPointScore;
    void Start()
    {
        listOfSpawnedCubes = new List<SpawnedInteractable>();
        timeCounter = 0;
        instantiateTimeCounter = timeToWaitBetween + 1;
        forwardVectorTest = hmd_transform.forward;
        if (playBackgroundMusic) SoundManager.Instance.PlayBackgroundMusic();
        findReferencesToHitInteractables();
        updateDifficultyParameters();
        DifficultyManager dm = DifficultyManager.Instance;
        updateSphereRadius(dm.getSphereRadius());
        listCubePairCount = new List<int>();
        listIterationCount = new List<int>();
        listBestPointScore = new List<int>();
        listPointScoreAllIterations = new List<int>();
        listAvgPointScore = new List<int>();
        difficultySettingsMenu.setPlayerCanOpenCanvas(!p_key_to_pause_game);
    }

    public TransformData getTransformDataLastLeftCube()
    {
        return transformLeftCube;
    }

    public TransformData getTransformDataLastRightCube()
    {
        return transformRightCube;
    }

    public BlockSequence getActBlockSequence()
    {
        if (blockSequences != null) return blockSequences[blockSequenceIndex];
        return null;
    }

    public void addToPointScoreInBlockSequence(int val)
    {
        if (getActBlockSequence() != null) getActBlockSequence().addToPointScore(val);
    }

    public SpawnedInteractable getLastLeftHandTarget()
    {
        return lastLeftHandTarget;
    }

    public SpawnedInteractable getLastRightHandTarget()
    {
        return lastRightHandTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (noAutomatedSpawning || fileWritten || DifficultyManager.Instance == null || DifficultyManager.Instance.gamePaused) return;

        if(Input.GetKeyDown(KeyCode.P))
        {
            difficultySettingsMenu.enableCanvas();
            difficultySettingsMenu.pauseGame();
            return;
        }

        if (blockSequences != null && blockSequences.Length > 0 && blockSequenceIndex >= blockSequences.Length && !fileWritten) // check if there is a valid blockSequenceArrray and all elements of the block array
                                                                                                                                // were allready spawned, then write the gathered sportgamedata to file
                                                                                                                                // from now on there will nothing happen here anymore
        {
            fileWritten = true;
            serializeSportGameData();
            serializeBlockGameData();
        }

        breakTimer += Time.deltaTime;

        if (breakTimer <= breakTimeInSeconds)
        {
            breakWindow.enableWindow();
            breakWindow.updateRemainingTime(breakTimeInSeconds - breakTimer);
            return;
        }
        else breakWindow.disableWindow();

        float animationTimeBonus = animatedSpawning ? 2.0f : 0.0f; // if the option animated spawning is active the cubes need exactly 2 seconds to fly to their destination were they can be hit
                                                                   // so this has be taken into calculation when measuring the time a player needed or has to hit the cubes

        if(instantiated && bothCubesDestroyedOrHit() ) 
            // if there is currently one instantiated cube pair, then check if the references to the cubes are already destroyed or if points were rewarded for hitting the cubes
            // so the timer is set over the limit time to hit the cubes, so that the spawning of the next cube pair can immediatly start in this call of update
        {
            timeCounter = timeToHitGameObjects + animationTimeBonus;
        }
        forwardVectorTest = hmd_transform.forward;
        timeCounter += Time.deltaTime;
        instantiateTimeCounter += Time.deltaTime;

        if(instantiateTimeCounter >= timeToWaitBetween && !instantiated) // check if instantiation time has passed and the current cube pair was not already instantiated
        {
            spawnCubesForBothHandsInSightOfCameraDirection(); // spawns one cube pair
        }

        if(timeCounter >= timeToHitGameObjects + animationTimeBonus) // Clean up remaining not hit cubes, if the time to hit the cubes passed, then reinitialise relevant variables
        {
            float destroyTimeLeft = 0;
            float destroyTimeRight = 0;
            if (lastLeftHandTarget == null || lastLeftHandTarget.getPointsRewarded()) destroyTimeLeft = 5.0f;
            if (lastRightHandTarget == null || lastRightHandTarget.getPointsRewarded()) destroyTimeRight = 5.0f;

            if(lastLeftHandTarget != null) Object.Destroy(lastLeftHandTarget.gameObject, destroyTimeLeft);
            if(lastRightHandTarget != null) Object.Destroy(lastRightHandTarget.gameObject, destroyTimeRight);
            timeCounter = -timeToWaitBetween;   // when timeToWaitBetween has passed for spawning the next pair, the timeCounter will be exactly zero
            instantiateTimeCounter = 0;
            instantiated = false;
        }
    }

    private void findReferencesToHitInteractables() // assign the correct references to the hitInteractables of controller gameobject elements,
                                                    // this is necessary in order to later write the sport game data
    {
        HitInteractable[] lefts = leftHandController.GetComponentsInChildren<HitInteractable>(false);
        Debug.Log(lefts.Length);
        foreach(HitInteractable hi in lefts)
        {
            if (!hi.CompareTag("VRTK_Model")) leftHand = hi;
        }
        HitInteractable[] rights = rightHandController.GetComponentsInChildren<HitInteractable>(false);
        Debug.Log(rights.Length);
        foreach (HitInteractable hi in rights)
        {
            if (!hi.CompareTag("VRTK_Model")) rightHand = hi;
        }
    }

    private void serializeSportGameData() // write sport game data to xml file
    {
        SerializeData.SerializeSportGameData("sportGameData", leftHand.listTimeNeededToHitObject, leftHand.listPrecisionWithThatObjectWasHit, leftHand.listEarnedPoints,
              rightHand.listTimeNeededToHitObject, rightHand.listPrecisionWithThatObjectWasHit, rightHand.listEarnedPoints, objectsSpawned, leftHand.countObjectsHit, rightHand.countObjectsHit,
              leftHand.listBlockPairNumber, rightHand.listBlockPairNumber, leftHand.listDifficulty, rightHand.listDifficulty);
    }

    private void serializeBlockGameData() // write sport game data to xml file
    {
        foreach (BlockSequence bs in blockSequences)
        {
            listCubePairCount.Add(bs.getCubePairCount());
            listIterationCount.Add(bs.getIterationCount());
            listBestPointScore.Add(bs.maxPointScoreOneIteration);
            listAvgPointScore.Add( (int) bs.getAvgPointScoreOneIteration());
            foreach (int pointScore in bs.pointScoreAllIterations)
            {
                listPointScoreAllIterations.Add(pointScore);
            }
        }
        SerializeData.SerializeBlocks("blockData", blockSequences, listCubePairCount, listIterationCount, listBestPointScore, listAvgPointScore, listPointScoreAllIterations);
    }

    void OnDrawGizmosSelected() // draws sphere radius of where the cubes will spawn around the player in editor mode
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }

    public bool bothCubesDestroyedOrHit()
    {
        return (lastLeftHandTarget == null || lastLeftHandTarget.getPointsRewarded()) && (lastRightHandTarget == null || lastRightHandTarget.getPointsRewarded());
    }

    private void spawnCubesForBothHandsInSightOfCameraDirection()
    {
        if (blockSequences == null || blockSequences.Length == 0) // no list of spawn points => randomized spawning of cubes
        {
            spawnRandomCubesForBothHands();
            instantiated = true;
        }
        else                                                       // spawning of elements in the current blockelement from blockarray ( => blockArray[blockCurrentIndex].getNextSphereCoordinates())
        {
            if (blockSequenceIndex >= blockSequences.Length) return;
            if (!blockSequences[blockSequenceIndex].hasNextSphereCoordinates())
            {
                breakTimer = 0f;
                ++blockSequenceIndex;
                return;
            }
            if (blockSequenceIndex >= blockSequences.Length) return;
            SphereCoordinates sc = blockSequences[blockSequenceIndex].nextSphereCoordinates();
            spawnCubesForSphereCoordinates(sc);
            instantiated = true;
        }
    }

    public void spawnCubesForSphereCoordinates(SphereCoordinates sc)
    {
        float degLeftPhi, degRightPhi, degLeftTheta, degRightTheta;
        float radiusLeft, radiusRight;
        Vector3 rotationLeft, rotationRight;
        Vector3 scaleLeft, scaleRight;
        GameObject leftToSpawn = leftHandGameObject, rightToSpawn = rightHandGameObject;
        int phiOffset = calculatePhiOffset();
        Debug.Log(phiOffset);


        if (!turnLoadedSequenceTowardsPlayer) phiOffset = 0;

        degLeftPhi = sc.phi2 + phiOffset;
        degLeftTheta = sc.theta2;
        radiusLeft = sc.radius2;
        rotationLeft = new Vector3(0, 0, sc.rotationZ2);

        degRightPhi = sc.phi + phiOffset;
        degRightTheta = sc.theta;
        radiusRight = sc.radius;
        rotationRight = new Vector3(0, 0, sc.rotationZ);

        int rotZ = Mathf.RoundToInt(sc.rotationZ2);
        int rotZ2 = Mathf.RoundToInt(sc.rotationZ);

        if (rotZ == 0 || rotZ == 90 || rotZ == 180 || rotZ == 270 || rotZ == 360
                || rotZ == -90 || rotZ == -180 || rotZ == -270)
        {
            leftToSpawn = leftHandGameObject;
        }
        if (rotZ == 45 || rotZ == 135 || rotZ == 225 || rotZ == 315 || rotZ == -45
                || rotZ == -135 || rotZ == -225 || rotZ == -315)
        {
            leftToSpawn = leftHandGameObjectDiagonal;
            rotationLeft = new Vector3(0, 0, sc.rotationZ2 - 45);
        }

        if (rotZ2 == 0 || rotZ2 == 90 || rotZ2 == 180 || rotZ2 == 270 || rotZ2 == 360
                || rotZ2 == -90 || rotZ2 == -180 || rotZ2 == -270)
        {
            rightToSpawn = rightHandGameObject;
        }
        if (rotZ2 == 45 || rotZ2 == 135 || rotZ2 == 225 || rotZ2 == 315 || rotZ2 == -45
                || rotZ2 == -135 || rotZ2 == -225 || rotZ2 == -315)
        {
            rightToSpawn = rightHandGameObjectDiagonal;
            rotationRight = new Vector3(0, 0, sc.rotationZ - 45);
        }

        if (useScaleFromSphereCoordinates)
        {
            scaleLeft = sc.scale2;
            scaleRight = sc.scale;
        }
        else
        {
            scaleLeft = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
            scaleRight = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        }

            //Debug.Log("DegLeftTheta: "+degLeftTheta);
            //Debug.Log("DegRightTheta: "+degRightTheta);
        ++roundGenerated;
        spawnGameObject(true, sphereToCartesianCoordinate(degLeftTheta, degLeftPhi, radiusLeft), rotationLeft, scaleLeft, leftToSpawn);
        spawnGameObject(false, sphereToCartesianCoordinate(degRightTheta, degRightPhi, radiusRight), rotationRight, scaleRight, rightToSpawn);
        objectsSpawned += 2;
    }

    private void spawnRandomCubesForBothHands()
    {
        float degLeftPhi, degRightPhi, degLeftTheta, degRightTheta;
        float radiusLeft, radiusRight;
        Vector3 rotationLeft, rotationRight;
        Vector3 scaleLeft, scaleRight;
        GameObject leftToSpawn = leftHandGameObject, rightToSpawn = rightHandGameObject;
        int phiOffset = calculatePhiOffset();
        Debug.Log(phiOffset);

        degLeftPhi = Random.Range(15, 40);
        degRightPhi = Random.Range(-40, -15);
        degLeftPhi += phiOffset;
        degRightPhi += phiOffset;
        degLeftTheta = 90;
        degRightTheta = 90;
        radiusLeft = sphereRadius;
        radiusRight = sphereRadius;
        rotationLeft = new Vector3(0, 0, 0);
        rotationRight = new Vector3(0, 0, 0);
        scaleLeft = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        scaleRight = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        leftToSpawn = leftHandGameObject;
        rightToSpawn = rightHandGameObject;
        
        ++roundGenerated;
        spawnGameObject(true, sphereToCartesianCoordinate(degLeftTheta, degLeftPhi, radiusLeft), rotationLeft, scaleLeft, leftToSpawn);
        spawnGameObject(false, sphereToCartesianCoordinate(degRightTheta, degRightPhi, radiusRight), rotationRight, scaleRight, rightToSpawn);
        objectsSpawned += 2;
    }

    private int calculatePhiOffset() // Interpolation of Phioffset for current Camera Direction
    {
        Vector3 forward = hmd_transform.forward;
        if (forward.x >= 0.99) return 0;
        if (forward.z >= 0.99) return 90;
        if (forward.x <= -0.99) return 180;
        if (forward.z <= -0.99) return 270;
        if (forward.x > 0 && forward.z > 0)
        {
            if(forward.z < 0.7) return (int) (forward.z * 45/ 0.7);
            else                return (int) (45 + ((forward.z - 0.7) * 45 / 0.3));
        }
        if (forward.x < 0 && forward.z > 0)
        {
            if(forward.x > -0.7) return (int) (90 + (forward.x * -45 / 0.7));
            else                 return (int) (135 + ((forward.x + 0.7) * -45 / 0.3));
        }
        if (forward.x < 0 && forward.z < 0)
        {
            if(forward.z > -0.7) return (int)(180 + (forward.z * -45 / 0.7));
            else                 return (int)(225 + ((forward.z + 0.7) * -45 / 0.3));
        }
        if (forward.x > 0 && forward.z < 0)
        {
            if(forward.x < 0.7) return (int)(270 + (forward.x * 45 / 0.7));
            else                return (int)(305 + ((forward.x - 0.7) * 45 / 0.3));
        }
        else return 0;
    }

    private Vector3 sphereToCartesianCoordinate(float theta, float phi)
    {
        return sphereToCartesianCoordinate(theta, phi, sphereRadius);
    }

    public Vector3 sphereToCartesianCoordinate(float theta, float phi, float radius)
    {
        float x = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi)) + hmd_transform.position.x;
        float z = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi)) + hmd_transform.position.z;
        float y = radius * Mathf.Cos(DegToRadians(theta)) + hmd_transform.position.y;
        return new Vector3(x, y, z);
    }

    public Vector3 cartesianToSphereCoordinate(Vector3 position)
    {
        Vector3 result = new Vector3();
        float radius = (position - hmd_transform.position).magnitude;
        /*Debug.Log("Radius: " + radius);
        if(position.x == 0)
        {
            result[0] = Mathf.Rad2Deg * Mathf.Atan(position.z / Mathf.Epsilon);
        }
        else result[0] = Mathf.Atan(position.z / position.x);
        if(position.x < 0)
        {
            result[0] += Mathf.PI;
        }
       result[0] = Mathf.Rad2Deg * result[0];
       result[1] = Mathf.Rad2Deg * Mathf.Asin(position.y - 1.6f / radius);*/
        // working implementation

        result[0] = Mathf.Rad2Deg * Mathf.Atan2(position.z - hmd_transform.position.z, position.x - hmd_transform.position.x);
        result[1] = Mathf.Rad2Deg * Mathf.Acos((position.y - hmd_transform.position.y) / radius);
        result[2] = radius;
        return result;
    }

    private float DegToRadians(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    public void spawnGameObject(bool leftHand, SphereCoordinates sc, GameObject objectToSpawn) // used for spawning of neutral red and blue cube (without arrows) in order to show the real positions where the cubes would spawn
    {
        Vector3 scale, position, rotation;
        float radius;
        float degPhi, degTheta;
        int phiOffset = calculatePhiOffset();
        if (!turnLoadedSequenceTowardsPlayer) phiOffset = 0;
        if (leftHand)
        {
            degPhi = sc.phi2 + phiOffset;
            degTheta = sc.theta2;
            radius = sc.radius2;
            if (useScaleFromSphereCoordinates)
            {
                scale = sc.scale2;
            }
            else
            {
                scale= new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
            }
            rotation = new Vector3(0, 0, 0);
        }
        else
        {
            degPhi = sc.phi + phiOffset;
            degTheta = sc.theta;
            radius = sc.radius;
            if (useScaleFromSphereCoordinates)
            {
                scale = sc.scale;
            }
            else
            {
                scale = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
            }
            rotation = new Vector3(0, 0, 0);
        }
        position = sphereToCartesianCoordinate(degTheta, degPhi, radius);
        spawnGameObject(leftHand, position, rotation, scale, objectToSpawn);
    }

    public void spawnGameObject(bool leftHand, Vector3 position, Vector3 rotationVector, Vector3 scaleVector, GameObject objectToSpawn)
    {
        Vector3 normalizedOffset = new Vector3(0, 0, 0);
        if(animatedSpawning)
        {
            //Vector3 directionTowardsGameObject = position - hmd_transform.position;
            Vector3 directionTowardsGameObject = leftHand ?  hmd_transform.forward - new Vector3(0.05f,0,0) : hmd_transform.forward + new Vector3(0.05f,0,0);
            directionTowardsGameObject.y = 0;
            normalizedOffset = directionTowardsGameObject.normalized * 20.0f;
            Vector3 movedPosition = position + normalizedOffset;
            position = movedPosition;
        }
      
        GameObject gameObject = Instantiate(objectToSpawn, position, Quaternion.identity);
        gameObject.transform.localScale = scaleVector;
        gameObject.transform.localEulerAngles = rotationVector;
        //gameObject.transform.LookAt(hmd_transform.position);
        SpawnedInteractable si = gameObject.GetComponent<SpawnedInteractable>();
        si.setMovedOffset(normalizedOffset);
        si.setTimeToHitObjects(timeToHitGameObjects);
        si.roundGenerated = roundGenerated;
        si.setHmd_transform(hmd_transform);
        si.lookAt();
        listOfSpawnedCubes.Add(si);
        if (leftHand)
        {
            lastLeftHandTarget = si;
            transformLeftCube = new TransformData(gameObject.transform);
        }
        else
        {
            lastRightHandTarget = si;
            transformRightCube = new TransformData(gameObject.transform);
        }
    }

    public void deleteAllSpawnedCubes()
    {
        foreach(SpawnedInteractable si in listOfSpawnedCubes)
        {
            if (si != null) Object.Destroy(si.gameObject);
        }
        listOfSpawnedCubes.Clear();
    }

    public void updateDifficultyParameters()
    {
        DifficultyManager inst = DifficultyManager.Instance;
        timeToHitGameObjects = inst.getTimeToHitObjects();
        scaleSpawnedGameObjects = inst.getScaleObjects();
        Debug.Log("Scale: " + scaleSpawnedGameObjects);
    }

    public void updateDifficultyParameters(float time, float scale)
    {
        timeToHitGameObjects = time;
        scaleSpawnedGameObjects = scale;
    }

    public void updateSphereRadius(float sphereRadiusN)
    {
        sphereRadius = sphereRadiusN;
    }
}
