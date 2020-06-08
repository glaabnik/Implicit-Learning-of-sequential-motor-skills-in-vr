using System.Collections;
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
    public HitInteractable leftHand;
    public HitInteractable rightHand;
    public GameObject rightHandGameObject;
    public GameObject leftHandGameObject;
    public float sphereRadius;
    public int timeToHitGameObjects;
    public int timeToWaitBetween;
    public float scaleSpawnedGameObjects = 1.0f;
    public bool randomizeElevation = false;
    public int offsetLeftSide = 40;
    public int offsetRightSide = 40;
    public bool animatedSpawning = true;
    public bool loadSequenceFromCsv = true;
    public bool turnLoadedSequenceTowardsPlayer = false;
    public string filenameCsv;
    // Debug Variables
    public Vector3 forwardVectorTest;

    private List<SphereCoordinates> sequenceOfSpawns;
    private float timeCounter;
    private float instantiateTimeCounter;
    private bool instantiated;
    private SpawnedInteractable lastLeftHandTarget;
    private SpawnedInteractable lastRightHandTarget;
    private int objectsSpawned = 0;
    private int roundGenerated = 0;
    void Start()
    {
        sequenceOfSpawns = new List<SphereCoordinates>();
        if (loadSequenceFromCsv)
        {
            if (!string.IsNullOrEmpty(filenameCsv)) LoadFixedSequenceOfSpawns.loadSpawnSequence(ref sequenceOfSpawns, filenameCsv);
        }
        timeCounter = 0;
        instantiateTimeCounter = timeToWaitBetween + 1;
        forwardVectorTest = hmd_transform.forward;
        spawnCubesForBothHandsInSightOfCameraDirection();
        instantiated = true;
       
        
        Debug.Log(" Size of List: "+sequenceOfSpawns.Count);
    }

    // Update is called once per frame
    void Update()
    {
        float animationTimeBonus = animatedSpawning ? 2.0f : 0.0f;

        if(instantiated && lastLeftHandTarget.getPointsRewarded() && lastRightHandTarget.getPointsRewarded())
        {
            timeCounter = timeToHitGameObjects + animationTimeBonus;
        }
        forwardVectorTest = hmd_transform.forward;
        timeCounter += Time.deltaTime;
        instantiateTimeCounter += Time.deltaTime;
        if(instantiateTimeCounter >= timeToWaitBetween && !instantiated)
        {
            spawnCubesForBothHandsInSightOfCameraDirection();
            instantiated = true;
        }
        if(timeCounter >= timeToHitGameObjects + animationTimeBonus)
        {
            float destroyTimeLeft = 0;
            float destroyTimeRight = 0;
            if (lastLeftHandTarget.getPointsRewarded()) destroyTimeLeft = 5.0f;
            if (lastRightHandTarget.getPointsRewarded()) destroyTimeRight = 5.0f;

            Object.Destroy(lastLeftHandTarget.gameObject, destroyTimeLeft);
            Object.Destroy(lastRightHandTarget.gameObject, destroyTimeRight);
            timeCounter = -timeToWaitBetween;
            instantiateTimeCounter = 0;
            instantiated = false;
        }
        bool inputS = Input.GetKey(KeyCode.L);
        if(inputS)
        {
            Debug.Log("L pressed");
            SerializeData.SerializeSportGameData("sportGameData", leftHand.listTimeNeededToHitObject, leftHand.listPrecisionWithThatObjectWasHit, leftHand.listEarnedPoints,
                rightHand.listTimeNeededToHitObject, rightHand.listPrecisionWithThatObjectWasHit, rightHand.listEarnedPoints, objectsSpawned, leftHand.countObjectsHit, rightHand.countObjectsHit);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hmd_transform.position, sphereRadius);
    }

    private void spawnCubesForBothHandsInSightOfCameraDirection()
    {
        int degLeftPhi, degRightPhi, degLeftTheta, degRightTheta;
        float radiusLeft, radiusRight;
        int phiOffset = calculatePhiOffset();
        Debug.Log(phiOffset);
        if (sequenceOfSpawns == null || sequenceOfSpawns.Count == 0) // no list of spawn points => randomized spawning of cubes
        {
            degLeftPhi = Random.Range(15, offsetLeftSide);
            degRightPhi = Random.Range(-offsetRightSide, -15);
            degLeftPhi += phiOffset;
            degRightPhi += phiOffset;
            degLeftTheta = 90;
            degRightTheta = 90;
            radiusLeft = sphereRadius;
            radiusRight = sphereRadius;
            if (randomizeElevation)
            {
                degLeftTheta = 70 + Random.Range(0, 40);
                degRightTheta = 70 + Random.Range(0, 40);
            }
        }
        else                                                          // spawning of blocks in points loaded from csv file
        {
            if (!turnLoadedSequenceTowardsPlayer) phiOffset = 0;
            int listSize = sequenceOfSpawns.Count;
            int actIndexInList = (objectsSpawned / 2) % listSize;
            degLeftPhi = (int) sequenceOfSpawns[actIndexInList].phi + phiOffset;
            degLeftTheta = (int)sequenceOfSpawns[actIndexInList].theta;
            radiusLeft = sequenceOfSpawns[actIndexInList].radius;
            degRightPhi = (int)sequenceOfSpawns[actIndexInList].phi2 + phiOffset;
            degRightTheta = (int)sequenceOfSpawns[actIndexInList].theta2;
            radiusRight = sequenceOfSpawns[actIndexInList].radius2;
            Debug.Log("DegLeftTheta: "+degLeftTheta);
            Debug.Log("DegRightTheta: "+degRightTheta);
        }
        ++roundGenerated;
        spawnGameObject(true, sphereToCartesianCoordinate(degLeftTheta, degLeftPhi, radiusLeft), leftHandGameObject);
        spawnGameObject(false, sphereToCartesianCoordinate(degRightTheta, degRightPhi, radiusRight), rightHandGameObject);
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
        float x = radius * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi)) + hmd_transform.position.x;
        float z = radius * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi)) + hmd_transform.position.z;
        float y = radius * Mathf.Cos(DegToRadians(theta)) + hmd_transform.position.y;
        return new Vector3(x, y, z);
    }

    private float DegToRadians(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    private void spawnGameObject(bool leftHand, Vector3 position, GameObject objectToSpawn)
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
        gameObject.transform.localScale = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        gameObject.transform.LookAt(hmd_transform.position);
        SpawnedInteractable si = gameObject.GetComponent<SpawnedInteractable>();
        si.setMovedOffset(normalizedOffset);
        si.setTimeToHitObjects(timeToHitGameObjects);
        si.roundGenerated = roundGenerated;
        si.setHmd_transform(hmd_transform);
        if (leftHand) lastLeftHandTarget = si;
        else lastRightHandTarget = si;
    }
}
