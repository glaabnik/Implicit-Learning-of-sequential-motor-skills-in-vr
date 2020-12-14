﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public float sphereRadius;
    public float timeToHitGameObjects;
    public float timeToWaitBetween;
    public float scaleSpawnedGameObjects = 1.0f;
    public bool animatedSpawning = true;
    public bool useScaleFromSphereCoordinates = false;
    public bool turnLoadedSequenceTowardsPlayer = false;
    public bool playBackgroundMusic = true;
    public bool noAutomatedSpawning = false;
    public BlockSequence[] blockSequences;

    // Debug Variables
    public Vector3 forwardVectorTest;

    private HitInteractable leftHand;
    private HitInteractable rightHand;
    private int blockSequenceIndex = 0;
    private float timeCounter;
    private float instantiateTimeCounter;
    private bool instantiated;
    private SpawnedInteractable lastLeftHandTarget;
    private SpawnedInteractable lastRightHandTarget;
    private int objectsSpawned = 0;
    private int roundGenerated = 0;
    private bool fileWritten = false;
    void Start()
    {
        timeCounter = 0;
        instantiateTimeCounter = timeToWaitBetween + 1;
        forwardVectorTest = hmd_transform.forward;
        if (playBackgroundMusic) SoundManager.Instance.PlayBackgroundMusic();
        findReferencesToHitInteractables();
    }

    // Update is called once per frame
    void Update()
    {
        if (noAutomatedSpawning || fileWritten || DifficultyManager.Instance == null || DifficultyManager.Instance.gamePaused) return;

        if (blockSequences != null && blockSequences.Length > 0 && blockSequenceIndex >= blockSequences.Length && !fileWritten) // check if there is a valid blockSequenceArrray and all elements of the block array
                                                                                                                                // were allready spawned, then write the gathered sportgamedata to file
                                                                                                                                // from now on there will nothing happen here anymore
        {
            serializeSportGameData();
            fileWritten = true;
        }

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
            instantiated = true;
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
        }
        else                                                       // spawning of elements in the current blockelement from blockarray ( => blockArray[blockCurrentIndex].getNextSphereCoordinates())
        {
            if (blockSequenceIndex >= blockSequences.Length) return;
            if (!blockSequences[blockSequenceIndex].hasNextSphereCoordinates()) ++blockSequenceIndex;
            if (blockSequenceIndex >= blockSequences.Length) return;
            SphereCoordinates sc = blockSequences[blockSequenceIndex].nextSphereCoordinates();
            spawnCubesForSphereCoordinates(sc);
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

        degLeftPhi = sc.phi + phiOffset;
        degLeftTheta = sc.theta;
        radiusLeft = sc.radius;
        rotationLeft = new Vector3(0, 0, sc.rotationZ);

        degRightPhi = sc.phi2 + phiOffset;
        degRightTheta = sc.theta2;
        radiusRight = sc.radius2;
        rotationRight = new Vector3(0, 0, sc.rotationZ2);

        if (sc.rotationZ == 0 || sc.rotationZ == 90 || sc.rotationZ == 180 || sc.rotationZ == 270 || sc.rotationZ == 360
                || sc.rotationZ == -90 || sc.rotationZ == -180 || sc.rotationZ == -270)
        {
            leftToSpawn = leftHandGameObject;
        }
        if (sc.rotationZ == 45 || sc.rotationZ == 135 || sc.rotationZ == 225 || sc.rotationZ == 315 || sc.rotationZ == -45
                || sc.rotationZ == -135 || sc.rotationZ == -225 || sc.rotationZ == -315)
        {
            leftToSpawn = leftHandGameObjectDiagonal;
            rotationLeft = new Vector3(0, 0, sc.rotationZ - 45);
        }

        if (sc.rotationZ2 == 0 || sc.rotationZ2 == 90 || sc.rotationZ2 == 180 || sc.rotationZ2 == 270 || sc.rotationZ2 == 360
                || sc.rotationZ2 == -90 || sc.rotationZ2 == -180 || sc.rotationZ2 == -270)
        {
            rightToSpawn = rightHandGameObject;
        }
        if (sc.rotationZ2 == 45 || sc.rotationZ2 == 135 || sc.rotationZ2 == 225 || sc.rotationZ2 == 315 || sc.rotationZ2 == -45
                || sc.rotationZ2 == -135 || sc.rotationZ2 == -225 || sc.rotationZ2 == -315)
        {
            rightToSpawn = rightHandGameObjectDiagonal;
            rotationRight = new Vector3(0, 0, sc.rotationZ2 - 45);
        }

        if (useScaleFromSphereCoordinates)
        {
            scaleLeft = sc.scale;
            scaleRight = sc.scale2;
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

    private Vector3 sphereToCartesianCoordinate(float theta, float phi, float radius)
    {
        float x = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi)) + hmd_transform.position.x;
        float z = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi)) + hmd_transform.position.z;
        float y = radius * Mathf.Cos(DegToRadians(theta)) + hmd_transform.position.y;
        return new Vector3(x, y, z);
    }

    private float DegToRadians(float degree)
    {
        return degree * Mathf.PI / 180;
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
        if (leftHand) lastLeftHandTarget = si;
        else lastRightHandTarget = si;
    }

    public void updateDifficultyParameters()
    {
        DifficultyManager inst = DifficultyManager.Instance;
        timeToHitGameObjects = inst.getTimeToHitObjects();
        scaleSpawnedGameObjects = inst.getScaleObjects();
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
